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

using Origam.Docker;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Origam.ProjectAutomation.Builders
{
    public class DockerCreator : AbstractBuilder
    {
        public override string Name => "Start Docker Container";
        private readonly DockerManager dockerManager = new DockerManager();
        public override void Execute(Project project)
        {
            if(!dockerManager.IsDockerInstaled())
            {
                throw new Exception("Docker is prerequired. Please install Docker.");
            }
            if(dockerManager.IsDockerVolumeAlreadyExists(project.Name))
            {
                throw new Exception(string.Format("Docker Volume {0} already exists. " +
                    "Please chose different Project Name.",project.Name));
            }
            dockerManager.CreateVolume(project.Name);
            string DatabaseAdminPassword = Project.CreatePassword();
            string containerId = StartDockerContainer(DatabaseAdminPassword, project);
            project.DatabaseUserName = "postgres";
            project.DatabasePassword = DatabaseAdminPassword;
            if(!IsContainerPrepared(containerId))
            { 
                throw new Exception(string.Format("Docker didn't start. " +
                    "Please check the container {0} logs.",project.Name));
            }
        }

        private bool IsContainerPrepared(string containerId)
        {
            long dockerdateTime = DateTime.Now.AddSeconds(60).Ticks;
            while (DateTime.Now.Ticks < dockerdateTime)
            {
                Thread.Sleep(5000);
                if (IsContainerRunningProperly(containerId))
                {
                    return true;
                }
            }
            return false;
        }
        private string StartDockerContainer(string databaseAdminPassword, Project project)
        {
            IDictionary<string, string> arguments = new Dictionary<string, string>();
            arguments.Add("DockerEnvPath",project.DockerEnvPath);
            arguments.Add("AdminPassword",databaseAdminPassword);
            arguments.Add("ProjectName",project.Name);
            arguments.Add("SourceFolder",project.SourcesFolder);
            arguments.Add("DockerPort",project.DockerPort.ToString());
            return dockerManager.StartDockerContainer(arguments,"origam/server:pg_master-latest");
        }
        private bool IsContainerRunningProperly(string containerId)
        {
            string output = dockerManager.GetDockerLogs(containerId);
            if (output.Contains("Press [CTRL+C] to stop"))
            {
                return true;
            }
            if (output.Contains("OrigamServer.dll"))
            {
                throw new Exception("Docker started with an error. Please check Docker logs.");
            }
            return false;
        }
        public override void Rollback()
        {
        }
    }
}
