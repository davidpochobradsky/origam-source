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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using LibGit2Sharp;
using Origam.DA.ObjectPersistence;
using Origam.DA.Service;
using Origam.Extensions;
using Origam.Git;
using Origam.Schema;
using Origam.Schema.GuiModel;
using Origam.UI;
using Origam.Windows.Editor.GIT;
using Origam.Workbench.Editors;
using Origam.Workbench.Services;
using WeifenLuo.WinFormsUI.Docking;

namespace Origam.Workbench.Commands
{
	/// <summary>
	/// Creates a new schema item and displays it in editor.
	/// </summary>
	public class AddNewSchemaItem : AbstractMenuCommand
	{
        public AddNewSchemaItem(bool showDialog)
        {
            ShowDialog = showDialog;
        }

        public bool ShowDialog { get; set; }
        string _name = null;
        ISchemaItemFactory _parentElement = null;
        WorkbenchSchemaService _schema = ServiceManager.Services.GetService(
            typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;

		public override bool IsEnabled
		{
			get
			{
				return _parentElement != null;
			}
			set
			{
				throw new ArgumentException(ResourceUtils.GetString("ErrorSetProperty"), "IsEnabled");
			}
		}

        public ISchemaItemFactory ParentElement
        {
            get
            {
                return _parentElement;
            }
            set
            {
                _parentElement = value;
            }
        }

        public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

        public event EventHandler<AbstractSchemaItem> ItemCreated ;
        public override void Run()
		{
			AbstractSchemaItem item = ParentElement.NewItem(this.Owner as Type, _schema.ActiveSchemaExtensionId, null);
			if(_name != null)
			{
				item.Name = _name;
			}

			// set abstract, if parent is abstract
			if(item.ParentItem != null && item.ParentItem.IsAbstract)
			{
				item.IsAbstract = true;
			}

			//_schema.SchemaBrowser.ebrSchemaBrowser.RefreshActiveNode();

			EditSchemaItem cmd = new EditSchemaItem(ShowDialog);
			cmd.Owner = item;

			_schema.LastAddedNodeParent = ParentElement;
			_schema.LastAddedType = this.Owner as Type;
			ItemCreated?.Invoke(this, item);
			cmd.Run();
		}
	
		public override void Dispose()
		{
			_schema= null;
		}

	}

	/// <summary>
	/// Creates a new schema item of same type and under the same parent as last added schema item and displays it in editor.
	/// </summary>
	public class AddRepeatingSchemaItem : AbstractMenuCommand
	{
		SchemaService _schema = ServiceManager.Services.GetService(typeof(SchemaService)) as SchemaService;

		public override bool IsEnabled
		{
			get
			{
				return _schema.LastAddedNodeParent != null;
			}
			set
			{
				throw new ArgumentException(ResourceUtils.GetString("ErrorSetProperty"), "IsEnabled");
			}
		}

		public override void Run()
		{
			AbstractSchemaItem item = _schema.LastAddedNodeParent.NewItem(
                _schema.LastAddedType, _schema.ActiveSchemaExtensionId, null);
			// set abstract, if parent is abstract
			if(item.ParentItem != null && item.ParentItem.IsAbstract)
			{
				item.IsAbstract = true;
			}
			EditSchemaItem cmd = new EditSchemaItem();
			cmd.Owner = item;
			cmd.Run();
		}

		public override void Dispose()
		{
			_schema = null;
		}

	}

	/// <summary>
	/// Converts existing schema item to a new type and displays it in editor. Before it delets the existing item.
	/// </summary>
	public class ConvertSchemaItem : AbstractMenuCommand
	{
		WorkbenchSchemaService _schema = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;

		public override bool IsEnabled
		{
			get
			{
				return _schema.ActiveNode is ISchemaItemConvertible;
			}
			set
			{
				throw new ArgumentException(ResourceUtils.GetString("ErrorSetProperty"), "IsEnabled");
			}
		}

		public override void Run()
		{
			ISchemaItemConvertible activeItem = _schema.ActiveNode as ISchemaItemConvertible;

			ISchemaItem converted = activeItem.ConvertTo(this.Owner as Type);

			//_schema.UpdateBrowser();
			EditSchemaItem cmd = new EditSchemaItem();
			cmd.Owner = converted;

			cmd.Run();
		}

		public override void Dispose()
		{
			_schema = null;

			base.Dispose ();
		}

	}

	/// <summary>
	/// Moves an existing schema item to a different schema extension.
	/// </summary>
	public class MoveToExtension : AbstractMenuCommand
	{
		WorkbenchSchemaService _schema = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;

		public override bool IsEnabled
		{
			get
			{
				AbstractSchemaItem schemaItem = (AbstractSchemaItem) _schema.ActiveNode;
				if (schemaItem.Package == Owner) return false;
				if (schemaItem.ParentItem == null) return true;
				return !schemaItem.Package.IncludedPackages.Contains(Owner) ||
				       schemaItem.ParentItem.Package == Owner;
			}
			set => throw new ArgumentException(ResourceUtils.GetString("ErrorSetProperty"), "IsEnabled");
		}

		public override void Run()
		{
			IPersistenceProvider persistenceProvider = ServiceManager.Services
				.GetService<IPersistenceService>().SchemaProvider;
			if(!(Owner is Package targetPackage))
			{
				throw new ArgumentOutOfRangeException("Owner", this.Owner, ResourceUtils.GetString("ErrorNotSchemaExtension"));
			}


			if(_schema.ActiveNode is SchemaItemGroup)
			{
				SchemaItemGroup activeItem = _schema.ActiveNode as SchemaItemGroup;

				activeItem.Package = targetPackage;
			
				activeItem.Persist();
			}
			else if(_schema.ActiveNode is AbstractSchemaItem activeItem)
			{
				CheckCanBeMovedOrThrow(activeItem, targetPackage);

				activeItem.SetExtensionRecursive(targetPackage);

				persistenceProvider.BeginTransaction();
				activeItem.Persist();
				persistenceProvider.EndTransaction();
			}
		}

		private static void CheckCanBeMovedOrThrow(AbstractSchemaItem activeItem,
			Package targetPackage)
		{
			List<ISchemaItem> dependenciesInPackagesNotReferencedByTargetPackage
				= activeItem.GetDependencies(true)
					.Cast<object>()
					.OfType<ISchemaItem>()
					.Where(item =>
						!targetPackage.IncludedPackages.Contains(item.Package)
						&& item.Package != targetPackage)
					.ToList();
			if (dependenciesInPackagesNotReferencedByTargetPackage.Count != 0)
			{
				throw new Exception(string.Format(
					Strings.ErrorDependenciesInPackagesNotReferencedByTargetPackage,
					targetPackage.Name,
					FormatToIdList(dependenciesInPackagesNotReferencedByTargetPackage)));
			}

			List<ISchemaItem> usagesInPackagesWhichDontDependOnTargetPackage
				= activeItem.GetUsage()
					.Cast<object>()
					.OfType<ISchemaItem>()
					.Where(item =>
						!item.Package.IncludedPackages.Contains(targetPackage)
						&& item.Package != targetPackage)
					.ToList();

			if (usagesInPackagesWhichDontDependOnTargetPackage.Count != 0)
			{
				throw new Exception(String.Format(
					Strings.ErrorUsagesInPackagesWhichDontDependOnTargetPackage,
					targetPackage.Name, 
					FormatToIdList(usagesInPackagesWhichDontDependOnTargetPackage)));
			}
		}

		private static string FormatToIdList(List<ISchemaItem> schemaItems)
		{
			return "["+ string.Join("\n", schemaItems.Select(x => x.Id)) +"]";
		}

		public override void Dispose()
		{
			_schema = null;

			base.Dispose ();
		}

	}

	/// <summary>
	/// Creates a new group and displays it in editor.
	/// </summary>
	public class AddNewGroup : AbstractMenuCommand
	{
		WorkbenchSchemaService _schema = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;

		public override bool IsEnabled
		{
			get
			{
				return _schema.ActiveNode is AbstractSchemaItemProvider || _schema.ActiveNode is SchemaItemGroup;
			}
			set
			{
				throw new ArgumentException(ResourceUtils.GetString("ErrorSetProperty"), "IsEnabled");
			}
		}

		public override void Run()
		{
			SchemaItemGroup item = (_schema.ActiveNode as ISchemaItemFactory).NewGroup(_schema.ActiveSchemaExtensionId);
			_schema.SchemaBrowser.EbrSchemaBrowser.RefreshActiveNode();
		}

		public override void Dispose()
		{
			_schema = null;

			base.Dispose ();
		}

	}

	/// <summary>
	/// Edits an active schema item in a diagram editor.
	/// </summary>
	public class EditDiagramActiveSchemaItem : AbstractCommand
	{
		WorkbenchSchemaService _schema = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;
		IPersistenceService _persistence = ServiceManager.Services.GetService(typeof(IPersistenceService)) as IPersistenceService;

		public override void Run()
		{
            AbstractSchemaItem item = this.Owner as AbstractSchemaItem;

            // First we test, if the item is not opened already
            foreach (IViewContent content in WorkbenchSingleton.Workbench.ViewContentCollection)
			{
				if(content.DisplayedItemId == item.Id &&
				   content.GetType().ToString() == "Origam.Workbench.Editors.DiagramEditor")
				{
					(content as DockContent).Activate();

					return;
				}
			}

			System.Reflection.Assembly a = Assembly.LoadWithPartialName("Origam.Workbench.Diagram");
			IViewContent editor = a.CreateInstance("Origam.Workbench.Editors.DiagramEditor") as IViewContent;

			// Set editor to dirty, if object has not been persisted, yet (new item)
			if(!item.IsPersisted)
				editor.IsDirty = true;
			else
			{
				// Get a copy of the item to edit (no cache usage => we get a fresh copy)
				AbstractSchemaItem freshItem = _persistence.SchemaProvider.RetrieveInstance(item.GetType(), item.PrimaryKey, false) as AbstractSchemaItem;
				freshItem.ParentItem = item.ParentItem;
				item = freshItem;
			}

			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
			editor.LoadObject(item);
			editor.TitleName = item.Name;
			editor.DisplayedItemId = item.Id;

			WorkbenchSingleton.Workbench.ShowView(editor);
			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

		}
	}

	/// <summary>
	/// Edits an active schema item in an editor.
	/// </summary>
	public class EditActiveSchemaItem : AbstractMenuCommand
	{
		WorkbenchSchemaService _schema = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;

		public override bool IsEnabled
		{
			get
			{
				if(_schema.IsSchemaLoaded)
				{
					return _schema.CanEditItem(_schema.ActiveNode);
				}
				else
				{
					return false;
				}
			}
			set
			{
				throw new ArgumentException(ResourceUtils.GetString("ErrorSetProperty"), "IsEnabled");
			}
		}

		public override void Run()
		{
			EditSchemaItem cmd = new EditSchemaItem();
			cmd.Owner = _schema.ActiveNode;
			cmd.Run();
		}

		public override void Dispose()
		{
			_schema = null;

			base.Dispose ();
		}

	}


    public class ExpandAllActiveSchemaItem : AbstractMenuCommand
    {
        WorkbenchSchemaService _schema = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;
        public override bool IsEnabled
        {
            get
            {
                return _schema.ActiveNode is AbstractSchemaItem;
            }
            set
            {
                throw new ArgumentException(ResourceUtils.GetString("ErrorSetProperty"), "IsEnabled");
            }
        }

        public override void Run()
        {
            if(_schema.ActiveNode.HasChildNodes)
            {
                ExpressionBrowser schemaBrowser = _schema.SchemaBrowser.EbrSchemaBrowser;
                schemaBrowser.ExpandAllChildNodes(_schema.ActiveNode);
            }
        }

        public override void Dispose()
        {
            _schema = null;

            base.Dispose();
        }

    }

    /// <summary>
    /// Edits a schema item in an editor. Schema item is passed as Owner.
    /// </summary>
    public class EditSchemaItem : AbstractCommand
    {
        public EditSchemaItem()
        {
            
        }

        public EditSchemaItem(bool showDialog)
        {
            ShowDialog = showDialog;
        }

        public bool ShowDialog { get; set; }
        public bool ShowDiagramEditorAfterSave { get; set; }
        IPersistenceService _persistence = ServiceManager.Services.GetService(typeof(IPersistenceService)) as IPersistenceService;
		WorkbenchSchemaService _schemaService = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;
//		private IParameterService _parameterService = ServiceManager.Services.GetService(typeof(IParameterService)) as IParameterService;

		public override void Run()
		{	
			// First we test, if the item is not opened already
			foreach(IViewContent content in WorkbenchSingleton.Workbench.ViewContentCollection)
			{
				if(content.DisplayedItemId == (Owner as IPersistent).Id 
				   && content is AbstractEditor)
				{
					(content as DockContent).Activate();

					return;
				}
			}
			IViewContent editor;
			IPersistent item;
			if(Owner is AbstractSchemaItem || Owner is Package)
			{
				item = this.Owner as IPersistent;
			}
			else
			{
				throw new ArgumentOutOfRangeException("Owner", this.Owner, ResourceUtils.GetString("ErrorEditObject"));
			}
            string itemType = item.GetType().ToString();
            if (item is Package)
			{
				editor = new Origam.Workbench.Editors.PackageEditor();
			}
			else if(itemType == "Origam.Schema.GuiModel.FormControlSet" 
				|| itemType == "Origam.Schema.GuiModel.PanelControlSet"
				|| itemType == "Origam.Schema.GuiModel.ControlSetItem")
			{
				System.Reflection.Assembly a = Assembly.LoadWithPartialName("Origam.Gui.Designer");
				editor = a.CreateInstance("Origam.Gui.Designer.ControlSetEditor") as IViewContent;
				if(editor == null)
					throw new Exception(ResourceUtils.GetString("ErrorLoadEditorFailed"));
			}
			else if(itemType == "Origam.Schema.EntityModel.XslTransformation"
                || itemType == "Origam.Schema.RuleModel.XslRule"
                || itemType == "Origam.Schema.RuleModel.EndRule"
                || itemType == "Origam.Schema.RuleModel.ComplexDataRule")
			{
				System.Reflection.Assembly a = Assembly.LoadWithPartialName("Origam.Workbench");
				editor = a.CreateInstance("Origam.Workbench.Editors.XslEditor") as IViewContent;
				if(editor == null)
					throw new Exception(ResourceUtils.GetString("ErrorLoadEditorFailed"));
			}
			else if(itemType == "Origam.Schema.EntityModel.XsdDataStructure")
			{
				System.Reflection.Assembly a = Assembly.LoadWithPartialName("Origam.Schema.EntityModel.UI");
				editor = a.CreateInstance("Origam.Schema.EntityModel.UI.XsdEditor") as IViewContent;
				if(editor == null)
					throw new Exception(ResourceUtils.GetString("ErrorLoadEditorFailed"));
			}
			else if(itemType == "Origam.Schema.DeploymentModel.ServiceCommandUpdateScriptActivity")
			{
				System.Reflection.Assembly a = Assembly.LoadWithPartialName("Origam.Schema.DeploymentModel.UI");
				editor = a.CreateInstance("Origam.Schema.DeploymentModel.ServiceScriptCommandEditor") as IViewContent;
				if(editor == null)
					throw new Exception(ResourceUtils.GetString("ErrorLoadEditorFailed"));
			}
            else if (item is EntityUIAction)
            {
               editor = new UiActionEditor();
            }
            else if (itemType == "Origam.Schema.WorkflowModel.Workflow" && ! ShowDialog)
            {
                if (item.IsPersisted)
                {
                    var diagramAction = new EditDiagramActiveSchemaItem();
                    diagramAction.Owner = this.Owner;
                    diagramAction.Run();
                    return;
                }
                else
                {
                    editor = new PropertyGridEditor();
                    this.ShowDialog = true;
                    this.ShowDiagramEditorAfterSave = true;
                }
            }
            else
			{
                editor = new PropertyGridEditor();
            }

            // Set editor to dirty, if object has not been persisted, yet (new item)
            if (!item.IsPersisted)
			{
				editor.IsDirty = true;
			}
			else
			{
				// Get a copy of the item to edit (no cache usage => we get a fresh copy)
				item = item.GetFreshItem();
			}

			System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
			if(! _schemaService.CanEditItem(item))
			{
				editor.IsReadOnly = true;
			}

			editor.LoadObject(item);
			editor.DisplayedItemId = item.Id;

			if(item is AbstractSchemaItem)
			{
				editor.TitleName = (item as AbstractSchemaItem).Name;
				if((item as AbstractSchemaItem).NodeImage == null)
				{
					(editor as Form).Icon = System.Drawing.Icon.FromHandle(
                        ((System.Drawing.Bitmap)_schemaService.SchemaBrowser.ImageList.Images[
                            _schemaService.SchemaBrowser.ImageIndex((item as AbstractSchemaItem).Icon)]).GetHicon());
				}
				else
				{
					(editor as Form).Icon = System.Drawing.Icon.FromHandle((item as AbstractSchemaItem)
						.NodeImage.ToBitmap()
						.GetHicon());
				}
            }
            else if(item is Package)
			{
				editor.TitleName = (item as Package).Name;
			}

            if (ShowDialog)
            {
                var result = (editor as Form).ShowDialog(WorkbenchSingleton.Workbench as IWin32Window);
                if (result == DialogResult.OK && ShowDiagramEditorAfterSave)
                {
                    this.ShowDialog = false;
                    ShowDiagramEditorAfterSave = false;
                    Run();
                }
            }
            else
            {
                WorkbenchSingleton.Workbench.ShowView(editor);
            }
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
		}
	}

	/// <summary>
	/// Delets the currently selected node.
	/// </summary>
	public class DeleteActiveNode : AbstractMenuCommand
	{
		WorkbenchSchemaService _schema = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;
		public event EventHandler BeforeDelete;
		public event EventHandler AfterDelete;
		public override bool IsEnabled
		{
			get
			{
				if(_schema.IsSchemaLoaded && _schema.ActiveNode != null)
				{
					if(! (_schema.CanDeleteItem(_schema.ActiveNode)))
					{
						return false;
					}

					return _schema.ActiveNode.CanDelete;
				}
				
				return false;
			}
			set
			{
				throw new ArgumentException(ResourceUtils.GetString("ErrorSetProperty"), "IsEnabled");
			}
		}

		public override void Run()
		{
			if((_schema.ActiveNode is AbstractSchemaItem && (_schema.ActiveNode as AbstractSchemaItem).SchemaExtensionId != _schema.ActiveSchemaExtensionId)
				| (_schema.ActiveNode is SchemaItemGroup && (_schema.ActiveNode as SchemaItemGroup).SchemaExtensionId != _schema.ActiveSchemaExtensionId))
			{
				throw new InvalidOperationException(ResourceUtils.GetString("ErrorDeleteItemNotActiveExtension"));
			}

			if(MessageBox.Show(ResourceUtils.GetString("DoYouWishDelete", _schema.ActiveNode.NodeText), ResourceUtils.GetString("DeleteTile"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
			{
				// first close an open editor
				foreach(IViewContent content in WorkbenchSingleton.Workbench.ViewContentCollection.ToArrayList())
				{
					if(content.DisplayedItemId == (_schema.ActiveNode as IPersistent).Id)
					{
						content.IsDirty = false;
						(content as DockContent).Close();
					}
				}
                IPersistenceProvider persistenceProvider = ServiceManager.Services
                    .GetService<IPersistenceService>().SchemaProvider;

                // then delete from the model
				BeforeDelete?.Invoke(this, EventArgs.Empty);
                try
                {
					persistenceProvider.BeginTransaction();
                    _schema.ActiveNode.Delete();
                    AfterDelete?.Invoke(this, EventArgs.Empty);
                }
                catch
                {
                    // it might fail because of references
                    persistenceProvider.EndTransactionDontSave();
                    throw;
                }
                persistenceProvider.EndTransaction();
            }
		}

		public override void Dispose()
		{
			_schema = null;
		}

	}

	/// <summary>
	/// Displays the documentation for the active schema item in the documentation pad
	/// </summary>
	public class ShowDocumentation : AbstractMenuCommand
	{
		WorkbenchSchemaService _schema = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;

		public override bool IsEnabled
		{
			get
			{
				return _schema.ActiveSchemaItem != null;
			}
			set
			{
				throw new ArgumentException(ResourceUtils.GetString("ErrorSetProperty"), "IsEnabled");
			}
		}

		public override void Run()
		{
			Pads.DocumentationPad pad = WorkbenchSingleton.Workbench.GetPad(typeof(Pads.DocumentationPad)) as Pads.DocumentationPad;
			pad.ShowDocumentation(_schema.ActiveSchemaItem.Id);
		}

		public override void Dispose()
		{
			_schema = null;

			base.Dispose ();
		}

	}

	/// <summary>
	/// Displays list of items on which currently selected item is dependent
	/// </summary>
	public class ShowDependencies : AbstractMenuCommand
	{
		WorkbenchSchemaService _schema = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;

		public override bool IsEnabled
		{
			get
			{
				return _schema.ActiveSchemaItem != null;
			}
			set
			{
				throw new ArgumentException(ResourceUtils.GetString("ErrorSetProperty"), "IsEnabled");
			}
		}

		public override void Run()
		{
			Pads.FindSchemaItemResultsPad pad = WorkbenchSingleton.Workbench.GetPad(typeof(Pads.FindSchemaItemResultsPad)) as Pads.FindSchemaItemResultsPad;

			var dependencies =_schema.ActiveSchemaItem
			    .GetDependencies(false)
			    .Cast<AbstractSchemaItem>()
			    .Where(x => x!=null)
			    .ToArray();

			pad.DisplayResults(dependencies);

			ViewFindSchemaItemResultsPad cmd = new ViewFindSchemaItemResultsPad();
			cmd.Run();
			cmd.Dispose();
		}

		public override void Dispose()
		{
			_schema = null;

			base.Dispose ();
		}

	}

	/// <summary>
	/// Displays list of items on which currently selected item is dependent
	/// </summary>
	public class ShowUsage : AbstractMenuCommand
	{
		WorkbenchSchemaService _schema = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;

		public override bool IsEnabled
		{
			get
			{
				return _schema.ActiveSchemaItem != null;
			}
			set
			{
				throw new ArgumentException(ResourceUtils.GetString("ErrorSetProperty"), "IsEnabled");
			}
		}

		public override void Run()
		{
			Pads.FindSchemaItemResultsPad pad = WorkbenchSingleton.Workbench.GetPad(typeof(Pads.FindSchemaItemResultsPad)) as Pads.FindSchemaItemResultsPad;
            var referenceList = _schema.ActiveSchemaItem.GetUsage();
            if (referenceList != null)
            {
                pad.DisplayResults((AbstractSchemaItem[])referenceList.ToArray(typeof(AbstractSchemaItem)));
            }

			ViewFindSchemaItemResultsPad cmd = new ViewFindSchemaItemResultsPad();
			cmd.Run();
			cmd.Dispose();
		}

		public override void Dispose()
		{
			_schema = null;

			base.Dispose ();
		}

	}

    /// <summary>
    /// Show file in directory in explorer.
    /// </summary>
    public class ShowExplorerXml : AbstractMenuCommand
    {
        WorkbenchSchemaService _schema = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;

        public override bool IsEnabled
        {
            get
            {
                return _schema.ActiveSchemaItem != null;
            }
            set
            {
                throw new ArgumentException(ResourceUtils.GetString("ErrorSetProperty"), "IsEnabled");
            }
        }

        public override void Run()
        {
            OrigamSettings settings = ConfigurationManager.GetActiveConfiguration();
            foreach (string file in _schema.ActiveSchemaItem.Files)
            {
                string filePath = Path.Combine(settings.ModelSourceControlLocation.Replace("/", "\\"),file);
                if (File.Exists(filePath))
                {
                    Process.Start("explorer.exe", "/select," + filePath);
                }
                break;
            }
        }

        public override void Dispose()
        {
            _schema = null;

            base.Dispose();
        }

    }

    /// <summary>
    /// Show file in directory in explorer.
    /// </summary>
    public class ShowConsoleXml : AbstractMenuCommand
    {
        WorkbenchSchemaService _schema = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;

        public override bool IsEnabled
        {
            get
            {
                return _schema.ActiveSchemaItem != null;
            }
            set
            {
                throw new ArgumentException(ResourceUtils.GetString("ErrorSetProperty"), "IsEnabled");
            }
        }

        public override void Run()
        {
            OrigamSettings settings = ConfigurationManager.GetActiveConfiguration();
            foreach (string file in _schema.ActiveSchemaItem.Files)
            {
                string filePath = Path.Combine(settings.ModelSourceControlLocation,file);
                if (File.Exists(filePath))
                {
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                    {
                        Indent = true,
                        NewLineOnAttributes = true
                    };
                    XmlDocument xml = new XmlDocument();
                    XmlViewer viewer = new XmlViewer
                    {
                        Text = file.Replace("\\", "/").Split('/').LastOrDefault()
                    };
                    try
                    {
                        xml.Load(filePath);
                        viewer.Content = xml.ToBeautifulString(xmlWriterSettings);
                    }
                    catch
                    {
                        viewer.Content = new StreamReader(filePath).ReadToEnd();
                    }
                    WorkbenchSingleton.Workbench.ShowView(viewer);
                }
            }
        }

        public override void Dispose()
        {
            _schema = null;

            base.Dispose();
        }

    }

    /// <summary>
    /// Show file in directory in explorer.
    /// </summary>
    public class ShowFileDiffXml : AbstractMenuCommand
    {
        WorkbenchSchemaService _schema = ServiceManager.Services.GetService(typeof(WorkbenchSchemaService)) as WorkbenchSchemaService;
       

        public override bool IsEnabled
        {
            get
            {
                return _schema.ActiveSchemaItem != null;
            }
            set
            {
                throw new ArgumentException(ResourceUtils.GetString("ErrorSetProperty"), "IsEnabled");
            }
        }

        public override void Run()
        {
            OrigamSettings settings = ConfigurationManager.GetActiveConfiguration();
            string activefile = Path.Combine(settings.ModelSourceControlLocation, _schema.ActiveSchemaItem.RootItem.RelativeFilePath);
            var provider = (FilePersistenceProvider)_schema.ActiveSchemaItem.PersistenceProvider;
            bool hasChange = false;
            if (provider != null)
            {
                GitManager gitManager = new GitManager(settings.ModelSourceControlLocation);
                foreach (string file in _schema.ActiveSchemaItem.Files)
                {
                    string fileName = Path.Combine(settings.ModelSourceControlLocation, file);
                    if (File.Exists(fileName))
                    {
                        gitManager.SetFile(fileName);
                        Commit lastCommit = gitManager.GetLastCommit();
                        string text = gitManager.GetModifiedChanges();
                        if (!string.IsNullOrEmpty(text))
                        {
                            GitDiferenceView gitDiferenceView = new GitDiferenceView
                            {
                                Text = gitManager.getCompareFileName()
                            };
                            text = Regex.Replace(text, @"^.*\ No newline at end of file.*\n", "", RegexOptions.Multiline);
                            gitDiferenceView.ShowDiff(fileName + " " + lastCommit.Sha, fileName, text);
                            WorkbenchSingleton.Workbench.ShowView(gitDiferenceView);
                            hasChange = true;
                        }
                    }
                }
            }
            if(!hasChange)
            {
                MessageBox.Show("Found no changes in " + activefile, "Git Diff",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public override void Dispose()
        {
            _schema = null;

            base.Dispose();
        }
    }
}
