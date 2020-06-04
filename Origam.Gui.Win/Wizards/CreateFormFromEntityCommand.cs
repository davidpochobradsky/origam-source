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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Origam.Schema;
using Origam.Schema.DeploymentModel;
using Origam.Schema.EntityModel;
using Origam.Schema.GuiModel;
using Origam.Schema.MenuModel;
using Origam.Services;
using Origam.UI;
using Origam.UI.WizardForm;
using Origam.Workbench;
using Origam.Workbench.Services;

namespace Origam.Gui.Win.Wizards
{
    /// <summary>
    /// Summary description for CreateFormFromEntityCommand.
    /// </summary>
    public class CreateFormFromEntityCommand : AbstractMenuCommand
	{
        SchemaBrowser _schemaBrowser = WorkbenchSingleton.Workbench.GetPad(typeof(SchemaBrowser)) as SchemaBrowser;
        ScreenWizardForm wizardForm;
        public override bool IsEnabled
		{
			get
			{
				return Owner is IDataEntity;
			}
			set
			{
				throw new ArgumentException("Cannot set this property", "IsEnabled");
			}
		}

		public override void Run()
		{
            List<string> listdsName = GetListDatastructure(DataStructure.ItemTypeConst);
            ArrayList list = new ArrayList();
            DataStructure dd = new DataStructure();
            PanelControlSet pp = new PanelControlSet();
            FormControlSet ff = new FormControlSet();
            list.Add(new ListViewItem(dd.ItemType, dd.Icon));
            list.Add(new ListViewItem(pp.ItemType, pp.Icon));
            list.Add(new ListViewItem(ff.ItemType, ff.Icon));
            
            Stack stackPage = new Stack();
            stackPage.Push(PagesList.Finish);
            stackPage.Push(PagesList.ScreenForm);
            if (listdsName.Any(name => name == (Owner as IDataEntity).Name))
            {
                stackPage.Push(PagesList.StructureNamePage);
            }
            stackPage.Push(PagesList.StartPage);

            wizardForm = new ScreenWizardForm
            {
                ItemTypeList = list,
                Title = ResourceUtils.GetString("ScreenWizardTitle"),
                PageTitle = "",
                Description = ResourceUtils.GetString("ScreenWizardDescription"),
                Pages = stackPage,
                Entity = Owner as IDataEntity,
                IsRoleVisible = false,
                textColumnsOnly = false,
                StructureList = listdsName,
                NameOfEntity = (Owner as IDataEntity).Name,
                ImageList = _schemaBrowser.EbrSchemaBrowser.imgList,
                Command = this
            };

            Wizard wiz = new Wizard(wizardForm);
            if (wiz.ShowDialog() != DialogResult.OK)
            {
                GeneratedModelElements.Clear();
            }
        }

        public override void Execute()
        {
            string groupName = null;
            if (wizardForm.Entity.Group != null) groupName = wizardForm.Entity.Group.Name;

            DataStructure dataStructure = EntityHelper.CreateDataStructure(wizardForm.Entity, wizardForm.NameOfEntity, true);
            GeneratedModelElements.Add(dataStructure);
            PanelControlSet panel = GuiHelper.CreatePanel(groupName, wizardForm.Entity, wizardForm.SelectedFieldNames, wizardForm.NameOfEntity);
            GeneratedModelElements.Add(panel);
            FormControlSet form = GuiHelper.CreateForm(dataStructure, groupName, panel);
            GeneratedModelElements.Add(form);
        }
    }

	public class CreateCompleteUICommand : AbstractMenuCommand
	{
        SchemaBrowser _schemaBrowser = WorkbenchSingleton.Workbench.GetPad(typeof(SchemaBrowser)) as SchemaBrowser;
        ScreenWizardForm wizardForm;
        public override bool IsEnabled
		{
			get
			{
				return Owner is IDataEntity;
			}
			set
			{
				throw new ArgumentException("Cannot set this property", "IsEnabled");
			}
		}

		public override void Run()
		{
            IDataEntity entity = Owner as IDataEntity;
            List<string> listdsName = GetListDatastructure(DataStructure.ItemTypeConst);

            ArrayList list = new ArrayList();
            DataStructure ds = new DataStructure();
            PanelControlSet panel1 = new PanelControlSet();
            FormControlSet frmSet = new FormControlSet();
            FormReferenceMenuItem form1 = new FormReferenceMenuItem();
            ServiceCommandUpdateScriptActivity activity1 = new ServiceCommandUpdateScriptActivity();

            list.Add(new ListViewItem(ds.ItemType, ds.Icon));
            list.Add(new ListViewItem(panel1.ItemType, panel1.Icon));
            list.Add(new ListViewItem(frmSet.ItemType, frmSet.Icon));
            list.Add(new ListViewItem(form1.ItemType, form1.Icon));
            list.Add(new ListViewItem(activity1.ItemType, activity1.Icon));

            Stack stackPage = new Stack();
            stackPage.Push(PagesList.Finish);
            stackPage.Push(PagesList.ScreenForm);
            if (listdsName.Any(name => name == (Owner as IDataEntity).Name))
            {
                stackPage.Push(PagesList.StructureNamePage);
            }
            stackPage.Push(PagesList.StartPage);

            wizardForm = new ScreenWizardForm
            {
                ItemTypeList = list,
                Title = ResourceUtils.GetString("CreateCompleteUIWizardTitle"),
                PageTitle = "",
                Description = ResourceUtils.GetString("CreateCompleteUIWizardDescription"),
                Pages = stackPage,
                Entity = Owner as IDataEntity,
                IsRoleVisible = true,
                textColumnsOnly = false,
                StructureList = listdsName,
                NameOfEntity = (Owner as IDataEntity).Name,
                ImageList = _schemaBrowser.EbrSchemaBrowser.imgList,
                Command = this,
                Role = entity.Name
            };

            Wizard wiz = new Wizard(wizardForm);
            if (wiz.ShowDialog() != DialogResult.OK)
            {
                GeneratedModelElements.Clear();
            }
		}

        public override void Execute()
        {
            string groupName = null;
            if (wizardForm.Entity.Group != null) groupName = wizardForm.Entity.Group.Name;

            DataStructure dataStructure = EntityHelper.CreateDataStructure(wizardForm.Entity, wizardForm.NameOfEntity, true);
            PanelControlSet panel = GuiHelper.CreatePanel(groupName, wizardForm.Entity, wizardForm.SelectedFieldNames,wizardForm.NameOfEntity);
            FormControlSet form = GuiHelper.CreateForm(dataStructure, groupName, panel);
            FormReferenceMenuItem menu = MenuHelper.CreateMenuItem(wizardForm.Entity.Caption == null || wizardForm.Entity.Caption == ""
                ? wizardForm.NameOfEntity : wizardForm.Entity.Caption, wizardForm.Role, form);
            GeneratedModelElements.Add(dataStructure);
            GeneratedModelElements.Add(panel);
            GeneratedModelElements.Add(form);
            GeneratedModelElements.Add(menu);
            if (wizardForm.Role != "*" && wizardForm.Role != "")
            {
                ServiceCommandUpdateScriptActivity activity = CreateRole(wizardForm.Role);
                GeneratedModelElements.Add(activity);
            }
        }
    }

	public class CreateFormFromPanelCommand : AbstractMenuCommand
	{
        PanelWizardForm panelWizard;
        SchemaBrowser _schemaBrowser = WorkbenchSingleton.Workbench.GetPad(typeof(SchemaBrowser)) as SchemaBrowser;
        public override bool IsEnabled
		{
			get
			{
				return Owner is PanelControlSet;
			}
			set
			{
				throw new ArgumentException("Cannot set this property", "IsEnabled");
			}
		}

		public override void Run()
		{
            PanelControlSet panel = Owner as PanelControlSet;
           
            List<string> listdsName = GetListDatastructure(PanelControlSet.ItemTypeConst); 
            ArrayList list = new ArrayList();
            list.Add(new ListViewItem(panel.ItemType, panel.Icon));

            Stack stackPage = new Stack();
            stackPage.Push(PagesList.Finish);
            if (listdsName.Any(name => name == panel.Name))
            {
                stackPage.Push(PagesList.StructureNamePage);
            }
            stackPage.Push(PagesList.StartPage);
            panelWizard = new PanelWizardForm
            {
                ItemTypeList = list,
                Title = ResourceUtils.GetString("CreateFormFromPanelWizardTitle"),
                PageTitle = "",
                Description = ResourceUtils.GetString("CreateFormFromPanelWizardDescription."),
                StructureList= listdsName,
                NameOfEntity = panel.Name,
                Pages = stackPage,
                Entity = panel,
                ImageList = _schemaBrowser.EbrSchemaBrowser.imgList,
                Command = this
            };
            Wizard wiz = new Wizard(panelWizard);
            if (wiz.ShowDialog() != DialogResult.OK)
            {
                GeneratedModelElements.Clear();
            }

        }

        public override void Execute()
        {
            PanelControlSet panel = ((PanelControlSet)panelWizard.Entity);
            string groupName = null;
            if (panelWizard.Entity.Group != null) groupName = panelWizard.Entity.Group.Name;
            DataStructure dataStructure = EntityHelper.CreateDataStructure(panel.DataEntity, panelWizard.NameOfEntity, true);
            GeneratedModelElements.Add(dataStructure);
            FormControlSet form = GuiHelper.CreateForm(dataStructure, groupName, panel);
            GeneratedModelElements.Add(form);
            Origam.Workbench.Commands.EditSchemaItem edit = new Origam.Workbench.Commands.EditSchemaItem();
            edit.Owner = form;
            edit.Run();
        }
    }

        public class CreateMenuFromFormCommand : AbstractMenuCommand
	{
        MenuFromForm menuFrom;
        SchemaBrowser _schemaBrowser = WorkbenchSingleton.Workbench.GetPad(typeof(SchemaBrowser)) as SchemaBrowser;
        public override bool IsEnabled
		{
			get
			{
				return Owner is FormControlSet;
			}
			set
			{
				throw new ArgumentException("Cannot set this property", "IsEnabled");
			}
		}

		public override void Run()
		{
            FormControlSet form = Owner as FormControlSet;

            ArrayList list = new ArrayList();
            list.Add(new ListViewItem(form.ItemType, form.Icon));

            Stack stackPage = new Stack();
            stackPage.Push(PagesList.Finish);
            stackPage.Push(PagesList.MenuPage);
            stackPage.Push(PagesList.StartPage);

            menuFrom = new MenuFromForm
            {
                ItemTypeList = list,
                Title = ResourceUtils.GetString("CreateMenuFromFormWizardTitle"),
                PageTitle = "",
                Description = ResourceUtils.GetString("CreateMenuFromFormWizardDescription"),
                Pages = stackPage,
                Entity = form,
                Role = form.Name,
                ImageList = _schemaBrowser.EbrSchemaBrowser.imgList,
                Command = this
            };
            Wizard wiz = new Wizard(menuFrom);
            if (wiz.ShowDialog() != DialogResult.OK)
            {
                GeneratedModelElements.Clear();
            }
		}

        public override void Execute()
        {
            FormReferenceMenuItem menu = MenuHelper.CreateMenuItem(menuFrom.Entity.Name == null || menuFrom.Entity.Name == ""
                    ? menuFrom.NameOfEntity : menuFrom.Entity.Name, menuFrom.Role, (FormControlSet)menuFrom.Entity);
            GeneratedModelElements.Add(menu);
            bool createRole = menuFrom.Role != "*" && menuFrom.Role != "";
            if (createRole)
            {
                ServiceCommandUpdateScriptActivity activity = CreateRole(menuFrom.Role);
                GeneratedModelElements.Add(activity);
            }
        }
}

	public class CreateMenuFromDataConstantCommand : AbstractMenuCommand
	{
        MenuFromForm menuFrom;
        SchemaBrowser _schemaBrowser = WorkbenchSingleton.Workbench.GetPad(typeof(SchemaBrowser)) as SchemaBrowser;
        public override bool IsEnabled
		{
			get
			{
				return Owner is DataConstant;
			}
			set
			{
				throw new ArgumentException("Cannot set this property", "IsEnabled");
			}
		}

		public override void Run()
		{
			DataConstant constant = Owner as DataConstant;

            ArrayList list = new ArrayList();
            DataConstantReferenceMenuItem dataconstant = new DataConstantReferenceMenuItem();
            list.Add(new ListViewItem(dataconstant.ItemType, dataconstant.Icon));

            Stack stackPage = new Stack();
            stackPage.Push(PagesList.Finish);
            stackPage.Push(PagesList.MenuPage);
            stackPage.Push(PagesList.StartPage);

            menuFrom = new MenuFromForm
            {
                ItemTypeList = list,
                Title = ResourceUtils.GetString("CreateMenuFromDataConstantWizardTitle"),
                PageTitle = "",
                Description = ResourceUtils.GetString("CreateMenuFromDataConstantWizardDescription"),
                Pages = stackPage,
                Entity = constant,
                Role = constant.Name,
                ImageList = _schemaBrowser.EbrSchemaBrowser.imgList,
                Command = this
            };
            Wizard wiz = new Wizard(menuFrom);
			if(wiz.ShowDialog() != DialogResult.OK)
			{
                GeneratedModelElements.Clear();
            }
		}

        public override void Execute()
        {
            DataConstantReferenceMenuItem menu = MenuHelper.CreateMenuItem(menuFrom.Caption, menuFrom.Role, menuFrom.Entity as DataConstant);
            GeneratedModelElements.Add(menu);
            bool createRole = menuFrom.Role != "*" && menuFrom.Role != "";
            if (createRole)
            {
                ServiceCommandUpdateScriptActivity activity = CreateRole(menuFrom.Role);
                GeneratedModelElements.Add(activity);
            }
        }
    }

	public class CreateMenuFromSequentialWorkflowCommand : AbstractMenuCommand
	{
        MenuFromForm menuFrom;
        SchemaBrowser _schemaBrowser = WorkbenchSingleton.Workbench.GetPad(typeof(SchemaBrowser)) as SchemaBrowser;
        public override bool IsEnabled
		{
			get
			{
				return Owner is Schema.WorkflowModel.Workflow;
			}
			set
			{
				throw new ArgumentException("Cannot set this property", "IsEnabled");
			}
		}

		public override void Run()
		{
			Schema.WorkflowModel.Workflow wf = Owner as Schema.WorkflowModel.Workflow;

            ArrayList list = new ArrayList();
            WorkflowReferenceMenuItem workflowReference = new WorkflowReferenceMenuItem();
            list.Add(new ListViewItem(workflowReference.ItemType, workflowReference.Icon));

            Stack stackPage = new Stack();
            stackPage.Push(PagesList.Finish);
            stackPage.Push(PagesList.MenuPage);
            stackPage.Push(PagesList.StartPage);

            menuFrom = new MenuFromForm
            {
                ItemTypeList = list,
                Title = ResourceUtils.GetString("CreateMenuFromSequentialWorkflowWizardTitle"),
                PageTitle = "",
                Description = ResourceUtils.GetString("CreateMenuFromSequentialWorkflowWizardTitle"),
                Pages = stackPage,
                Entity = wf,
                Role = wf.Name,
                ImageList = _schemaBrowser.EbrSchemaBrowser.imgList,
                Command = this
            };
            Wizard wiz = new Wizard(menuFrom);
            if (wiz.ShowDialog() != DialogResult.OK)
			{
                GeneratedModelElements.Clear();
            }
		}
        public override void Execute()
        {
            WorkflowReferenceMenuItem menu = MenuHelper
                .CreateMenuItem(menuFrom.Caption, menuFrom.Role, menuFrom.Entity as Schema.WorkflowModel.Workflow);
            GeneratedModelElements.Add(menu);
            bool createRole = menuFrom.Role != "*" && menuFrom.Role != "";
            if (createRole)
            {
                ServiceCommandUpdateScriptActivity activity = CreateRole(menuFrom.Role);
                GeneratedModelElements.Add(activity);
            }
        }
    }
}
