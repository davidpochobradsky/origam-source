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

using System.Collections;
using System.Text;
using Origam.DA.Service;
using Origam.Schema.EntityModel;
using Origam.UI;
using Origam.Workbench.Services;
using Origam.Workbench.Services.CoreServices;

namespace Origam.Gui.Win.Commands
{
    public class ShowDataStructureFilterSetSql : AbstractMenuCommand
    {
        private WorkbenchSchemaService _schema =
            ServiceManager.Services.GetService(
                typeof(SchemaService)) as WorkbenchSchemaService;

        public override bool IsEnabled
        {
            get
            {
                return Owner is DataStructureFilterSet;
            }
            set
            {
                base.IsEnabled = value;
            }
        }

        public override void Run()
        {
            AbstractSqlDataService abstractSqlDataService = DataService.GetDataService() as AbstractSqlDataService;
            AbstractSqlCommandGenerator generator = (AbstractSqlCommandGenerator)abstractSqlDataService.DbDataAdapterFactory.Clone();
            DataStructureFilterSet filterSet = Owner as DataStructureFilterSet;
            generator.PrettyFormat = true;
            generator.generateConsoleUseSyntax = true;
            StringBuilder builder = new StringBuilder();
            DataStructure ds = filterSet.RootItem as DataStructure;
            builder.AppendFormat("-- SQL statements for data structure: {0}\r\n", ds.Name);
            // parameter declarations
            builder.AppendLine(
                generator.SelectParameterDeclarationsSql(
                    filterSet, false, null));

            foreach (DataStructureEntity entity in ds.Entities)
            {
                if (entity.Columns.Count > 0)
                {
                    builder.AppendLine(generator.CreateOutputTableSql());
                    builder.AppendLine("-----------------------------------------------------------------");
                    builder.AppendLine("-- " + entity.Name);
                    builder.AppendLine("-----------------------------------------------------------------");
                    builder.AppendLine(
                        generator.SelectSql(ds,
                            entity,
                            filterSet,
                            null,
                            null,
                            new Hashtable(),
                            null,
                            false
                        )
                    );
                    builder.AppendLine(generator.CreateDataStructureFooterSql());
                }
            }
            new ShowSqlConsole(builder.ToString()).Run();
        }
    }
}