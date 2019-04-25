#region license
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

using Origam.DA.Service;
using Origam.Schema.DeploymentModel;
using Origam.Schema.EntityModel;
using Origam.Workbench.Services.CoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Origam.DA.Common.Enums;

namespace Origam.UI
{
	/// <summary>
	/// Summary description for AbstractMenuCommand.
	/// </summary>
	public abstract class AbstractMenuCommand : AbstractCommand, IMenuCommand
	{
	    public virtual bool IsEnabled { get; set; } = true;

        #region Property

        public ServiceCommandUpdateScriptActivity CreateTableScript(string name, Guid guid)
        {
            AbstractSqlDataService abstractSqlData = (AbstractSqlDataService)DataService.GetDataService();
            string script = abstractSqlData.EntityDdl(guid);
            OrigamSettings settings = ConfigurationManager.GetActiveConfiguration();
            if (settings.DeployPlatforms != null)
            {
                foreach (Platform platform in settings.DeployPlatforms)
                {
                    AbstractSqlDataService DsPlatform = (AbstractSqlDataService)DataService.GetDataService(platform);
                    string platformscript = DsPlatform.EntityDdl(guid);
                    ServiceCommandUpdateScriptActivity _create = DeploymentHelper.CreateDatabaseScript(name, platformscript, DsPlatform.PlatformName);
                    DsPlatform.Dispose();
                }
            }
            return DeploymentHelper.CreateDatabaseScript(name, script, abstractSqlData.PlatformName);
        }
      
        public ServiceCommandUpdateScriptActivity CreateDatabaseScript(string name, IDictionary<AbstractSqlDataService, StringBuilder> dict)
        {
            ServiceCommandUpdateScriptActivity create = null;
            for (int index = 0; index < dict.Count; index++)
            {
                var item = dict.ElementAt(index);
                create = 
                    DeploymentHelper.CreateDatabaseScript(name, ((StringBuilder)item.Value).ToString(), 
                                                                ((AbstractSqlDataService)item.Key).PlatformName);
            }
            return create;
        }
        public IDictionary<AbstractSqlDataService, StringBuilder> InitDictionary()
        {
            IDictionary<AbstractSqlDataService, StringBuilder> dict = new Dictionary<AbstractSqlDataService, StringBuilder>();
            AbstractSqlDataService abstractSqlData = (AbstractSqlDataService)DataService.GetDataService();
            OrigamSettings settings = ConfigurationManager.GetActiveConfiguration();
            if (settings.DeployPlatforms != null)
            {
                foreach (Platform platform in settings.DeployPlatforms)
                {
                    AbstractSqlDataService DsPlatform = (AbstractSqlDataService)DataService.GetDataService(platform);
                    dict.Add(DsPlatform, new StringBuilder());
                }
            }
            dict.Add(abstractSqlData, new StringBuilder());
            return dict;
        }
        public void FieldsScripts(FieldMappingItem fk, FieldMappingItem baseField, IDataEntity baseEntity)
        {
            OrigamSettings settings = ConfigurationManager.GetActiveConfiguration();
            string[] fkDdl = DataService.FieldDdl(fk.Id);
            int i = 0;
            foreach (string ddl in fkDdl)
            {
                // if the foreign key is based on an existing field 
                // take only the foreign key ddl
                if (baseField == null || i == 1)
                {
                    var script3 = DeploymentHelper.CreateDatabaseScript(baseEntity.Name + "_" + fk.Name, ddl,
                        ((AbstractSqlDataService)DataService.GetDataService()).PlatformName);
                    GeneratedModelElements.Add(script3);
                }
                i++;
            }
            if (settings.DeployPlatforms != null)
            {
                foreach (Platform platform in settings.DeployPlatforms)
                {
                    AbstractSqlDataService DsPlatform = (AbstractSqlDataService)DataService.GetDataService(platform);
                    fkDdl = DsPlatform.FieldDdl(fk.Id);
                    i = 0;
                    foreach (string ddl in fkDdl)
                    {
                        // if the foreign key is based on an existing field 
                        // take only the foreign key ddl
                        if (baseField == null || i == 1)
                        {
                            var script3 = DeploymentHelper.CreateDatabaseScript(baseEntity.Name + "_" + fk.Name, ddl,DsPlatform.PlatformName);
                        }
                        i++;
                    }
                    DsPlatform.Dispose();
                }
            }
        }
        public ServiceCommandUpdateScriptActivity CreateRole(string role)
        {
            OrigamSettings settings = ConfigurationManager.GetActiveConfiguration();
            AbstractSqlDataService abstractSqlData = (AbstractSqlDataService)DataService.GetDataService();
            if (settings.DeployPlatforms != null)
            {
                foreach (Platform platform in settings.DeployPlatforms)
                {
                    AbstractSqlDataService DsPlatform = (AbstractSqlDataService)DataService.GetDataService(platform);
                    ServiceCommandUpdateScriptActivity _create = DeploymentHelper.CreateSystemRole(role, DsPlatform);
                    DsPlatform.Dispose();
                }
            }
            return DeploymentHelper.CreateSystemRole(role,abstractSqlData);
        }
        #endregion
        #region IDisposable Members

        public virtual void Dispose()
		{
		}

		#endregion
	}
}
