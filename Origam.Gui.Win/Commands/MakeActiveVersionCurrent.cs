#region license
/*
Copyright 2005 - 2020 Advantage Solutions, s. r. o.

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

using System;
using Origam.Schema.DeploymentModel;
using Origam.UI;
using Origam.Workbench;
using Origam.Workbench.Services;

namespace Origam.Gui.Win.Commands
{
    /// <summary>
    /// Makes the selected version the current version of the package
    /// </summary>
    public class MakeActiveVersionCurrent : AbstractMenuCommand
    {
        WorkbenchSchemaService _schemaService = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;
        SchemaBrowser _schemaBrowser = WorkbenchSingleton.Workbench.GetPad(typeof(SchemaBrowser)) as SchemaBrowser;
        public override bool IsEnabled
        {
            get
            {
                return Owner is DeploymentVersion 
                       && (Owner as DeploymentVersion).IsCurrentVersion == false 
                       & (Owner as DeploymentVersion).SchemaExtension.PrimaryKey.Equals(
                           _schemaService.ActiveExtension.PrimaryKey);
            }
            set
            {
                throw new ArgumentException("Cannot set this property", "IsEnabled");
            }
        }

        public override void Run()
        {
            MakeVersionCurrent cmd = new MakeVersionCurrent();
            cmd.Owner = Owner as DeploymentVersion;
            cmd.Run();
        }

        public override void Dispose()
        {
            _schemaService = null;
        }
        public override int GetImageIndex(string icon)
        {
            return _schemaBrowser.ImageIndex(icon);
        }
    }
}