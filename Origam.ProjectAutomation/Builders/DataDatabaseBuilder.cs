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

using static Origam.DA.Common.Enums;

namespace Origam.ProjectAutomation
{
    public class DataDatabaseBuilder : AbstractDatabaseBuilder
    {
        string _databaseName;
        DatabaseType _databaseType;

        public override string Name
        {
            get
            {
                return "Create Data Database";
            }
        }

        public override void Execute(Project project)
        {
            _databaseType = project.DatabaseType;
            _databaseName = project.DataDatabaseName;
            DataService(_databaseType).ConnectionString = DataService(_databaseType).BuildConnectionString(
                project.DatabaseServerName,project.Port, "", project.DatabaseUserName,
                project.DatabasePassword, project.DatabaseIntegratedAuthentication, false);
            this.DataService(_databaseType).CreateDatabase(_databaseName);
        }

        public string BuildConnectionString(Project project, bool pooling)
        {
            _databaseType = project.DatabaseType;
            return DataService(project.DatabaseType).BuildConnectionString(project.DatabaseServerName,project.Port,
                project.DataDatabaseName, project.DatabaseUserName,
                project.DatabasePassword, project.DatabaseIntegratedAuthentication, pooling);
        }
        public string BuildConnectionStringArchitect(Project project, bool pooling)
        {
            _databaseType = project.DatabaseType;
            if(_databaseType==DatabaseType.MsSql)
            {
                return BuildConnectionString(project, pooling);
            }
            if (_databaseType == DatabaseType.PostgreSql)
            {
                this.DataService(_databaseType).DbUser=project.Name;
                return DataService(project.DatabaseType).BuildConnectionString(project.DatabaseServerName, project.Port,
                    project.DataDatabaseName, DataService(_databaseType).DbUser,
                     DataService(_databaseType).DBPassword, project.DatabaseIntegratedAuthentication, pooling);
            }
            return null;
        }
        public override void Rollback()
        {
            OrigamUserContext.Reset();
            this.DataService(_databaseType).DeleteDatabase(_databaseName);
        }
    }
}
