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

using System;
using Origam.Schema.EntityModel;
using Origam.UI;
using Origam.Workbench.Services;

namespace Origam.Gui.Win.Commands
{
    public class GenerateDataStructureEntityColumns : AbstractMenuCommand
    {
        WorkbenchSchemaService _schema = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;

        public override bool IsEnabled
        {
            get
            {
                return Owner is DataStructureEntity;
            }
            set
            {
                throw new ArgumentException("Cannot set this property", "IsEnabled");
            }
        }

        public override void Run()
        {
            DataStructureEntity entity = Owner as DataStructureEntity;
            entity.AllFields = false;

            foreach(DataStructureColumn col in entity.Columns)
            {
                col.IsDeleted = true;
                col.Persist();
            }

            foreach(IDataEntityColumn column in entity.EntityDefinition.EntityColumns)
            {
                DataStructureColumn newColumn = entity.NewItem(typeof(DataStructureColumn), 
                    _schema.ActiveSchemaExtensionId, null) as DataStructureColumn;
                newColumn.Field = column;
                newColumn.Name = column.Name;
                newColumn.Persist();
            }
            entity.Persist();
        }

        public override void Dispose()
        {
            _schema = null;
        }

    }
}