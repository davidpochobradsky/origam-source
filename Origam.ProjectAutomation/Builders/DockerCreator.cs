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
using System.Diagnostics;
using System.Threading;

namespace Origam.ProjectAutomation.Builders
{
    public class DockerCreator : AbstractBuilder
    {
        public override string Name => "Start Docker Container";
        private string VolumeName;
        private string DockerExePath = @"c:\Program Files\Docker\Docker\resources\bin\docker.exe";

        public override void Execute(Project project)
        {
            IsDockerInstaled();
            VolumeName = project.Name;
            DoesContainerExist();
            string DatabaseAdminPassword = Project.CreatePassword();
            Process.Start(DockerExePath, " volume create " + project.Name);
            Thread.Sleep(2000);
            RunDocker(DatabaseAdminPassword, project);
            project.DatabaseUserName = "postgres";
            project.DatabasePassword = DatabaseAdminPassword;
            if(!WaitForDocker())
            { 
                throw new Exception("Docker didn't start. Please check Docker logs.");
            }
        }
        private void DoesContainerExist()
        {
            string output = DockerManager.GetDockerConsole("volume list");
            if (output.Contains("VolumeName"))
            {
                throw new Exception("Data Postgres container already exists.");
            }
        }
        private bool WaitForDocker()
        {
            long dockerdateTime = DateTime.Now.AddSeconds(60).Ticks;
            while (DateTime.Now.Ticks < dockerdateTime)
            {
                Thread.Sleep(5000);
                if (IsDockerRunning())
                {
                    return true;
                }
            }
            return false;
        }
        private void RunDocker(string databaseAdminPassword, Project project)
        {
            //string attrib = " run --env-file " + project.DockerEnvPath +
            //    " -e PG_Origam_Password=" + databaseAdminPassword + " -it " +
            //    "--name " + project.Name + " --mount source=" + project.Name + ",target=/var/lib/postgresql " +
            //    " -v " + project.SourcesFolder + ":/home/origam/HTML5/data/origam -p " +
            //    project.DockerPort.ToString() + ":8080 -p 5433:5433 origam/server:pg_master-latest";
            string argument = string.Format(" run --env-file {0} -e PG_Origam_Password={1} -it --name {2} " +
                "--mount source={3},target=/var/lib/postgresql -v {4}:/home/origam/HTML5/data/origam -p {5}:8080 " +
                "-p 5433:5433 origam/server:pg_master-latest ",
                project.DockerEnvPath, databaseAdminPassword, project.Name,project.Name, project.SourcesFolder,
                project.DockerPort.ToString());
            DockerManager.RunDocker(argument);
        }
        private bool IsDockerRunning()
        {
            string output = DockerManager.GetDockerConsole("logs " + VolumeName);
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
        private void IsDockerInstaled()
        {
            string output = DockerManager.GetDockerConsole("ps");
            if (!output.Contains("COMMAND"))
            {
                throw new Exception("Docker Desktop is not installed or is not running.");
            }
        }
        public override void Rollback()
        {
        }
    }
}
