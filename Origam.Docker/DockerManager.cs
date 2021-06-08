#region license
/*
Copyright 2005 - 2021 Advantage Solutions, s. r. o.

This file is part of ORIGAM (http://www.origam.org).

ORIGAM is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

ORIGAM is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with ORIGAM. If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Origam.Docker
{
    public static class DockerManager
    {
        private static  DockerClient client = new DockerClientConfiguration().CreateClient();
        private static bool finished = false;

        public static void IsDockerInstaled()
        {
            var task = Task.Run(async () =>
            {
                return await client.System.GetVersionAsync();
            });
            VersionResponse result = task.Result;
        }
        public static void CreateVolume(string name)
        {
            VolumesCreateParameters volumesCreateParameters = new VolumesCreateParameters
            {
                Name = name
            };
            var task = Task.Run(async () =>
            {
                return await client.Volumes.CreateAsync(volumesCreateParameters);
            });
            var result = task.Result;
        }

        public static bool GetDockerVolume(string volumeName)
        {
            var task = Task.Run(async () =>
            {
                return await client.Volumes.ListAsync();
            });
            return task.Result.Volumes.Where(volumelist => volumelist.Name == volumeName).Any();
        }
        public static void PullImage(string image, string tag)
        {
            finished = false;
            var progress = new Progress<JSONMessage>();
            progress.ProgressChanged += Progress_ProgressChanged;
            client.Images.CreateImageAsync(
                new ImagesCreateParameters()
                {
                    FromImage = image,
                    Tag = tag
                }, null,
                progress).ConfigureAwait(false);
        }

        public static string GetDockerLogs(string id)
        {
            string output;
            ContainerLogsParameters logparams = new ContainerLogsParameters { ShowStdout=true};
            var task = Task.Run(async () =>
            {
                return await client.Containers.GetContainerLogsAsync(id, logparams);
            });
            var streamlogs = task.Result;
            using (var reader = new StreamReader(streamlogs))
            {
                output = reader.ReadToEnd();
            }
            return output;
        }

        public static string RunDocker(IDictionary<string, string> arguments)
        {
            IsImagePulled();
            IList<string> env = new List<string>();
            string[] envfile = File.ReadAllLines(arguments["DockerEnvPath"]);
            env.Add(string.Format("PG_Origam_Password={0}", arguments["AdminPassword"]));
            foreach (string line in envfile)
            {
                env.Add(line);
            }

            IDictionary<string, EmptyStruct> volume = new Dictionary<string, EmptyStruct>();
            volume.Add("/var/lib/postgresql", new EmptyStruct { });

            IList<Mount> mounts = new List<Mount>
            {
                new Mount { Source = arguments["ProjectName"], Target = "/var/lib/postgresql",Type = "volume"},
                 new Mount { Source = arguments["SourceFolder"], Target = "/home/origam/HTML5/data/origam",Type = "bind"}
            };
            IDictionary<string, IList<PortBinding>> portbind = new Dictionary<string, IList<PortBinding>>
            {
                { "8080/tcp", new List<PortBinding> { new PortBinding {HostPort = arguments["DockerPort"] } }},
                { "5433/tcp", new List<PortBinding> { new PortBinding {HostPort = "5433" }}}
            };

            HostConfig hostconfig = new HostConfig
            {
                Mounts = mounts,
                PortBindings = portbind,
                PublishAllPorts = true
            };
            IDictionary<string, EmptyStruct> exposePort = new Dictionary<string, EmptyStruct>
            {
                { "5433/tcp", default },
                { "8080/tcp", default }
            };

            CreateContainerParameters create_containerParameters = new CreateContainerParameters
            {
                Name = arguments["ProjectName"],
                Image = "origam/server:pg_master-latest",
                Env = env,
                HostConfig = hostconfig,
                ExposedPorts = exposePort
            };

            var containerTask = Task.Run(async () =>
            {
                return await client.Containers.CreateContainerAsync(
                                    create_containerParameters);
            });
            var createcontainer = containerTask.Result;

            var startContanerTask = Task.Run(async () =>
            {
                return await client.Containers.StartContainerAsync(createcontainer.ID,
                                    new ContainerStartParameters());
            });
            if (startContanerTask.Result)
            {
                return createcontainer.ID;
            }
            throw new Exception("Docker Container doesnt start. Please check a log.");
        }

        private static void IsImagePulled()
        {
            long dockerdateTime = DateTime.Now.AddSeconds(60).Ticks;
            while (DateTime.Now.Ticks < dockerdateTime)
            {
                Thread.Sleep(5000);
                if (finished)
                {
                    return;
                }
            }

        }

        private static void Progress_ProgressChanged(object sender, JSONMessage prog_msg)
        {
            if(prog_msg.Status.Contains("Status: Downloaded"))
            {
                finished = true;
            }
            if (prog_msg.Status.Contains("Status: Image is up to date"))
            {
                finished = true;
            }
        }
    }
}
