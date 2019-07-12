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
using System.Collections;
using System.Windows.Forms;

using Origam.Schema.EntityModel;
using Origam.UI;
using Origam.UI.WizardForm;
using Origam.Workbench;

namespace Origam.Schema.LookupModel.Wizards
{
    /// <summary>
    /// Summary description for CreateLookupFromEntityCommand.
    /// </summary>
    public class CreateLookupFromEntityCommand : AbstractMenuCommand
	{
        SchemaBrowser _schemaBrowser = WorkbenchSingleton.Workbench.GetPad(typeof(SchemaBrowser)) as SchemaBrowser;
        public override bool IsEnabled
		{
			get
			{
				return Owner is IDataEntity;
			}
			set
			{
				throw new ArgumentException(ResourceUtils.GetString("ErrorSetProperty"), "IsEnabled");
			}
		}

		public override void Run()
		{
            DataServiceDataLookup dd = new DataServiceDataLookup();
            ArrayList list = new ArrayList();
            list.Add(new ListViewItem(dd.ItemType, _schemaBrowser.ImageIndex(dd.Icon)));

            Stack stackPage = new Stack();
            stackPage.Push(PagesList.LookupForm);
            stackPage.Push(PagesList.startPage);

            LookupForm lookupForm = new LookupForm
            {
                Description = "Create Lookup Wizard",
                Pages = stackPage,
                Entity = Owner as IDataEntity,
                imgList = _schemaBrowser.EbrSchemaBrowser.imgList,
                listItemType = list
            };

            Wizard wiz = new Wizard(lookupForm);
             CreateLookupFromEntityWizard wizz = new CreateLookupFromEntityWizard();
            //wiz.Entity = Owner as IDataEntity;
            if (wiz.ShowDialog() == DialogResult.OK)
			{
                var result = LookupHelper.CreateDataServiceLookup(
                    lookupForm.LookupName, lookupForm.Entity, lookupForm.IdColumn, lookupForm.NameColumn,
                    null, lookupForm.IdFilter, lookupForm.ListFilter, null);
                //var result = LookupHelper.CreateDataServiceLookup(
                //    wiz.LookupName, wiz.Entity, wiz.IdColumn, wiz.NameColumn,
                //    null, wiz.IdFilter, wiz.ListFilter, null);
                GeneratedModelElements.Add(result.ListDataStructure);
                GeneratedModelElements.Add(result);
            }
        }
	}
}
