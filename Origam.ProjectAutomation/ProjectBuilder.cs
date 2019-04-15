﻿#region license
/*
Copyright 2005 - 2019 Advantage Solutions, s. r. o.

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

using Origam.ProjectAutomation.Builders;
using System.Collections.Generic;

namespace Origam.ProjectAutomation
{
    public class ProjectBuilder
    {
        private readonly List<IProjectBuilder> tasks = new List<IProjectBuilder>();
        private readonly SettingsBuilder settingsBuilder = new SettingsBuilder();
        private readonly DataDatabaseBuilder dataDatabaseBuilder = new DataDatabaseBuilder();
        private readonly ConfigureWebServerBuilder configureWebServerBuilder = new ConfigureWebServerBuilder();

        public ProjectBuilder()
        {           
            tasks.Add(settingsBuilder);
            tasks.Add(dataDatabaseBuilder);
            tasks.Add(new FileModelImportBuilder());
            tasks.Add(new FileModelInitBuilder());
            tasks.Add(new DataDatabaseStructureBuilder());
            tasks.Add(new SettingsFinalConnectionStringBuilder());
            tasks.Add(new CopyServerFilesBuilder());
            tasks.Add(new ModifyConfigurationFilesBuilder());
            tasks.Add(configureWebServerBuilder);
            tasks.Add(new ApplyDatabasePermissionsBuilder());
            tasks.Add(new NewPackageBuilder());
            tasks.Add(new GitBuilder());
        }

        public void Create(Project project)
        {
            project.DataConnectionString = 
                dataDatabaseBuilder.BuildConnectionString(project, true);
            project.BuilderDataConnectionString = 
                dataDatabaseBuilder.BuildConnectionString(project, false);
            project.BaseUrl =
                configureWebServerBuilder.WebSiteUrl(project.WebRootName);

            IProjectBuilder activeTask = null;
            try
            {
                foreach (IProjectBuilder builder in tasks)
                {
                    activeTask = builder;
                    builder.State = TaskState.Running;
                    builder.Execute(project);
                    builder.State = TaskState.Finished;
                }
            }
            catch
            {
                activeTask.State = TaskState.Failed;
                for (int i = tasks.Count - 1; i >= 0; i--)
                {
                    Rollback(tasks[i]);
                }
                throw;
            }
        }

        #region Properties
        public List<IProjectBuilder> Tasks => tasks;

        public string[] WebSites() => configureWebServerBuilder.WebSites();
        #endregion

        private void Rollback(IProjectBuilder builder)
        {
            try
            {
                if(builder.State == TaskState.Finished)
                {
                    builder.State = TaskState.RollingBack;
                    builder.Rollback();
                    builder.State = TaskState.RolledBack;
                }
            }
            catch
            {
                builder.State = TaskState.RollbackFailed;
            }
        }
    }
}
