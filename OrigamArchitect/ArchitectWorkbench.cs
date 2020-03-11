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
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSharpFunctionalExtensions;
using JR.Utils.GUI.Forms;
using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Origam;
using Origam.DA;
using Origam.DA.ObjectPersistence;
using Origam.DA.Service;
using Origam.DA.Service.MetaModelUpgrade;
using Origam.Excel;
using Origam.Extensions;
using Origam.Gui.UI;
using Origam.Gui.Win;
using Origam.Gui.Win.Commands;
using Origam.Licensing;
using Origam.OrigamEngine;
using Origam.Rule;
using Origam.Schema;
using Origam.Schema.DeploymentModel;
using Origam.Schema.EntityModel;
using Origam.Schema.MenuModel;
using Origam.UI;
using Origam.Workbench;
using Origam.Workbench.BaseComponents;
using Origam.Workbench.Commands;
using Origam.Workbench.Editors;
using Origam.Workbench.Pads;
using Origam.Workbench.Services;
using Origam.Workbench.Services.CoreServices;
using Origam.Workflow;
using Origam.Workflow.Gui.Win;
using OrigamArchitect.Commands;
using WeifenLuo.WinFormsUI.Docking;

namespace OrigamArchitect
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	internal class frmMain : Form, IWorkbench
	{
		private static readonly log4net.ILog log 
            = log4net.LogManager.GetLogger(
			MethodBase.GetCurrentMethod().DeclaringType);

        private CancellationTokenSource modelCheckCancellationTokenSource = new CancellationTokenSource();
        private readonly Dictionary<IToolStripContainer,List<ToolStrip>> loadedForms 
			= new Dictionary<IToolStripContainer,List<ToolStrip>>();
		
		AsMenu _fileMenu = null;
		AsMenu _schemaMenu = null;
		AsMenu _viewMenu = null;
		AsMenu _toolsMenu = null;
		AsMenu _helpMenu = null;
        AsMenu _windowMenu = null;

		WorkbenchSchemaService _schema;
		private StatusBarService _statusBarService;
		bool closeAll = false;

		// Toolboxes
		SchemaBrowser _schemaBrowserPad;
#if ORIGAM_CLIENT
		AttachmentPad _attachmentPad;
		AuditLogPad _auditLogPad;
#else
		WorkflowWatchPad _workflowWatchPad;
		AttachmentPad _attachmentPad;
		AuditLogPad _auditLogPad;
		DocumentationPad _documentationPad;
		FindSchemaItemResultsPad _findSchemaItemResultsPad;
        FindRulesPad _findRulesPad;
#endif
        PropertyPad _propertyPad;
		WorkflowPlayerPad _workflowPad;
		OutputPad _outputPad;
		LogPad _logPad;
        ServerLogPad _serverLogPad;
        ExtensionPad _extensionPad;

		Hashtable _shortcuts = new Hashtable();

		private static ICellStyle _dateCellStyle;
		private string _configFilePath;
	    private FormWindowState lastWindowState;

        public const string ORIGAM_COM_BASEURL = "https://origam.com/";
		public const string ORIGAM_COM_API_BASEURL = "https://origam.com/web/";
		public const bool IgnoreHTTPSErrors = false;
		

		private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.StatusBarPanel sbpText;
        private System.Windows.Forms.StatusBarPanel sbpMemory;
        private MenuStrip menuStrip;
        private LabeledToolStrip ducumentToolStrip;
		private LabeledToolStrip toolsToolStrip;
        private PictureBox logoPictureBox;
		private FlowLayoutPanel toolStripFlowLayoutPanel;
		private Panel toolStripPanel;
        private BackgroundWorker LicenseBackgroudExtender;
		private BackgroundWorker AutoUpdateBackgroudFinder;
        private TableLayoutPanel rightToolsTripLayoutPanel;
        private ComboBox searchComboBox;


		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			WorkflowHost.DefaultHost.SupportsUI = true;
            dockPanel.Theme = new OrigamTheme();
            dockPanel.ShowDocumentIcon = false;

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string origamPath = Path.Combine(appDataPath, "ORIGAM");
#if ORIGAM_CLIENT
			string configFileName = "ClientWorkspace.config";
#else
			string configFileName = "ArchitectWorkspace.config";
#endif

			if(! Directory.Exists(origamPath))
			{
				Directory.CreateDirectory(origamPath);
			}

			_configFilePath = Path.Combine(origamPath, configFileName);

            TypeDescriptor.AddAttributes(typeof(ISchemaItem),
                        new EditorAttribute(typeof(ModelUIEditor), typeof(System.Drawing.Design.UITypeEditor)));
            TypeDescriptor.AddAttributes(typeof(SchemaItemAncestorCollection),
                        new EditorAttribute(typeof(SchemaItemAncestorCollectionEditor),
                            typeof(System.Drawing.Design.UITypeEditor)));
		    TypeDescriptor.AddAttributes(typeof(Bitmap),
		        new EditorAttribute(typeof(System.Drawing.Design.BitmapEditor), typeof(System.Drawing.Design.UITypeEditor)));
		    TypeDescriptor.AddAttributes(typeof(Byte[]),
		        new EditorAttribute(typeof(Byte[]), typeof(FileSelectionUITypeEditor)));
    }

    public void OpenForm(object owner,Hashtable parameters)
		{
			ExecuteSchemaItem cmd = new ExecuteSchemaItem();
			cmd.Owner = owner;
			
			foreach (DictionaryEntry entry in parameters)
			{
				cmd.Parameters.Add(entry.Key,entry.Value);
			}
			
			cmd.Run();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				components?.Dispose();
			}
			base.Dispose( disposing );
		}

		private void OnChildFormToolStripsLoaded(object sender, EventArgs e)
		{
			UpdateToolStrips();
		}
		
		private void OnAllToolStripsRemovedFromALoadedForm(object sender, EventArgs e)
		{
			var toolStripContainer = (IToolStripContainer) sender;
			if (!loadedForms.ContainsKey(toolStripContainer)) return;
			RemoveToolStrips(toolStripContainer);
			loadedForms.Remove(toolStripContainer);
		}

		private void OnToolStripsNeedUpdate(object sender, EventArgs args)
		{
			UpdateToolStrips();
		}
		
		private void CleanUpToolStripsWhenClosing(IViewContent closingForm)
		{
			if (closingForm is IToolStripContainer toolStripContainer)
			{
				RemoveToolStrips(toolStripContainer);
				loadedForms.Remove(toolStripContainer);
			}
		}

		private void UpdateToolStrips()
		{
		    loadedForms.Keys.ForEach(RemoveToolStrips);
            if (ActiveDocument is IToolStripContainer toolStripContainer)
			{
				toolStripContainer.AllToolStripsRemoved -= OnAllToolStripsRemovedFromALoadedForm;
				toolStripContainer.AllToolStripsRemoved += OnAllToolStripsRemovedFromALoadedForm;
			    toolStripContainer.ToolStripsNeedUpdate -= OnToolStripsNeedUpdate;
			    toolStripContainer.ToolStripsNeedUpdate += OnToolStripsNeedUpdate;
			    int widthOfDisplayedToolStrips = toolStripPanel.Controls
			        .Cast<Control>()
			        .Select(x => x.Width)
			        .Sum();
			    int availableWidth = toolStripPanel.Width - widthOfDisplayedToolStrips;
                loadedForms[toolStripContainer] = toolStripContainer.GetToolStrips(availableWidth);
			    loadedForms[toolStripContainer]
                    .Where(ts => ts != null)
					.ForEach(ts => toolStripFlowLayoutPanel.Controls.Add(ts));
			}

		}

		private void RemoveToolStrips(IToolStripContainer toolStripOwner)
		{
		    toolStripFlowLayoutPanel.Controls
		        .OfType<LabeledToolStrip>()
		        .Where(toolStrip => toolStrip.Owner == toolStripOwner)
		        .ToList()
		        .ForEach(toolStrip => 
		            toolStripFlowLayoutPanel.Controls.Remove(toolStrip));
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.sbpText = new System.Windows.Forms.StatusBarPanel();
            this.sbpMemory = new System.Windows.Forms.StatusBarPanel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.ducumentToolStrip = new Origam.Gui.UI.LabeledToolStrip(null);
            this.toolsToolStrip = new Origam.Gui.UI.LabeledToolStrip(null);
            this.toolStripFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.toolStripPanel = new System.Windows.Forms.Panel();
            this.rightToolsTripLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.searchComboBox = new System.Windows.Forms.ComboBox();
            this.LicenseBackgroudExtender = new System.ComponentModel.BackgroundWorker();
            this.AutoUpdateBackgroudFinder = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.sbpText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpMemory)).BeginInit();
            this.toolStripFlowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.toolStripPanel.SuspendLayout();
            this.rightToolsTripLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockPanel
            // 
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.dockPanel.Location = new System.Drawing.Point(0, 119);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.ShowDocumentIcon = true;
            this.dockPanel.Size = new System.Drawing.Size(864, 464);            
            this.dockPanel.TabIndex = 7;
            // 
            // statusBar
            // 
            this.statusBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.statusBar.Location = new System.Drawing.Point(0, 583);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.sbpText,
            this.sbpMemory});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(864, 28);
            this.statusBar.TabIndex = 9;
            // 
            // sbpText
            // 
            this.sbpText.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.sbpText.Name = "sbpText";
            this.sbpText.Width = 833;
            // 
            // sbpMemory
            // 
            this.sbpMemory.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.sbpMemory.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.sbpMemory.Name = "sbpMemory";
            this.sbpMemory.Width = 10;
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(864, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // ducumentToolStrip
            // 
            this.ducumentToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ducumentToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ducumentToolStrip.MinimumSize = new System.Drawing.Size(0, 95);
            this.ducumentToolStrip.Name = "ducumentToolStrip";
            this.ducumentToolStrip.Size = new System.Drawing.Size(111, 95);
            this.ducumentToolStrip.TabIndex = 0;
            this.ducumentToolStrip.Text = strings.DocumentToolStripText;
            // 
            // toolsToolStrip
            // 
            this.toolsToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolsToolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolsToolStrip.MinimumSize = new System.Drawing.Size(0, 95);
            this.toolsToolStrip.Name = "toolsToolStrip";
            this.toolsToolStrip.Size = new System.Drawing.Size(111, 95);
            this.toolsToolStrip.TabIndex = 0;
            this.toolsToolStrip.Text = strings.ToolsToolStripText;
            // 
            // toolStripFlowLayoutPanel
            // 
            this.toolStripFlowLayoutPanel.AutoSize = true;
            this.toolStripFlowLayoutPanel.Controls.Add(this.ducumentToolStrip);
            this.toolStripFlowLayoutPanel.Controls.Add(this.toolsToolStrip);
            this.toolStripFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStripFlowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.toolStripFlowLayoutPanel.MaximumSize = new System.Drawing.Size(10000, 95);
            this.toolStripFlowLayoutPanel.MinimumSize = new System.Drawing.Size(100, 95);
            this.toolStripFlowLayoutPanel.Name = "toolStripFlowLayoutPanel";
            this.toolStripFlowLayoutPanel.Size = new System.Drawing.Size(222, 95);
            this.toolStripFlowLayoutPanel.TabIndex = 0;
            this.toolStripFlowLayoutPanel.WrapContents = false;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.Location = new System.Drawing.Point(3, 12);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.logoPictureBox.Size = new System.Drawing.Size(194, 32);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.logoPictureBox.TabIndex = 1;
            this.logoPictureBox.TabStop = false;
            // 
            // toolStripPanel
            // 
            this.toolStripPanel.Controls.Add(this.rightToolsTripLayoutPanel);
            this.toolStripPanel.Controls.Add(this.toolStripFlowLayoutPanel);
            this.toolStripPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolStripPanel.Location = new System.Drawing.Point(0, 24);
            this.toolStripPanel.MaximumSize = new System.Drawing.Size(10000, 95);
            this.toolStripPanel.MinimumSize = new System.Drawing.Size(150, 95);
            this.toolStripPanel.Name = "toolStripPanel";
            this.toolStripPanel.Size = new System.Drawing.Size(864, 95);
            this.toolStripPanel.TabIndex = 9;
            // 
            // rightToolsTripLayoutPanel
            // 
            this.rightToolsTripLayoutPanel.ColumnCount = 1;
            this.rightToolsTripLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.rightToolsTripLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.rightToolsTripLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.rightToolsTripLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
            this.rightToolsTripLayoutPanel.Controls.Add(this.searchComboBox, 0, 1);
            this.rightToolsTripLayoutPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightToolsTripLayoutPanel.Location = new System.Drawing.Point(664, 0);
            this.rightToolsTripLayoutPanel.Name = "rightToolsTripLayoutPanel";
            this.rightToolsTripLayoutPanel.RowCount = 2;
            this.rightToolsTripLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.rightToolsTripLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.rightToolsTripLayoutPanel.Size = new System.Drawing.Size(200, 95);
            this.rightToolsTripLayoutPanel.TabIndex = 2;
            // 
            // searchComboBox
            // 
            this.searchComboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchComboBox.FormattingEnabled = true;
            this.searchComboBox.Location = new System.Drawing.Point(3, 50);
            this.searchComboBox.Name = "searchComboBox";
            this.searchComboBox.Size = new System.Drawing.Size(194, 24);
            this.searchComboBox.TabIndex = 2;
            this.searchComboBox.TabStop = false;
		    this.searchComboBox.KeyDown += searchBox_KeyDown;
            this.searchComboBox.Text = "Search";
            // 
            // LicenseBackgroudExtender
            // 
            this.LicenseBackgroudExtender.DoWork += new System.ComponentModel.DoWorkEventHandler(this.LicenseBackgroudExtender_DoWork);
            // 
            // AutoUpdateBackgroudFinder
            // 
            this.AutoUpdateBackgroudFinder.DoWork += new System.ComponentModel.DoWorkEventHandler(this.AutoUpdateBackgroundFinder_DoWork);
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(864, 611);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.toolStripPanel);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.statusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "frmMain";
            this.Text = "ORIGAM Architect";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMain_Closing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sbpText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sbpMemory)).EndInit();
            this.toolStripFlowLayoutPanel.ResumeLayout(false);
            this.toolStripFlowLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.toolStripPanel.ResumeLayout(false);
            this.toolStripPanel.PerformLayout();
            this.rightToolsTripLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		internal bool AdministratorMode { get; set; } = false;

		public bool PopulateEmptyDatabaseOnLoad { get; set; } = true;

        public bool ApplicationDataDisconnectedMode { get; set; } = false;
        
        public void UpdateToolbar()
		{
			if(this.Disposing) return;

			foreach(object item in ducumentToolStrip.Items)
			{
				(item as IStatusUpdate)?.UpdateItemsToDisplay();
			}
			foreach (object item in toolsToolStrip.Items)
			{
				(item as IStatusUpdate)?.UpdateItemsToDisplay();
				}
			}

		private void PrepareMenuBars()
		{
            ducumentToolStrip.ItemClicked += toolStrip_ItemClicked;
			toolsToolStrip.ItemClicked += toolStrip_ItemClicked;
#if ORIGAM_CLIENT
			_fileMenu = new AsMenu(this,strings.File_Menu);
			_viewMenu = new AsMenu(this, strings.View_Menu);
			_helpMenu = new AsMenu(this, strings.Help_Menu);
			_windowMenu = new AsMenu(this,strings.Window_Menu);
			menuStrip.Items.Add(_fileMenu);
            menuStrip.Items.Add(_viewMenu);
            menuStrip.Items.Add(_windowMenu);
            menuStrip.Items.Add(_helpMenu);
#else
            _fileMenu = new AsMenu(this, strings.File_Menu);
			_schemaMenu = new AsMenu(this, strings.Model_Menu);		
			_viewMenu = new AsMenu(this, strings.View_Menu);
			_toolsMenu = new AsMenu(this, strings.Tools_Menu);
			_helpMenu = new AsMenu(this, strings.Help_Menu);
            _windowMenu = new AsMenu(this, strings.Window_Menu);
			menuStrip.Items.Add(_fileMenu);
            menuStrip.Items.Add(_viewMenu);
            menuStrip.Items.Add(_schemaMenu);
            menuStrip.Items.Add(_toolsMenu);
            menuStrip.Items.Add(_windowMenu);
            menuStrip.Items.Add(_helpMenu);
#endif
        }

		private void FinishMenuBars()
		{
			UpdateMenu();
			UpdateToolbar();
		}

		private void CreateMainMenu()
		{
			ducumentToolStrip.Items.Clear();
			toolsToolStrip.Items.Clear();
            _fileMenu.Clear();
			_shortcuts.Clear();
			_schemaMenu?.Clear();
			_viewMenu.Clear();
			_toolsMenu?.Clear();
			_helpMenu.Clear();
            _windowMenu?.Clear();
			CreateHelpMenu();
			CreateFileMenu();
            CreateWindowMenu();
			CreateViewMenuDisconnected();
			CreateProcessBrowserContextMenu();
			foreach(object o in menuStrip.Items)
			{
				(o as AsMenu)?.PopulateMenu();
				}
			}

		private void UnloadMainMenu()
		{
			CreateMainMenu();
			UpdateMenu();
			UpdateToolbar();
		}

		private void CreateMainMenuConnect()
		{
			#if ORIGAM_CLIENT
				CreateViewMenu();
			#else
				CreateViewMenu();
				CreateModelMenu();
				CreateToolsMenu();
				CreateModelBrowserContextMenu();
			#endif

			foreach(object o in menuStrip.Items)
			{
				(o as AsMenu)?.PopulateMenu();
				}
			UpdateMenu();
		}

		private void CreateProcessBrowserContextMenu()
		{
			// context menu for workflowPlayerPad
			AsContextMenu workflowPlayerContextMenu = new AsContextMenu(this);
			AsMenuCommand mnuMakeWorkflowRecurring = new AsMenuCommand(strings.Retry_MenuCommand);
			mnuMakeWorkflowRecurring.Command = new Commands.MakeWorkflowRecurring();
			mnuMakeWorkflowRecurring.Command.Owner = mnuMakeWorkflowRecurring;
			mnuMakeWorkflowRecurring.Image = Images.RecurringWorkflow;
			mnuMakeWorkflowRecurring.Click += MenuItemClick;

			workflowPlayerContextMenu.AddSubItem(mnuMakeWorkflowRecurring);
			_workflowPad.ebrSchemaBrowser.ContextMenuStrip = workflowPlayerContextMenu;
		}

		private void CreateModelBrowserContextMenu()
		{
			IList<ToolStripItem> clonedIems 
				= CloneToolStripItems(_schemaMenu.SubItems);
			
			AsContextMenu schemaContextMenu = new AsContextMenu(this);
			schemaContextMenu.AddSubItems(clonedIems);
			
			_schema.SchemaContextMenu = schemaContextMenu;
		}

		private IList<ToolStripItem> CloneToolStripItems(
			IList<ToolStripItem> items)
			{
			var clonedItems = new List<ToolStripItem>();
			foreach (var item in items)
			{
				switch (item)
				{
					case AsMenuCommand command:
						clonedItems.Add(new AsMenuCommand(command));
						break;
					case ToolStripSeparator _:
						clonedItems.Add(new ToolStripSeparator());
						break;
					default:
						throw new Exception($"Need a copy constructor for {item.GetType()} here.");	
			}
			}

			return clonedItems;
		}


		private void CreateFileMenu()
		{
#if ORIGAM_CLIENT
			AsMenuCommand mnuSave = CreateMenuItem(strings.Save_MenuCommand, new SaveContent(), Images.Save, Keys.Control | Keys.S, _fileMenu);
			AsMenuCommand mnuRefresh = CreateMenuItem(strings.Refresh_MenuCommand, new RefreshContent(), Images.Refresh, Keys.Control | Keys.R, _fileMenu);
			AsMenuCommand mnuFinishWorkflowTask = CreateMenuItem(strings.FinishTask_MenuCommand, new Commands.FinishWorkflowTask(), Images.Forward, Keys.F5, _fileMenu);

			_fileMenu.SubItems.Add(CreateSeparator());

			CreateMenuItem(strings.Connect_MenuItem, new ConnectRepository(), Images.Home, Keys.None, _fileMenu);
			CreateMenuItem(strings.Disconnect_MenuItem, new DisconnectRepository(), null, Keys.None, _fileMenu);

			if(AdministratorMode)
			{
				CreateMenuItem(strings.ConnectionConfig_MenuItem, new Origam.Workbench.Commands.EditConfiguration(), Images.ConnectionConfiguration, Keys.None, _fileMenu);

				_fileMenu.SubItems.Add(CreateSeparator());
				
				CreateMenuItem(strings.Refresh_MenuItem, new Commands.UpdateModelAndTargetEnvironment(), null, Keys.None, _fileMenu);
				CreateMenuItem(strings.RunRefreshActions_MenuItem, new DeployVersion(), null, Keys.None, _fileMenu);
			}

			_fileMenu.SubItems.Add(CreateSeparator());
			
			CreateMenuItem(strings.Exit_MenuItem, new ExitWorkbench(), null, Keys.None, _fileMenu);

	
		    ducumentToolStrip.Items.Add(CreateButtonFromMenu(mnuSave,ImageRes.Save));
            ducumentToolStrip.Items.Add(CreateButtonFromMenu(mnuRefresh,ImageRes.Refresh));
            ducumentToolStrip.Items.Add(CreateButtonFromMenu(mnuFinishWorkflowTask,ImageRes.FinishTask));
	
			searchComboBox.Enabled = false;
			searchComboBox.Visible = false;
			rightToolsTripLayoutPanel.Controls.Remove(this.searchComboBox);
			rightToolsTripLayoutPanel.RowCount = 1;
			logoPictureBox.Dock = DockStyle.Fill;

#else
            CreateMenuItem(strings.NewProject_MenuItem, new Commands.CreateNewProject(), Images.New, Keys.None, _fileMenu);

            _fileMenu.SubItems.Add(CreateSeparator());
            
            AsMenuCommand mnuSave = CreateMenuItem(strings.Save_MenuCommand, new SaveContent(), Images.Save, Keys.Control | Keys.S, _fileMenu);
			AsMenuCommand mnuRefresh = CreateMenuItem(strings.Refresh_MenuCommand, new RefreshContent(), Images.Refresh, Keys.Control | Keys.R, _fileMenu);
			AsMenuCommand mnuFinishWorkflowTask = CreateMenuItem(strings.FinishTask_MenuCommand, new Commands.FinishWorkflowTask(), Images.Forward, Keys.F5, _fileMenu);
			
			_fileMenu.SubItems.Add(CreateSeparator());

			CreateMenuItem(strings.Connect_MenuItem, new ConnectRepository(), Images.Home, Keys.None, _fileMenu);
			CreateMenuItem(strings.Disconnect_MenuItem, new DisconnectRepository(), null, Keys.None, _fileMenu);
			CreateMenuItem(strings.ConnectionConfig_MenuItem, new EditConfiguration(), Images.ConnectionConfiguration, Keys.None, _fileMenu);
			AsMenuCommand mnuPersistSchema = CreateMenuItem(strings.UpdateModelRepository_MenuItem, new PersistSchema(), Images.SaveToDatabase, Keys.Control | Keys.Shift | Keys.S, _fileMenu);
			AsMenuCommand mnuServerRestart = CreateMenuItem(strings.SetServerRestart_MenuItem, new Commands.SetServerRestart(), Images.RestartServer, Keys.None, _toolsMenu);
            SqlViewer vwr = new SqlViewer();
            AsMenuCommand mnuShowSqlConsole = CreateMenuItem(strings.SqlConsole_MenuItem, 
                new ShowSqlConsole(""), vwr.Icon.ToBitmap(), Keys.None, _toolsMenu);

			_fileMenu.SubItems.Add(CreateSeparator());
			
			CreateMenuItem(strings.ImportPackagesFromRepository_MenuItem, new ImportPackagesFromRepository(), null, Keys.None, _fileMenu);
			CreateMenuItem(strings.ExportModel_MenuItem, new ExportSchemaToFile(), null, Keys.None, _fileMenu);
			CreateMenuItem(strings.ImportUpdatedModel_MenuItem, new ImportSchemaFromFile(), null, Keys.None, _fileMenu);
			CreateMenuItem(strings.ExportSinglePackage_MenuItem, new ExportPackageToFile(), null, Keys.None, _fileMenu);
			CreateMenuItem(strings.ImportSinglePackage_Menuitem, new ImportPackageFromFile(), null, Keys.None, _fileMenu);
			CreateMenuItem(strings.RunUpdateScripts_MenuItem, new DeployVersion(), null, Keys.None, _fileMenu);
			CreateMenuItem(strings.UpdateModel_MenuItem, new UpdateModelAndTargetEnvironment(), null, Keys.None, _fileMenu);
			
			_fileMenu.SubItems.Add(CreateSeparator());
			
			CreateMenuItem(strings.Exit_MenuItem, new ExitWorkbench(), null, Keys.None, _fileMenu);

			ducumentToolStrip.Items.Add(CreateButtonFromMenu(mnuSave,ImageRes.Save));
            ducumentToolStrip.Items.Add(CreateButtonFromMenu(mnuRefresh,ImageRes.Refresh));
            ducumentToolStrip.Items.Add(CreateButtonFromMenu(mnuFinishWorkflowTask,ImageRes.FinishTask));
           
			toolsToolStrip.Items.Add(CreateButtonFromMenu(mnuPersistSchema,ImageRes.UpdateModelRepository));
			toolsToolStrip.Items.Add(CreateButtonFromMenu(mnuServerRestart,ImageRes.RestartServer));
#endif
        }
		private void CreateHelpMenu()
		{
#if ORIGAM_CLIENT
			CreateMenuItem(strings.About_MenuItem, new Commands.ViewAboutScreen(), null, Keys.None, _helpMenu);
#else
            CreateMenuItem(strings.Help_MenuItem, new Commands.ShowHelp(), null, Keys.F1, _helpMenu);
            CreateMenuItem(strings.CommunityForums_MenuItem, new Commands.ShowCommunity(), null, Keys.None, _helpMenu);
            _helpMenu.SubItems.Add(CreateSeparator());	
            CreateMenuItem(strings.About_MenuItem, new Commands.ViewAboutScreen(), null, Keys.None, _helpMenu);
#endif
		}

        private void CreateWindowMenu()
        {
#if ORIGAM_CLIENT
			CreateMenuItem(strings.Close_MenuItem, new Commands.CloseWindow(), null, Keys.None, _windowMenu);
			CreateMenuItem(strings.CloseAll_MenuItem, new Commands.CloseAllWindows(), null, Keys.None, _windowMenu);
            CreateMenuItem(strings.CloseAllButThis_MenuItem, new Commands.CloseAllButThis(), null, Keys.None, _windowMenu);
#else
            CreateMenuItem(strings.Close_MenuItem, new Commands.CloseWindow(), null, Keys.None, _windowMenu);
            CreateMenuItem(strings.CloseAll_MenuItem, new Commands.CloseAllWindows(), null, Keys.None, _windowMenu);
            CreateMenuItem(strings.CloseAllButThis_MenuItem, new Commands.CloseAllButThis(), null, Keys.None, _windowMenu);
#endif
        }

		private void CreateViewMenuDisconnected()
		{
#if ORIGAM_CLIENT
			if(AdministratorMode)
			{
				CreateMenuItem(strings.Properties_MenuItem, new ViewPropertyPad(), Images.PropertyPad, Keys.F4, _viewMenu);
				CreateMenuItem(strings.Output_MenuItem, new ViewOutputPad(), Images.Output, Keys.None, _viewMenu);
			}

			CreateMenuItem(strings.Menu_MenuItem, new Commands.ViewProcessBrowserPad(), _workflowPad.Icon.ToBitmap(), Keys.F2, _viewMenu);
			CreateMenuItem(strings.WorkQueue_MenuItem, new Commands.ViewWorkQueuePad(), null, Keys.None, _viewMenu);
#else
            CreateMenuItem(strings.Properties_MenuItem, new ViewPropertyPad(), _propertyPad.Icon.ToBitmap(), Keys.F4, _viewMenu);
			CreateMenuItem(strings.Output_MenuItem, new ViewOutputPad(), _outputPad.Icon.ToBitmap(), Keys.None, _viewMenu);
			CreateMenuItem(strings.Log_MenuItem, new ViewLogPad(), _logPad.Icon.ToBitmap(), Keys.None, _viewMenu);
            CreateMenuItem(strings.ServerLog_MenuItem, new ViewServerLogPad(), _serverLogPad.Icon.ToBitmap(), Keys.None, _viewMenu);
            CreateMenuItem(strings.ModelBrowser_MenuItem, new ViewSchemaBrowserPad(), _schemaBrowserPad.Icon.ToBitmap(), Keys.F3, _viewMenu);
			CreateMenuItem(strings.WorkQueue_MenuItem, new Commands.ViewWorkQueuePad(), null, Keys.None, _viewMenu);
#endif
		}

		private void CreateViewMenu()
		{
#if ORIGAM_CLIENT
			CreateMenuItem(strings.Attachements_MenuItem, new ViewAttachmentPad(), Images.Attachment, Keys.None, _viewMenu);
			CreateMenuItem(strings.AuditLog_MenuItem, new ViewAuditLogPad(), Images.History, Keys.None, _viewMenu);
#else
            CreateMenuItem(strings.PackageBrowser_MenuItem, new ViewExtensionPad(), _extensionPad.Icon.ToBitmap(), Keys.None, _viewMenu);
			CreateMenuItem(strings.WorkflowWatch_MenuItem, new Commands.ViewWorkflowWatchPad(), _workflowWatchPad.Icon.ToBitmap(), Keys.None, _viewMenu);
			CreateMenuItem(strings.Documentation_MenuItem, new ViewDocumentationPad(), _documentationPad.Icon.ToBitmap(), Keys.None, _viewMenu);
			CreateMenuItem(strings.FindSchemaItemResults_MenuItem, new ViewFindSchemaItemResultsPad(), _findSchemaItemResultsPad.Icon.ToBitmap(), Keys.None, _viewMenu);
#endif
		}

		private void CreateModelMenu()
		{
			AsMenuCommand schemaNewMenu = CreateMenuWithSubmenu(strings.New_MenuItem, ImageRes.icon_new, new SchemaItemEditorsMenuBuilder(), _schemaMenu);
			CreateMenuItem(strings.NewGroup_MenuItem, new AddNewGroup(), ImageRes.icon_new_group, Keys.None, _schemaMenu);
            CreateMenuItem(strings.RepeatNew_MenuItem, new AddRepeatingSchemaItem(), ImageRes.icon_repeat_new, Keys.F12, _schemaMenu);
			CreateMenuWithSubmenu(strings.Actions_MenuItem, ImageRes.icon_actions, new SchemaActionsMenuBuilder(), _schemaMenu);
			CreateMenuWithSubmenu(strings.ConvertTo_MenuItem, ImageRes.icon_convert_to, new SchemaItemConvertMenuBuilder(), _schemaMenu);
			CreateMenuWithSubmenu(strings.MoveToPackage_MenuItem, ImageRes.icon_move_to_package, new ExtensionMenuBuilder(), _schemaMenu);
				
			_schemaMenu.SubItems.Add(CreateSeparator());

            CreateMenuItem(strings.ExpandAll,
                new ExpandAllActiveSchemaItem(), ImageRes.Arrow, Keys.None,
                _schemaMenu);
            AsMenuCommand mnuEditSchemaItem = CreateMenuItem(strings.EditItem_MenuItem, 
                new EditActiveSchemaItem(), ImageRes.icon_edit_item, Keys.None, 
                _schemaMenu);
			AsMenuCommand mnuDelete = CreateMenuItem(strings.Delete_MenuItem, 
                new DeleteActiveNode(), ImageRes.icon_delete, Keys.None, _schemaMenu);
			CreateMenuItem(strings.Execute_MenuItem, new ExecuteActiveSchemaItem(), 
                ImageRes.icon_execute, Keys.Control | Keys.X, _schemaMenu);
			_schemaMenu.SubItems.Add(CreateSeparator());	
	
			CreateMenuItem(strings.FindDependencies_MenuItem, new ShowDependencies(),
                ImageRes.icon_find_dependencies, Keys.None, _schemaMenu);
			CreateMenuItem(strings.FindReferences_MenuItem, new ShowUsage(), 
                ImageRes.icon_find_references, Keys.None, _schemaMenu);
            _schemaMenu.SubItems.Add(CreateSeparator());
            CreateMenuItem(strings.SourceXml_MenuItem, new ShowExplorerXml(), 
                ImageRes.icon_show_in_explorer, Keys.None, _schemaMenu);
            CreateMenuItem(strings.XmlConsole, new ShowConsoleXml(), 
                ImageRes.icon_show_xml, Keys.None, _schemaMenu);
            AsMenuCommand schemamenuGit = CreateMenuWithSubmenu("Git", Images.Git, new GitMenuBuilder(), _schemaMenu);
        }

		private void CreateToolsMenu()
		{
			CreateMenuItem(strings.DeploymentScriptGenerator_MenuItem, new Commands.ShowDbCompare(), Images.DeploymentScriptGenerator, Keys.None, _toolsMenu);
            CreateMenuItem(strings.ShowWebApplication_MenuItem, new Commands.ShowWebApplication(), null, Keys.None, _toolsMenu);
            CreateMenuItem(strings.GenerateGUID_MenuItem, new Commands.GenerateGuid(), null, Keys.Control | Keys.Shift | Keys.G, _toolsMenu);
			CreateMenuItem(strings.DumpWindowXML_MenuItem, new Commands.DumpWindowXml(), null, Keys.None, _toolsMenu);
			CreateMenuItem(strings.ShowTrace_MenuItem, new Commands.ShowTrace(), null, Keys.Control | Keys.T, _toolsMenu);
			CreateMenuItem(strings.ResetUserCache_MenuItem, new Commands.ResetUserCaches(), null, Keys.None, _toolsMenu);
			CreateMenuItem(strings.RebuildLocalizationFiles_MenuItem, new Commands.GenerateLocalizationFile(), null, Keys.None, _toolsMenu);
            CreateMenuItem("Convert to File Storage", new Commands.ConvertModelToFileStorage(), null, Keys.None, _toolsMenu);
			CreateMenuItem("Compare to Original DB data", new Commands.CompareToOriginalDBData(), null, Keys.None, _toolsMenu);
        }

        private void UpdateMenu()
		{
			if(this.Disposing) return;

			foreach(object item in menuStrip.Items)
			{
				if(item is IStatusUpdate)
				{
					(item as IStatusUpdate).UpdateItemsToDisplay();
				}
			}
		}

        private ToolStripSeparator CreateSeparator()
		{
            return new ToolStripSeparator();
		}

		private AsMenuCommand CreateMenuWithSubmenu(string name, Image image, ISubmenuBuilder builder, AsMenu parentMenu)
		{
			AsMenuCommand result = new AsMenuCommand(name);
			if(image != null)
			{
				result.Image = image;
			}
			result.SubItems.Add(builder);
			parentMenu.SubItems.Add(result);
			return result;
		}

		private AsButtonCommand CreateButtonFromMenu(AsMenuCommand menu)
		{
			return
				new AsButtonCommand(menu.Description)
				{
					Image = menu.Image,
					Command = menu.Command
				};
		}
		private AsButtonCommand CreateButtonFromMenu(AsMenuCommand menu,Image newImage)
		{
			return
				new AsButtonCommand(menu.Description)
				{
					Image = newImage,
					Command = menu.Command
				};
		}

		private AsMenuCommand CreateMenuItem(string text, ICommand command, Image image)
		{
			AsMenuCommand menuItem = new AsMenuCommand(text, command);
			menuItem.Click += MenuItemClick;
			
			if(image != null)
			{
				menuItem.Image = image;
            }

			return menuItem;
		}

		private AsMenuCommand CreateMenuItem(string text, ICommand command, Image image, Keys shortcut, AsMenu parentMenu)
		{
			AsMenuCommand result = CreateMenuItem(text, command, image);
			if(shortcut != Keys.None)
			{
				result.ShortcutKeys = shortcut;
				_shortcuts[shortcut] = result;
			}
			parentMenu.SubItems.Add(result);
			return result;
		}

		private IDockContent GetContentFromPersistString(string persistString)
		{
			foreach(IPadContent pad in this.PadContentCollection)
			{
				if(persistString == pad.GetType().ToString())
					return pad as DockContent;
			}
			
			return null;
		}
		
		private void LoadWorkspace()
		{
			dockPanel.SuspendLayout(true);
			try
			{
				if (File.Exists(_configFilePath) && dockPanel.Contents.Count == 0)
				{
					dockPanel.LoadFromXml(_configFilePath,GetContentFromPersistString);
				}
				else
				{
					// Default Workspace

#if ORIGAM_CLIENT
				new Commands.ViewProcessBrowserPad().Run();
#else
				new ViewExtensionPad().Run();
#endif
				}
			}
			finally
			{
				dockPanel.ResumeLayout(true, true);
			}
		}

		private void frmMain_Closing(object sender,CancelEventArgs e)
		{
			if(! Disconnect())
			{
				e.Cancel = true;
			}
			else
			{
				e.Cancel = false;
			}
		}


		#region IWorkbench Members

		public bool IsConnected { get; private set; } 

		public string Title
		{
			get => this.Text;
			set => this.Text = value;
			}

		public ViewContentCollection ViewContentCollection { get; } 
			= new ViewContentCollection();

		public PadContentCollection PadContentCollection { get; } 
			= new PadContentCollection();

		public IViewContent ActiveViewContent
			=> this.dockPanel.ActiveContent as IViewContent;

		public IViewContent ActiveDocument
			=> this.dockPanel.ActiveDocument as IViewContent;

		public int WorkflowFormsCount
		{
			get
			{
				int result = 0;
				foreach(IViewContent content in this.ViewContentCollection)
				{
					if(content is Origam.Workflow.WorkflowForm)
					{
						result++;
					}
				}

				return result;
			}
		}

		public void ShowView(IViewContent content)
		{
			if(ConfigurationManager.GetActiveConfiguration() is OrigamSettings settings &&
			   settings.MaxOpenTabs > 0 &&
			   this.dockPanel.DocumentsCount == settings.MaxOpenTabs)
			{
				throw new Exception("Too many open documents. Please close some documents first.");
			}

			ViewContentCollection.Add(content);
			((DockContent) content).Show(dockPanel);
			((DockContent) content).Closed += DockContentClosed;
			OnViewOpened(new ViewContentEventArgs(content));
		}

		public void ShowPad(IPadContent content)
		{
			DockContent dock = content as DockContent;

			switch(dock.DockState)
			{
				case DockState.DockBottomAutoHide:
					dock.DockState = DockState.DockBottom;
					break;
				case DockState.DockLeftAutoHide:
					dock.DockState = DockState.DockLeft;
					break;
				case DockState.DockRightAutoHide:
					dock.DockState = DockState.DockRight;
					break;
				case DockState.DockTopAutoHide:
					dock.DockState = DockState.DockTop;
					break;
			}

			dock.Show(dockPanel);
		}

		public T GetPad<T>() where T: IPadContent
		{
			return (T)GetPad(typeof(T));
		}

		public IPadContent GetPad(Type type)
		{
			foreach (IPadContent pad in PadContentCollection) 
			{
				// try the type itself
				if (pad.GetType() == type) 
				{
					return pad;
				}

				if(type.IsInterface)
				{
					// try all interfaces
					foreach(Type interfaceType in pad.GetType().GetInterfaces())
					{
						if(interfaceType == type) return pad;
					}
				}
			}

			return null;
		}

		public void CloseContent(IViewContent content)
		{
            ((DockContent) content).Close();
		}

		public void CloseAllViews(IViewContent except)
		{
            foreach (DockContent content in this.dockPanel.DocumentsToArray())
            {
                if (except == null || !except.Equals(content))
                {
                    content.Close();
                    Application.DoEvents();
                }
            }
        }

		public void CloseAllViews()
		{
            CloseAllViews(null);
		}

		public void RedrawAllComponents()
		{
			// TODO:  Add frmMain.RedrawAllComponents implementation
		}

		public event Origam.UI.ViewContentEventHandler ViewOpened;

		public event Origam.UI.ViewContentEventHandler ViewClosed;

		public event System.EventHandler ActiveWorkbenchWindowChanged;

		#endregion

	    protected override void OnResizeEnd(EventArgs e)
	    {
	        UpdateToolStrips();
	    }

        protected override void OnResize(EventArgs e)
	    {
	        base.OnResize(e);
            if (WindowState != lastWindowState)
	        {
	            lastWindowState = WindowState;
	            UpdateToolStrips();
            }
	    }

	    protected virtual void OnViewOpened(ViewContentEventArgs e)
		{
			e.Content.DirtyChanged += Content_DirtyChanged;
			UpdateToolbar();
			HandleAttachments();
			if (ActiveMdiChild is IToolStripContainer tsContainer)
			{
				tsContainer.ToolStripsLoaded += OnChildFormToolStripsLoaded;
			}

			ViewOpened?.Invoke(this, e);
        }
		
		protected virtual void OnViewClosed(ViewContentEventArgs e)
		{
			if(e.Content is IRecordReferenceProvider)
			{
				(e.Content as IRecordReferenceProvider).RecordReferenceChanged -= AttachmentDocument_ParentIdChanged;
			}
			
			this.ViewContentCollection.Remove(e.Content);
			UpdateToolbar();
			HandleAttachments();

			ViewClosed?.Invoke(this, e);
			CleanUpToolStripsWhenClosing(closingForm: e.Content);
			}

		void OnActiveWindowChanged(object sender, EventArgs e)
		{
			UpdateToolbar();
			HandleAttachments();
			if(!closeAll && ActiveWorkbenchWindowChanged != null) 
			{
				ActiveWorkbenchWindowChanged(this, e);
			}
			if (ActiveDocument is AbstractEditor editor)
            {
                editor.ActionsBuilder = new SchemaActionsMenuBuilder();
                editor.NewElementsBuilder = new SchemaItemEditorsMenuBuilder(showDialog: true);
			}
			UpdateToolStrips();
        }

		private void HandleAttachments()
		{
			if(this.ActiveDocument is IRecordReferenceProvider)
			{
				// Last window closed
				if(this.ViewContentCollection.Count == 0)
				{
					AttachmentDocument_ParentIdChanged(this, Guid.Empty, Guid.Empty, new Hashtable());
					return;
				}

				// deactivate all attachment notifications
				foreach(IViewContent content in this.ViewContentCollection)
				{
					if(content is IRecordReferenceProvider)
					{
						(content as IRecordReferenceProvider).RecordReferenceChanged -= AttachmentDocument_ParentIdChanged;
					}
				}

				// Check if there is still an active document
				if(this.ActiveDocument == null)
				{
					AttachmentDocument_ParentIdChanged(this, Guid.Empty, Guid.Empty, new Hashtable());
				}
				else
				{
					// fire current parent id
					AttachmentDocument_ParentIdChanged(this,
						(this.ActiveDocument as IRecordReferenceProvider).MainEntityId,
						(this.ActiveDocument as IRecordReferenceProvider).MainRecordId,
						(this.ActiveDocument as IRecordReferenceProvider).ChildRecordReferences
						);
				
					// subscribe to notifications
					(this.ActiveDocument as IRecordReferenceProvider).RecordReferenceChanged += AttachmentDocument_ParentIdChanged;
				}
			}
			else
			{
				AttachmentDocument_ParentIdChanged(this, Guid.Empty, Guid.Empty, new Hashtable());
			}
		}

		private void MenuItemClick(object sender, EventArgs e)
		{
			// User clicked the menu, so we run the command associated with the
			// menu item.

			UpdateMenu();
			UpdateToolbar();

			try
			{
				this.Cursor = Cursors.WaitCursor;
				if(sender is AsMenuCommand)
				{
					if(((AsMenuCommand)sender).IsEnabled)
					{
						((AsMenuCommand)sender).Command.Run();
					}
				}
				else if(sender is AsButtonCommand)
				{
					if(((AsButtonCommand)sender).IsEnabled)
					{
						((AsButtonCommand)sender).Command.Run();
					}
				}
			}
			catch(Exception ex)
			{
				AsMessageBox.ShowError(this, ex.Message, strings.GenericError_Title, ex);
				this.Cursor = Cursors.Default;
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		private void DockContentClosed(object sender, EventArgs e)
		{
			try
			{
				OnViewClosed(new ViewContentEventArgs(sender as IViewContent));
				(sender as DockContent).Closed -= DockContentClosed;
			
				(sender as DockContent).DockPanel = null;

				_statusBarService.SetStatusMemory(GC.GetTotalMemory(true));
			}
			catch{}
		}

        public void Connect()
        {
	        Connect(null);
	        SubscribeToPersistenceServiceEvents();
        }

        private static void InitializeMetaModelUpgradeService()
        {
	        ServiceManager.Services.AddService(new MetaModelUpgradeService());
        }

        private void SubscribeToPersistenceServiceEvents()
		{
			var currentPersistenceService =
				ServiceManager.Services.GetService<IPersistenceService>();

			if (currentPersistenceService is FilePersistenceService filePersistService)
			{
				filePersistService.ReloadNeeded += OnFilePersistServiceReloadRequested;
			}
		}

		void OnFilePersistServiceReloadRequested(object sender, FileSystemChangeEventArgs args)
		{
			FilePersistenceService filePersistenceService =
				(FilePersistenceService) sender;
			FilePersistenceProvider filePersistenceProvider
				= (FilePersistenceProvider) filePersistenceService.SchemaProvider;
            bool reloadConfirmed = true;
            if (ShouldRaiseWarning(filePersistenceProvider, args.File))
            {
                reloadConfirmed = this.RunWithInvoke(() => ConfirmReload(args));
            }
            if (reloadConfirmed)
            {
                Maybe<XmlLoadError> mayBeError = TryLoadModelFiles(filePersistenceService);
                if (mayBeError.HasValue)
                {
                    this.RunWithInvoke(Disconnect);
                }
                else
                {
                    this.RunWithInvokeAsync(() => UpdateUIAfterReload(filePersistenceProvider, args));
                }
                this.RunWithInvoke(DoModelChecksAsync); 
            }
		}

	    private bool ConfirmReload(FileSystemChangeEventArgs args)
        {
            // try/catch block for unhandled exceptions 
            // main try/catch block doesn't show error message 
            // and leaves application in unstable state
            try
            {
                DialogResult dialogResult = FlexibleMessageBox.Show(
                    this,
                    $"Model file changes detected!{Environment.NewLine}{Environment.NewLine}{args}.{Environment.NewLine}{Environment.NewLine}Do you want to reload the model?",
                    "Changes in Model Directory Detected",
                    MessageBoxButtons.YesNo);
                return dialogResult == DialogResult.Yes;
            }
            catch(Exception ex)
            {
                AsMessageBox.ShowError(
                    null, ex.Message, strings.GenericError_Title, ex);
            }
            return false;
        }

        private void UpdateUIAfterReload(
	        FilePersistenceProvider filePersistenceProvider, 
            FileSystemChangeEventArgs args)
        {
            // try/catch block for unhandled exceptions 
            // main try/catch block doesn't show error message 
            // and leaves application in unstable state
            try
            {
                GetPad<SchemaBrowser>()
                    ?.EbrSchemaBrowser
                    .ReloadTreeAndRestoreExpansionState();
                GetPad<ExtensionPad>()?.LoadPackages();
                GetPad<FindSchemaItemResultsPad>()?.Clear();
	            GetPad<DocumentationPad>()?.Reload();
                ReloadOpenWindows(filePersistenceProvider);
            }
            catch(Exception ex)
            {
                AsMessageBox.ShowError(null, ex.Message,strings.GenericError_Title, ex);
            }
        }

        private bool ShouldRaiseWarning(
            FilePersistenceProvider filePersistProvider, 
            FileInfo file)
        {
	        return ViewContentCollection
		        .Cast<IViewContent>()
		        .Select(view =>
			        filePersistProvider.FindPersistedObjectInfo(view.DisplayedItemId))
		        .Where(objInfo => objInfo != null)
		        .Any(objInfo => objInfo.OrigamFile.Path.EqualsTo(file));
        }

		private Maybe<XmlLoadError> TryLoadModelFiles(FilePersistenceService filePersistService)
		{
			Maybe<XmlLoadError> maybeError =
				filePersistService.Reload(tryUpdate: false);

			if (maybeError.HasNoValue) return null;
			XmlLoadError error = maybeError.Value;
			switch (error.Type)	
			{
				case ErrType.XmlGeneralError:
				    this.RunWithInvoke(() => MessageBox.Show(this, error.Message));
                    return maybeError;
				case ErrType.XmlVersionIsOlderThanCurrent:
					return TryHandleOldVersionFound(filePersistService, error.Message) 
					    ? null 
					    : maybeError;
				default:
					throw new NotImplementedException();
			}
		}

		private bool TryHandleOldVersionFound(FilePersistenceService filePersistService,
			string message)
		{
			DialogResult updateVersionsResult = MessageBox.Show(this,
				$"{message}{Environment.NewLine}Do you want to upgrade this file and all other files with old versions?",
				"Old Meta Model Version Detected", MessageBoxButtons.YesNo);
			if (updateVersionsResult != DialogResult.Yes) return false;
			MessageBox.Show(this,
				$"This functionality has not been implemented yet.{Environment.NewLine}No files will be reloaded!");
			Maybe<XmlLoadError> reloadError =
				filePersistService.Reload(tryUpdate: true);
			if (reloadError.HasValue)
			{
				throw new NotImplementedException();
			}
			return false; // change to true, when filePersistService.Reload(tryUpdate: true) is implemented!
		}

		private void ReloadOpenWindows(
			FilePersistenceProvider filePersistenceProvider)
		{
			List<IViewContent> openViewList = ViewContentCollection
				.Cast<IViewContent>()
			    .Where(x => CanBeReOpened(filePersistenceProvider, x))
				.ToList();
			
			IViewContent originallyActiveContent = 
				(IViewContent)dockPanel.ActiveDocument;
			ReopenViewContents(filePersistenceProvider, openViewList);

			if (originallyActiveContent is SchemaCompareEditor)
			{
				ActivateViewByType(typeof(SchemaCompareEditor));
			}
			else
			{
				ActivateViewByContent(originallyActiveContent);
			}
		}

		private static bool CanBeReOpened(
			FilePersistenceProvider filePersistenceProvider, IViewContent viewContent)
		{
			if (viewContent is SchemaCompareEditor) return true;
			if (viewContent.LoadedObject == null) return false;
			IPersistent loadedObject = (IPersistent) viewContent.LoadedObject;
			return filePersistenceProvider.Has(loadedObject.Id);
		}

		private void ActivateViewByType(Type typeToActivate)
		{
			ViewContentCollection
				.Cast<IViewContent>()
				.Where(cont => cont.GetType() == typeToActivate)
				.Cast<DockContent>()
				.First()
				.Activate();
		}

		private void ActivateViewByContent(IViewContent refContent)
		{
			if(refContent == null) return;
			IBrowserNode2 loadedObject = (IBrowserNode2)refContent.LoadedObject;
			
			ViewContentCollection
				.Cast<IViewContent>()
			    .Where(cont => cont.LoadedObject !=null)
				.Where(cont => 
					((IBrowserNode2)cont.LoadedObject).NodeId == loadedObject.NodeId)
				.Where(content => refContent.GetType() == content.GetType())
				.Cast<DockContent>()
				.Single()
				.Activate();
		}

		private void ReopenViewContents(
			FilePersistenceProvider filePersistenceProvider,
			List<IViewContent> openViewList)
		{
			foreach (IViewContent viewContent in openViewList)
			{
				Maybe<AbstractCommand> maybeReOpenCommand 
                    = GetCommandToReOpen(viewContent);
                if(maybeReOpenCommand.HasNoValue)
                {
                    continue;
                }
				CloseContent(viewContent);
				AbstractCommand reOpenCommand = maybeReOpenCommand.Value;
				if (viewContent is SchemaCompareEditor)
				{
					reOpenCommand.Run();
					continue;
				}
				IFilePersistent loadedObject = 
					RefreshLoadedObject(filePersistenceProvider, viewContent);
				reOpenCommand.Owner = loadedObject;
				reOpenCommand.Run();
			}
		}

		private static IFilePersistent RefreshLoadedObject(
			FilePersistenceProvider filePersistenceProvider, IViewContent viewContent)
		{
			var loadedObject = (IFilePersistent) viewContent.LoadedObject;
			if (loadedObject == null) throw new Exception("loadedObject not set");
			filePersistenceProvider.RefreshInstance(loadedObject);
			return loadedObject;
		}

		private Maybe<AbstractCommand> GetCommandToReOpen(IViewContent viewContent)
		{
			if (viewContent is SchemaCompareEditor) return new ShowDbCompare();
			if (viewContent is AbstractViewContent) return new EditSchemaItem();
			if (viewContent is AsForm) return new ExecuteSchemaItem();
			return null;
		}

		public void Connect(string configurationName)
		{
			// Ask for configuration
            if (!LoadConfiguration(configurationName))
			{
				return;
			}         
            InitializeMetaModelUpgradeService();
            SubscribeToUpgradeServiceEvents();
            Application.DoEvents();

            foreach (DockContent content in this.dockPanel.Documents.ToList())
			{
                content.Close();
			}

			if(this.dockPanel.DocumentsCount > 0)
			{
				// could not close all the documents
				return;
			}

			try
			{
				if(this.WindowState != FormWindowState.Normal)
				{
					this.WindowState = FormWindowState.Normal;
				}

				_statusBarService.SetStatusText(strings.ConnectingToModelRepository_StatusText);

				// Login to the repository
				try
				{
					if(! LoadSchemaList())
					{
						return;
					}
				}
				catch(Exception ex)
				{
					AsMessageBox.ShowError(
                        this,
                        string.Format(strings.RepositoryLoginFailedMessage, Environment.NewLine + ex.Message),
                        strings.RepositoryLoginFailedTitle,
                        ex);

					return;
				}
				_schema.SchemaBrowser = _schemaBrowserPad;

				// Init services
				InitializeConnectedServices();

				// Initialize model-connected user interface
				InitializeConnectedPads();
                CreateMainMenuConnect();
				IsConnected = true;
#if !ORIGAM_CLIENT
				DoModelChecksAsync();
#endif

#if ORIGAM_CLIENT
				OrigamSettings settings = ConfigurationManager.GetActiveConfiguration() ;

				try
				{
					_schema.LoadSchema(settings.DefaultSchemaExtensionId, settings.ExtraSchemaExtensionId, false, AdministratorMode);
				}
				catch(Exception ex)
				{
					this.Show();
					AsMessageBox.ShowError(this, ex.Message, strings.LoadingModelErrorTitle, ex);
                
					Disconnect();
					return;
				}
				
				CheckModelRootPackageVersion();
#endif
                UpdateTitle();
            }
            finally
			{
				_statusBarService.SetStatusText("");
				this.WindowState = FormWindowState.Maximized;
			}

#if ORIGAM_CLIENT
			Commands.ViewProcessBrowserPad cmd = new Commands.ViewProcessBrowserPad();
#else
			ViewExtensionPad cmd = new ViewExtensionPad();
#endif
			this.LoadWorkspace();
			cmd.Run();
		}

		private void SubscribeToUpgradeServiceEvents()
		{
			var metaModelUpgradeService = ServiceManager.Services.GetService<IMetaModelUpgradeService>();

			metaModelUpgradeService.UpgradeStarted += (sender, args) =>
			{
				Task.Run(() =>
				{
					using (var form = new ModelUpgradeForm(metaModelUpgradeService))
					{
						form.ShowDialog();
					}
				});
			};
		}

		public void DoModelChecksAsync()
	    {
	       var currentPersistenceService =
	            ServiceManager.Services.GetService<IPersistenceService>();
           if (!(currentPersistenceService is FilePersistenceService)) return;

	       var cancellationToken = modelCheckCancellationTokenSource.Token;
	        Task.Factory.StartNew(
	            () => DoModelChecks(cancellationToken),
	            cancellationToken 
            ).ContinueWith(
	            TaskErrorHandler,
	            TaskScheduler.FromCurrentSynchronizationContext()
            );
	    }

	    private void TaskErrorHandler(Task previousTask)
	    {
	        try
	        {
	            previousTask.Wait();
	        }
	        catch (AggregateException ae)
	        {
	            if (!(ae.InnerException is OperationCanceledException))
	            {
	                log.Error(ae.InnerException);
	                this.RunWithInvoke(() => AsMessageBox.ShowError(null, ae.InnerException.Message, strings.GenericError_Title, ae.InnerException));
	            }
	        }
	    }

	    private void DoModelChecks(CancellationToken cancellationToken)
	    {
            using (FilePersistenceService independentPersistenceService = new FilePersistenceBuilder()
	            .CreateNoBinFilePersistenceService())
            {
                List<Dictionary<IFilePersistent, string>> errorFragments =
                    ModelRules.GetErrors(
                        schemaProviders: new OrigamProviderBuilder()
                            .SetSchemaProvider(independentPersistenceService.SchemaProvider)
                            .GetAll(), 
                        independentPersistenceService: independentPersistenceService, 
                        cancellationToken: cancellationToken); 

	            var persistenceProvider = (FilePersistenceProvider)independentPersistenceService.SchemaProvider;
	            var errorSections = persistenceProvider.GetFileErrors(
	                ignoreDirectoryNames: new []{ ".git","l10n"},
	                cancellationToken: cancellationToken);

	            if (errorFragments.Count != 0)
	            {
                    FindRulesPad resultsPad = WorkbenchSingleton.Workbench.GetPad(typeof(FindRulesPad)) as FindRulesPad;
                    this.RunWithInvoke(() =>
                       {
                           DialogResult dialogResult = MessageBox.Show(
                               "Some model elements do not satisfy model integrity rules. Do you want to show the rule violations?",
                               "Model Errors",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                           if (dialogResult == DialogResult.Yes)
                           {
                               resultsPad.DisplayResults(errorFragments);
                           }
                       }
                    );
                }
	            if (errorSections.Count != 0)
	            {
		            this.RunWithInvoke(() =>
		            {
			            FlexibleMessageBox.Show(
				            "The following errors were found in the loaded model:\n\n" +
				            string.Join("\n\n", errorSections), "Model Errors");
		            });
	            }
	        }
	    }
	    
	    protected override void WndProc(ref Message m)
		{
			if(m.Msg == 16)
			{
				CancelEventArgs args1 = new CancelEventArgs(false);
				if (m.Msg == 0x16)
				{
					args1.Cancel = m.WParam == IntPtr.Zero;
				}
				else
				{
					args1.Cancel = !base.Validate();
					this.OnClosing(args1);
					if (m.Msg == 0x11)
					{
						m.Result = args1.Cancel ? IntPtr.Zero : ((IntPtr) 1);
					}
				}
				if ((m.Msg != 0x11) && !args1.Cancel)
				{
					this.OnClosed(EventArgs.Empty);
					base.Dispose();
				}
			}
			else
			{
				base.WndProc (ref m);
			}
		}


		public bool UnloadSchema()
		{
            CloseAllViews();
            // make sure the application knows all windows were closed otherwise
            // it might throw an error when trying to set an active form which
            // is not there anymore
            Application.DoEvents();

            if(this.dockPanel.DocumentsCount > 0)
			{
				// could not close all the documents
				return false;
			}

			if(_schema.IsSchemaChanged && _schema.SupportsSave)
			{
				DialogResult result = MessageBox.Show(this, strings.ModelChangedSaveToRepQuestion, strings.ModelChanged_Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

				switch(result)
				{
					case DialogResult.Yes:
						_schema.SaveSchema();
						break;
					case DialogResult.Cancel:
						return false;
				}
			}
			_workflowPad.OrigamMenu = null;
			
#if ! ORIGAM_CLIENT
			_findSchemaItemResultsPad?.ResetResults();
			_documentationPad?.ClearDocumentation();
#endif
			_auditLogPad?.ClearList();
			return true;
		}

        public bool Disconnect()
		{
			try
			{
				if (IsConnected)
				{
					SaveWorkspace();
				}

				if (!_schema.Disconnect()) return false;
                ClearReferenceIndex();
				UnloadConnectedServices();
				UnloadConnectedPads();
				UnloadMainMenu();

				IsConnected = false;
				ConfigurationManager.SetActiveConfiguration(null);

				UpdateTitle();

				modelCheckCancellationTokenSource.Cancel();
				modelCheckCancellationTokenSource =
					new CancellationTokenSource();
				return true;
			}
			catch (Exception ex)
			{
				log.Error(ex);
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}

			return true;
		}

        private void ClearReferenceIndex()
        {
            IPersistenceService persistenceService =
                 ServiceManager.Services.GetService<IPersistenceService>();
            if (persistenceService != null)
            {
                ReferenceIndexManager.ClearReferenceIndex(true);
            }
        }

        private void UnloadConnectedPads()
        {
            GetPad<AuditLogPad>().ClearList();
            GetPad<ExtensionPad>().UnloadPackages();
            GetPad<FindSchemaItemResultsPad>().Clear();
            GetPad<FindRulesPad>().Clear();
        }

        private void SaveWorkspace()
		{
			try
			{
				dockPanel.SaveAsXml(_configFilePath);
			} 
			catch {}
		}
		/// <summary>
		/// Get configuration and load it. If there is more than 1 configuration, display selection dialog.
		/// </summary>
		private bool LoadConfiguration(string configurationName)
		{
			OrigamSettingsCollection configurations =
				ConfigurationManager.GetAllConfigurations();

            if (configurationName != null)
            {
                foreach (OrigamSettings config in configurations)
                {
                    if (config.Name == configurationName)
                    {
                        ConfigurationManager.SetActiveConfiguration(config);
                        return true;
                    }
                }
                throw new ArgumentOutOfRangeException(nameof(configurationName),
                    configurationName,
                    strings.ConfigurationNotFound_ExceptionMessage);
            }
            else if (configurations.Count == 0)
            {
                Commands.CreateNewProject cmd = new Commands.CreateNewProject();
                cmd.Run();
                return false;
            }
			else if(configurations.Count == 1)
			{
				ConfigurationManager.SetActiveConfiguration(configurations[0]);
			}
			else
			{
				ConfigurationSelector selector = new ConfigurationSelector();
				selector.Configurations = configurations;

				if(selector.ShowDialog(this) == DialogResult.OK)
				{
					ConfigurationManager.SetActiveConfiguration(selector.SelectedConfiguration);
				}
				else
				{
					return false;
				}

				Application.DoEvents();
			}

			return true;
		}

		private void UnloadConnectedServices()
		{
		    IControlsLookUpService controlsLookupService =
				ServiceManager.Services.GetService<IControlsLookUpService>();
			if (controlsLookupService != null)
			{
                ServiceManager.Services.UnloadService(controlsLookupService);
			}

		    var persistenceService =
		        ServiceManager.Services.GetService<IPersistenceService>();
		    if (persistenceService is FilePersistenceService filePersistService)
		    {
		        filePersistService.ReloadNeeded -= OnFilePersistServiceReloadRequested;
		    }

		    FilePersistenceBuilder.Clear();
            OrigamEngine.UnloadConnectedServices();
        }

		/// <summary>
		/// After configuration is selected, connect to the repository and load the model list from the repository.
		/// </summary>
		private bool LoadSchemaList()
		{
			IPersistenceService persistence = OrigamEngine.CreatePersistenceService();
			ServiceManager.Services.AddService(persistence);

			try
			{
				bool isRepositoryVersionCompatible = false;
				bool isRepositoryEmpty = false;
				try
				{
					isRepositoryVersionCompatible 
						= persistence.IsRepositoryVersionCompatible();
				}
                catch (DatabaseProcedureNotFoundException ex)
				{
					if(ex.ProcedureName == "OrigamDatabaseSchemaVersion")
					{
						// let's assume repository is empty
						isRepositoryEmpty = true;
					}
					else
					{
						throw;
					}
				}
				if(! isRepositoryVersionCompatible && ! isRepositoryEmpty)
				{
					bool shouldUpdate = false;

#if ORIGAM_CLIENT
				shouldUpdate = AdministratorMode;
#else
					shouldUpdate = true;
#endif

					if(shouldUpdate & persistence.CanUpdateRepository())
					{
						if(MessageBox.Show(this, strings.UpgradeModelQuestion, strings.UpgradeModelTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
						{
							persistence.UpdateRepository();
						}
						else
						{
							MessageBox.Show(this, strings.CannotLogin_Message, strings.CannotLogin_Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
							ServiceManager.Services.UnloadService(persistence);
							return false;
						}
					}
					else
					{
						MessageBox.Show(this, strings.CannotLogin_Message2, strings.CannotLogin_Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
						ServiceManager.Services.UnloadService(persistence);
						return false;
					}
				}
				else if(isRepositoryEmpty)
				{
					if(MessageBox.Show(this, strings.RepositoryInitializeQuestion, strings.Initialize_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					{
                        persistence.InitializeRepository();
					}
					else
					{
						MessageBox.Show(this, strings.CannotLogin_Message, strings.CannotLogin_Title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
						ServiceManager.Services.UnloadService(persistence);
						return false;
					}
				}
				persistence.LoadSchemaList();
				return true;
			}
			catch
			{
				ServiceManager.Services.UnloadService(persistence);
				throw;
			}
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.WindowsPrincipal);
			AsMessageBox.DebugInfoProvider = new Origam.Workflow.DebugInfo();
			SplashScreen splash = new SplashScreen();
			splash.Show();
			Application.DoEvents();
			InitializeDefaultServices();
			InitializeDefaultPads();

			//this.LoadWorkspace();
			
			// Menu and toolbars
			PrepareMenuBars();
			CreateMainMenu();
			FinishMenuBars();
	
			this.dockPanel.ActiveDocumentChanged += dockPanel_ActiveDocumentChanged;
            this.dockPanel.ContentRemoved += dockPanel_ContentRemoved;
			this.dockPanel.ActiveContentChanged += dockPanel_ActiveContentChanged;



#if !ORIGAM_CLIENT
			// auto-update within release-branch
			AutoUpdateBackgroudFinder.RunWorkerAsync();
			// search Origam.com for whether there is a newer architect version within the same release branch
			// as the current branch
#endif
			Connect();
			splash.Dispose();
		}


		private void InitializeDefaultPads()
		{
            // this will not be used in the Client, but we need to have an instance, because icons are taken from it
            _extensionPad = CreatePad<ExtensionPad>();
            _attachmentPad = CreatePad<AttachmentPad>();
            _auditLogPad = CreatePad<AuditLogPad>();
            _schema.SchemaListBrowser = _extensionPad;

#if ORIGAM_CLIENT
			_schemaBrowserPad = new SchemaBrowser();

			if(AdministratorMode)
			{
				_propertyPad = new PropertyPad();
				this.PadContentCollection.Add(_propertyPad);

				_outputPad = new OutputPad();
				this.PadContentCollection.Add(_outputPad);
			}
#else

            _schemaBrowserPad = CreatePad<SchemaBrowser>();
            _workflowWatchPad = CreatePad<WorkflowWatchPad>();
            _propertyPad = CreatePad<PropertyPad>();
            _logPad = CreatePad<LogPad>();
            _outputPad = CreatePad<OutputPad>();
            _documentationPad = CreatePad<DocumentationPad>();
            _findSchemaItemResultsPad = CreatePad<FindSchemaItemResultsPad>();
            _findRulesPad = CreatePad<FindRulesPad>();
#endif

            _workflowPad = CreatePad<WorkflowPlayerPad>();
            _serverLogPad = CreatePad<ServerLogPad>();
            CreatePad<WorkQueuePad>();
		}

		private void InitializeConnectedPads()
		{
#if !ORIGAM_CLIENT
            GetPad<ExtensionPad>()?.LoadPackages();
#endif
        }
        private T CreatePad<T>()
        {
            var pad = Activator.CreateInstance<T>();
            PadContentCollection.Add((IPadContent)pad);
            return pad;
        }

		/// <summary>
		/// After successfully loading the model list, we initialize model-connected services
		/// </summary>
		private void InitializeConnectedServices()
		{
			ServiceManager.Services.AddService(new ServiceAgentFactory());
			ServiceManager.Services.AddService(new StateMachineService());
			ServiceManager.Services.AddService(OrigamEngine.CreateDocumentationService());
			ServiceManager.Services.AddService(new TracingService());
            ServiceManager.Services.AddService(new DataLookupService());
            ServiceManager.Services.AddService(new ControlsLookUpService());
            ServiceManager.Services.AddService(new DeploymentService());
			ServiceManager.Services.AddService(new ParameterService());
			ServiceManager.Services.AddService(new Origam.Workflow.WorkQueue.WorkQueueService());
			ServiceManager.Services.AddService(new AttachmentService());
			ServiceManager.Services.AddService(new RuleEngineService());
		}

		private void InitializeDefaultServices()
		{
			// Status bar service
			_statusBarService = new StatusBarService(statusBar);
			ServiceManager.Services.AddService(_statusBarService);

			// Schema service
			_schema = new WorkbenchSchemaService();
			ServiceManager.Services.AddService(_schema);
			_schema.SchemaLoaded += _schema_SchemaLoaded;
			_schema.SchemaUnloading += _schema_SchemaUnloading;
#if ! ORIGAM_CLIENT
			_schema.ActiveNodeChanged += _schema_ActiveNodeChanged;
#endif
		}

		private void _schema_ActiveNodeChanged(object sender, EventArgs e)
		{
			UpdateToolbar();
            AbstractSqlDataService abstractSqlDataService = DataService.GetDataService() as AbstractSqlDataService;
            AbstractSqlCommandGenerator abstractSqlCommandGenerator = (AbstractSqlCommandGenerator)abstractSqlDataService.DbDataAdapterFactory;
            if (_schema.ActiveSchemaItem != null)
			{
				ShowDocumentation cmd = new ShowDocumentation();
				cmd.Run();
			}

			if(_schema.ActiveNode is TableMappingItem)
			{
				try
				{
					_outputPad.SetOutputText(abstractSqlCommandGenerator.TableDefinitionDdl(_schema.ActiveNode as TableMappingItem));
					_outputPad.AppendText(abstractSqlCommandGenerator.ForeignKeyConstraintsDdl(_schema.ActiveNode as TableMappingItem));
				}
				catch(Exception ex)
				{
					_outputPad.SetOutputText(ex.Message);
				}
			}

			if(_schema.ActiveNode is Function)
			{
				try
				{
					_outputPad.SetOutputText(abstractSqlCommandGenerator.FunctionDefinitionDdl(_schema.ActiveNode as Function));
				}
				catch(Exception ex)
				{
					_outputPad.SetOutputText(ex.Message);
				}
			}

			if(_schema.ActiveNode is DataStructure)
			{
				try
				{
					DatasetGenerator gen = new DatasetGenerator(false);

					_outputPad.SetOutputText(gen.CreateDataSet(_schema.ActiveNode as DataStructure).GetXmlSchema());
				}
				catch(Exception ex)
				{
					_outputPad.SetOutputText(ex.Message);
				}
			}

		}

		private void dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
		{
			if(this.dockPanel.ActiveDocument != null)
				OnActiveWindowChanged(sender, new EventArgs());
		}

		private void dockPanel_ContentRemoved(object sender, DockContentEventArgs e)
		{
			UpdateToolbar();

			if(e.Content is IViewContent)
			{
				(e.Content as IViewContent).StatusTextChanged -= ActiveViewContent_StatusTextChanged;
				_statusBarService.SetStatusText("");
			}
		}

		private void dockPanel_ActiveContentChanged(object sender, EventArgs e)
		{
			UpdateToolbar();
			if(this.ActiveViewContent != null)
			{
				this.ActiveViewContent.StatusTextChanged += new EventHandler(ActiveViewContent_StatusTextChanged);
				
				// do this last, because there is DoEvents inside, which can cause ActiveViewContent to be null
				_statusBarService.SetStatusText(this.ActiveViewContent.StatusText);
			}
		}

		private void Content_DirtyChanged(object sender, EventArgs e)
		{
			UpdateToolbar();
		}

		private void ActiveViewContent_StatusTextChanged(object sender, EventArgs e)
		{
			if(this.ActiveViewContent != null)
			{
				_statusBarService.SetStatusText(this.ActiveViewContent.StatusText);
			}
		}

		// Handle keyboard shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_shortcuts.Contains(keyData))
            {
                ToolStripItem menu = _shortcuts[keyData] as ToolStripItem;
                AsMenuCommand cmd = menu as AsMenuCommand;
                if (menu.Enabled || (cmd != null && cmd.IsEnabled))
                {
                    if (cmd != null)
                    {
                        // PerformClick() does not fire anything if Enabled=false
                        cmd.UpdateItemsToDisplay();
                    }   
                    menu.PerformClick();
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

		private void _schema_SchemaLoaded(object sender, EventArgs e)
		{
			OrigamEngine.InitializeSchemaItemProviders(_schema);
            IDeploymentService deployment 
                = ServiceManager.Services.GetService<IDeploymentService>();

#if ORIGAM_CLIENT
			deployment.CanUpdate(_schema.ActiveExtension);
			string modelVersion = _schema.ActiveExtension.Version;
			string dbVersion = deployment.CurrentDeployedVersion(_schema.ActiveExtension);

			if(modelVersion != dbVersion)
			{
				if(AdministratorMode)
				{
					string message = string.Format(
                        strings.ModelDatabaseVersionMissmatch_Message,
                        Environment.NewLine, 
                        modelVersion + Environment.NewLine,
                        dbVersion);

					AsMessageBox.ShowError(this, message, strings.ConnectionTitle, null);
				}
				else
				{
					string message = string.Format(
                        strings.ModelDatabaseVersionMissmatchNoAdmin_Message,
                        Environment.NewLine, 
                        modelVersion + Environment.NewLine,
                        dbVersion);

					throw new Exception(message);
				}
			}
#else
            ApplicationDataDisconnectedMode 
                = !TestConnectionToApplicationDataDatabase();
            if (ApplicationDataDisconnectedMode)
            {
                UpdateTitle();
                return;
            }
            bool isEmpty = deployment.IsEmptyDatabase();
            // data database is empty and we are not supposed to ask for running init scripts
            // that means the new project wizard is running and will take care
            if (isEmpty && !PopulateEmptyDatabaseOnLoad)
            {
                return;
            }
            if (isEmpty)
            {
                if (MessageBox.Show(strings.RunInitDatabaseScriptsQuestion,
                    strings.DatabaseEmptyTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    deployment.Deploy();
                }
            }
			Origam.Workbench.Commands.DeployVersion deployCommand = 
                new Origam.Workbench.Commands.DeployVersion();
	        if(deployCommand.IsEnabled)
            {
                if (MessageBox.Show(strings.RunDeploymentScriptsQuestion,
                    strings.DeploymentSctiptsPending_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    deployment.Deploy();
	                GetPad<ExtensionPad>()?.LoadPackages();
                }
            }

#endif
			IParameterService parameterService = 
                ServiceManager.Services.GetService(typeof(IParameterService)) as IParameterService;

			try
			{
				parameterService.RefreshParameters();
			}
			catch(Exception ex)
			{
				// show the error but go on
				// error can occur e.g. when duplicate constant name is loaded, e.g. due to incompatible packages
				AsMessageBox.ShowError(this, ex.Message, strings.ErrorWhileLoadingParameters_Message, ex);
			}

#if ! ORIGAM_CLIENT
            // we have to initialize the new user after parameter service gets loaded
            // otherwise it would fail generating SQL statements
            if (isEmpty)
            {
                string userName = SecurityManager.CurrentPrincipal.Identity.Name;
                if (MessageBox.Show(string.Format(strings.AddUserToUserList_Question,
                    userName),
                    strings.DatabaseEmptyTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    IOrigamProfileProvider profileProvider = SecurityManager.GetProfileProvider();
                    profileProvider.AddUser("Architect (" + userName + ")", userName);
                }
            }
#endif

			MenuSchemaItemProvider menuProvider = _schema.GetProvider(typeof(MenuSchemaItemProvider)) as MenuSchemaItemProvider;
			if(menuProvider.ChildItems.Count > 0)
			{
				_workflowPad.OrigamMenu = menuProvider.ChildItems[0] as Origam.Schema.MenuModel.Menu;
			}
            UpdateTitle();
		}

        private void UpdateTitle()
        {
#if ORIGAM_CLIENT
			Title = "";
#else
            Title = strings.OrigamArchitect_Title;
#endif
			OrigamSettings settings 
                = ConfigurationManager.GetActiveConfiguration() ;
            if(settings == null)
            {
                return;
            }
			if(settings.TitleText != "")
			{
                if(Title != "")
                {
                    Title += " - ";
                }
				Title += settings.TitleText;
			}
            if(_schema?.ActiveExtension == null)
            {
                return;
            }
#if !ORIGAM_CLIENT
            Title += " - ";
            Title += _schema.ActiveExtension.Name;
#endif
			Title += string.Format(
                strings.ModelVersion_Title, 
                _schema?.ActiveExtension?.VersionString,
                (ApplicationDataDisconnectedMode 
                    ? strings.Disconnected : ""));
        }

		private void AttachmentDocument_ParentIdChanged(object sender, Guid mainEntityId, Guid mainRecordId, Hashtable childReferences)
		{
			try
			{
				_attachmentPad?.GetAttachments(mainEntityId, mainRecordId, childReferences);
				}
			catch
			{
			}
		}


		public void ProcessGuiLink(IOrigamForm sourceForm, object linkTarget, Hashtable parameters)
		{
			AbstractMenuItem targetMenuItem = linkTarget as AbstractMenuItem;

			OrigamArchitect.Commands.ExecuteSchemaItem cmd = new OrigamArchitect.Commands.ExecuteSchemaItem();

			if(sourceForm != null && (targetMenuItem is FormReferenceMenuItem))
			{
				if((targetMenuItem as FormReferenceMenuItem).Screen.PrimaryKey.Equals(sourceForm.PrimaryKey))
				{
					object[] val = new object[parameters.Count];

					int i = 0;
					foreach(DictionaryEntry entry in parameters)
					{
						val[i] = entry.Value;
						
						i++;
					}

					sourceForm.SetPosition(val);
					return;
				}
			}

			foreach(DictionaryEntry entry in parameters)
			{
				cmd.Parameters.Add(entry.Key, entry.Value);
			}

			cmd.RecordEditingMode = true;
			cmd.Owner = targetMenuItem;

			cmd.Run();
		}

		public void ExitWorkbench()
		{
			this.Close();
		}

		private void _schema_SchemaUnloading(object sender, CancelEventArgs e)
		{
			e.Cancel = ! UnloadSchema();
		}

		private void CheckModelRootPackageVersion()
		{
			if(! AdministratorMode)
			{
				bool found = false;
				foreach(Package extension in _schema.LoadedPackages)
				{
					if((Guid)extension.PrimaryKey["Id"] == new Guid("147FA70D-6519-4393-B5D0-87931F9FD609"))
					{
						if(! extension.VersionString.Equals("5.0"))
						{
							MessageBox.Show(this, strings.ModelVersionErrorMessage, strings.ModelVersionTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
							Disconnect();
							return;
						}

						found = true;
						break;

					}
				}

				if(! found)
				{
					MessageBox.Show(this, strings.ModelMissingMessage, strings.ModelVersionTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
					Disconnect();
					return;
				}
			}
		}

		private void searchBox_KeyDown(object sender, KeyEventArgs e)
		{
#if ORIGAM_CLIENT
#else
			if(e.KeyCode == Keys.Enter)
			{
				if(_schema.IsSchemaLoaded)
                {
                    string text = (sender as ComboBox).Text;
                    if (text.Equals(String.Empty)) return;
                    _findSchemaItemResultsPad.ResetResults();
                    IPersistenceService persistence = ServiceManager.Services.GetService(typeof(IPersistenceService)) as IPersistenceService;
                    AbstractSchemaItem[] results = persistence.SchemaProvider.FullTextSearch<AbstractSchemaItem>(text);

                    if (results.LongLength > 0)
                    {
                        _findSchemaItemResultsPad.DisplayResults(results);
                    }

					MessageBox.Show(this, string.Format(strings.ResultCountMassage, results.LongLength) , strings.SearchResultTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                e.Handled = true;
			}
#endif
		}

        void toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			MenuItemClick(e.ClickedItem, EventArgs.Empty);
		}

        public void ExportToExcel(string name, ArrayList list)
        {
            try
            {
                ExcelFormat excelFormat = ExcelTools.StringToExcelFormat(
                    (ConfigurationManager.GetActiveConfiguration()
                    ).GUIExcelExportFormat);
                SaveFileDialog sfd = GetExportToExcelSaveDialog(excelFormat);
                if(sfd.ShowDialog(this) == DialogResult.Cancel)
                {
                    return;
                }
                IWorkbook workbook = ExcelTools.GetWorkbook(excelFormat);

                // CREATE CELL STYLES
                // So they can be reused later. There is a limit of 4000 cell styles in Excel 2003 and earlier, so
                // we have to create only as many styles as neccessary.
                _dateCellStyle = workbook.CreateCellStyle();
                _dateCellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("m/d/yy h:mm");

                ////create a entry of SummaryInformation
                if(name != null)
                {
                    ExcelTools.SetWorkbookSubject(workbook, name);
                }

                ISheet sheet1 = workbook.CreateSheet("Data");
                IRow headerRow = sheet1.CreateRow(0);

                ArrayList firstRow = list[0] as ArrayList;

                for(int i = 0; i < firstRow.Count; i++)
                {
                    headerRow.CreateCell(i).SetCellValue((string)firstRow[i]);
                }

                for(int rowNumber = 1; rowNumber < list.Count; rowNumber++)
                {
                    IRow excelRow = sheet1.CreateRow(rowNumber);

                    ArrayList row = list[rowNumber] as ArrayList;

                    for(int i = 0; i < row.Count; i++)
                    {
                        object val = row[i];

                        if(val is DateTime)
                        {
                            ICell cell = excelRow.CreateCell(i);
                            cell.SetCellValue((DateTime)val);
                            cell.CellStyle = _dateCellStyle;
                        }
                        else if(val is int || val is double || val is float || val is decimal)
                        {
                            excelRow.CreateCell(i).SetCellValue(Convert.ToDouble(val));
                        }
                        else if(val != null)
                        {
                            string fieldValue = row[i].ToString();
                            if(fieldValue.IndexOf("\r") > 0)
                            {
                                fieldValue = fieldValue.Replace("\n", "");
                                fieldValue = fieldValue.Replace("\r", Environment.NewLine);
                                fieldValue = fieldValue.Replace("\t", " ");
                                excelRow.CreateCell(i).SetCellValue(fieldValue);
                            }
                            else
                            {
                                excelRow.CreateCell(i).SetCellValue(fieldValue);
                            }
                        }
                    }
                }

                using(Stream s = sfd.OpenFile())
                {
                    workbook.Write(s);
                }

                System.Diagnostics.Process.Start(sfd.FileName);
            }
            catch(Exception ex)
            {
                AsMessageBox.ShowError(this, ex.Message, strings.ExcelExportError_Message, ex);
            }
        }

        private SaveFileDialog GetExportToExcelSaveDialog(
            ExcelFormat excelFormat)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if(excelFormat == ExcelFormat.XLSX)
            {
                sfd.Filter = "Microsoft Excel | *.xlsx";
                sfd.DefaultExt = "xlsx";
            }
            else
            {
                sfd.Filter = "Microsoft Excel | *.xls";
                sfd.DefaultExt = "xls";
            }
            sfd.Title = strings.ExcepExport_Title;
            return sfd;
        }

        private void AutoUpdate()
		{
			System.Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

			string url = null;
			string newVersion = null;
			
			// don't update if run from architect (0.0.0.0)
			if (version.Build == 0 && version.Major == 0 && version.MajorRevision == 0
				&& version.Minor == 0 && version.MinorRevision == 0 && version.Revision == 0)
			{
				return;
    }

			if (version.Major == 0)
			{
				url = string.Format("{0}{1}",
					ORIGAM_COM_API_BASEURL,
					"public/getSetupNewestBuildInMasterBranch");
}
			else
			{
				/*
				  https://origam.com/public/getSetupNewestBuildInStableBranch?stableBranch=2016.3
				*/
				url = string.Format("{0}public/getSetupNewestBuildInStableBranch?stableBranch={1}.{2}",
					ORIGAM_COM_API_BASEURL, version.Major, version.Minor);
			}

			try
			{
				using (WebResponse webResponse = HttpTools.GetResponse(url,
					"GET", null, null,
					new Hashtable()
					{ { "Accept-Encoding", "gzip,deflate"} }
					, null, null, null, 10000, null, IgnoreHTTPSErrors))
				{
					string output;
					HttpWebResponse httpWebResponse = webResponse as HttpWebResponse;
					output = HttpTools.ReadResponseTextRespectionContentEncoding(httpWebResponse);
					JObject jResult = (JObject)JsonConvert.DeserializeObject(output);

					int newestBuildVersion = int.Parse(((string)jResult["ROOT"]
						["Releases_Build"][0]["BuildVersion"]));
					string upgradeUrl = (string)jResult["ROOT"]["Releases_Build"][0]["Url"];

					if (version.Major == 0)
					{
						newVersion = string.Format("0.0.0.{0}", newestBuildVersion);
					}
					else
					{
						newVersion = string.Format("{0}.{1}.{2}.0", version.Major,
							version.Minor, newestBuildVersion);
					}

					if (newVersion.CompareTo(version.ToString()) > 0)
					{

						string message = string.Format(
							strings.AutoUpdate_NewerVersionNotice,
							newVersion, version.ToString());

						string caption = strings.NewerVersion_Title;
						MessageBoxButtons buttons = MessageBoxButtons.YesNo;
						DialogResult result;
						// Displays the MessageBox.
						result = MessageBox.Show(message, caption, buttons);

						if (result == System.Windows.Forms.DialogResult.Yes)
						{
							System.Diagnostics.Process.Start(upgradeUrl);
							// Closes the parent form.
							this.Close();
						}
					}
				}
			}
			catch (Exception e)
			{
				// continue if auto-upgrade fails
			}
		}

		private static string ExtendAndSaveLicense(string licenseString)
		{
			//prepare the request
			Cursor.Current = Cursors.WaitCursor;
			JObject jobj = new JObject(
				new JProperty("Releases_ClientLicense_API",
					new JObject(
						new JProperty("LicenseXmlString", licenseString)
					)
				)
			);

			string output = null;
			try
			{
				using (WebResponse webResponse = HttpTools.GetResponse(
				string.Format("{0}public/ExtendClientLicense", ORIGAM_COM_API_BASEURL),
				"POST", jobj.ToString(), "application/json",
				new Hashtable()
				{ { "Accept-Encoding", "gzip,deflate"} }
				, null, null, null, 10000, null,
				IgnoreHTTPSErrors))
				{
					HttpWebResponse httpWebResponse = webResponse as HttpWebResponse;
					output = HttpTools.ReadResponseTextRespectionContentEncoding(httpWebResponse);
					if (httpWebResponse.StatusCode == HttpStatusCode.OK)
					{
						JObject jResult = (JObject)JsonConvert.DeserializeObject(output);
						Origam.Licensing.License license = Origam.Licensing.License.Load(
							jResult["ROOT"]["Releases_ClientLicense_API"][0]["LicenseXmlString"].ToString());

						OrigamLicenseHelper.StoreLicenseToRegistry(license.ToString());

						// success
						return null;
					}
				}
			}
			catch (WebException wex)
			{
				if (wex.Status == WebExceptionStatus.Timeout || wex.Status == WebExceptionStatus.ConnectFailure)
				{
					// http error
					return wex.Message;
				}
				string errorInfo = null;
				using (HttpWebResponse httpWebResponse = wex.Response as HttpWebResponse)
				{
					if (httpWebResponse != null && httpWebResponse.StatusCode == HttpStatusCode.BadRequest)
					{
						errorInfo = HttpTools.ReadResponseTextRespectionContentEncoding(httpWebResponse);
						try
						{
							JObject jResult = (JObject)JsonConvert.DeserializeObject(errorInfo);
							if (jResult["Message"] != null)
							{
								return jResult["Message"].ToString();
							}
						}
						catch
						{ }
					}
				}
				return string.Format(strings.RegisterLoginForm_UnexpectedErrorWithMessage_Message,
					wex.Message);
			}
			return string.Format(strings.RegisterLoginForm_UnexpectedError_Message);
		}

		private void LicenseBackgroudExtender_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker bw = sender as BackgroundWorker;

			string extensionFailureReason = null;
			string licenseString = (string)e.Argument;

			Origam.Licensing.License license = Origam.Licensing.License.Load(licenseString);
			
			extensionFailureReason = ExtendAndSaveLicense(licenseString);
			if (!string.IsNullOrEmpty(extensionFailureReason))
			{
				if ((license.Expiration - DateTime.Now).Days < 7)
				{
					// license hasn't been extended			
					// license still valid, but warn
					MessageBox.Show(String.Format(
						strings.LicenseWillExpire_Message,
							(license.Expiration - DateTime.Now).Days,
							license.Expiration,
							extensionFailureReason
						),
						strings.LicenseWillExpire_Label,
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning);
				}
			}
			e.Result = true;

			// If the operation was canceled by the user, 
			// set the DoWorkEventArgs.Cancel property to true.
			if (bw.CancellationPending)
			{
				e.Cancel = true;
			}
		}
		
		private void AutoUpdateBackgroundFinder_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker bw = sender as BackgroundWorker;
			AutoUpdate();
			e.Result = true;
			if (bw.CancellationPending)
			{
				e.Cancel = true;
			}
		}

        private static bool TestConnectionToApplicationDataDatabase()
        {
            AbstractSqlDataService abstractSqlDataService = DataService.GetDataService() as AbstractSqlDataService;
            try
            {
                abstractSqlDataService.ExecuteUpdate("SELECT 1",null);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
