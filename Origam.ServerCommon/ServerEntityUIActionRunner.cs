﻿using System;
using System.Collections;
using System.Data;
using System.Xml;
using Origam.Rule;
using Origam.Schema.GuiModel;
using Origam.Schema.MenuModel;
using Origam.Schema.WorkflowModel;
using Origam.Workbench.Services;
using Origam.Workbench.Services.CoreServices;
using Origam.Gui;
using Origam.ServerCommon;

namespace Origam.Server
{
    public class ServerEntityUIActionRunner: EntityUIActionRunner
    {
        private readonly UIManager uiManager;
        private readonly SessionManager sessionManager;
        private readonly IBasicUIService basicUiService;
        private readonly IReportManager reportManager;

        public ServerEntityUIActionRunner(IEntityUIActionRunnerClient actionRunnerClient,
            UIManager uiManager, SessionManager sessionManager, IBasicUIService basicUiService,
            IReportManager reportManager) : base(actionRunnerClient)
        {
            this.uiManager = uiManager;
            this.sessionManager = sessionManager;
            this.basicUiService = basicUiService;
            this.reportManager = reportManager;
        }

        protected override void PerformAppropriateAction(
            ExecuteActionProcessData processData)
        {
            switch (processData.Type)
            {
                case PanelActionType.QueueAction:
                    ExecuteQueueAction(processData);
                    break;
                case PanelActionType.Report:
                    ExecuteReportAction(processData);
                    break;
                case PanelActionType.Workflow:
                    ExecuteWorkflowAction(processData); 
                    break;
                case PanelActionType.ChangeUI:
                    ExecuteChangeUIAction(processData);
                    break;
                case PanelActionType.OpenForm:
                    ExecuteOpenFormAction(processData);
                    break;
                case PanelActionType.SelectionDialogAction:
                    ExecuteSelectionDialogAction(processData);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        
        private static void CheckSelectedRowsCountPositive(int count)
        {
            if (count == 0)
            {
                throw new RuleException(Resources.ErrorNoRecordsSelectedForAction);
            }
        }

        private void ExecuteQueueAction(
            ExecuteActionProcessData processData)
        {
            WorkQueueSessionStore wqss = sessionManager.GetSession(processData) 
                as WorkQueueSessionStore;
            IWorkQueueService wqs = ServiceManager.Services.GetService(typeof(IWorkQueueService)) 
                as IWorkQueueService;
            DataSet copy = processData.DataTable.DataSet.Clone();
            foreach (object id in processData.SelectedItems)
            {
                DataRow row = processData.DataTable.Rows.Find(id);
                if (row != null)
                {
                    copy.Tables[processData.DataTable.TableName].LoadDataRow(row.ItemArray, true);
                }
            }
            DataTable selectedRows = copy.Tables[processData.DataTable.TableName];
            DataSet command = DataService.LoadData(
                new Guid("1d33b667-ca76-4aaa-a47d-0e404ed6f8a6"), 
                new Guid("6eefc3cf-6b6e-4d40-81f7-5c37a81e8a01"), 
                Guid.Empty, Guid.Empty, null, "WorkQueueCommand_parId", 
                processData.ActionId);
            if (command.Tables["WorkQueueCommand"].Rows.Count == 0)
            {
                throw new Exception(Resources.ErrorWorkQueueCommandNotFound);
            }
            DataRow cmdRow = command.Tables["WorkQueueCommand"].Rows[0];
            // work queue command
            if ((Guid)cmdRow["refWorkQueueCommandTypeId"] == (Guid)processData.ParameterService
                .GetParameterValue("WorkQueueCommandType_WorkQueueClassCommand"))
            {
                if (processData.Action == null
                    || processData.Action.Mode != PanelActionMode.Always)
                {
                    CheckSelectedRowsCountPositive(processData.SelectedItems.Count);
                }
                WorkQueueWorkflowCommand cmd = wqss.WQClass.GetCommand((string)cmdRow["Command"]);
                // We handle the UI actions, work queue service will handle all the other background actions
                if (cmd.ActionType == PanelActionType.OpenForm)
                {
                    foreach (WorkQueueWorkflowCommandParameterMapping pm in cmd.ParameterMappings)
                    {
                        object val;
                        switch (pm.Value)
                        {
                            case WorkQueueCommandParameterMappingType.QueueEntries:
                                val = DataDocumentFactory.New(selectedRows.DataSet);
                                break;
                            case WorkQueueCommandParameterMappingType.Parameter1:
                                val = cmdRow.IsNull("Param1") ? null : cmdRow["Param1"];
                                break;
                            case WorkQueueCommandParameterMappingType.Parameter2:
                                val = cmdRow.IsNull("Param2") ? null : cmdRow["Param2"];
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("Value", pm.Value, Resources.ErrorUnknownWorkQueueCommandValueType);
                        }
                        processData.Parameters.Add(pm.Name, val);
                    }
                    // resend the execute - now with the actual action and with queue-command parameters
                    resultList.AddRange(ExecuteAction(processData.SessionFormIdentifier, 
                        processData.RequestingGrid, processData.Entity, cmd.ActionType.ToString(), 
                        cmd.Id.ToString(), new Hashtable(), 
                        processData.SelectedItems, processData.Parameters));
                    return;
                }
            }
            // otherwise we ask the work queue service to process the command
            wqs.HandleAction(new Guid(wqss.Request.ObjectId), wqss.WQClass.Name,
                selectedRows,
                (Guid)cmdRow["refWorkQueueCommandTypeId"],
                cmdRow.IsNull("Command") ? null : (string)cmdRow["Command"],
                cmdRow.IsNull("Param1") ? null : (string)cmdRow["Param1"],
                cmdRow.IsNull("Param2") ? null : (string)cmdRow["Param2"],
                cmdRow.IsNull("refErrorWorkQueueId") ? null : cmdRow["refErrorWorkQueueId"]);
            resultList.Add(new PanelActionResult(ActionResultType.RefreshData));
        }

        private void ExecuteReportAction(ExecuteActionProcessData processData)
        {
            if (processData.Action == null
                || processData.Action.Mode != PanelActionMode.Always)
            {
                CheckSelectedRowsCountPositive(processData.SelectedItems.Count);
                if (processData.SelectedItems.Count > 1)
                {
                    throw new Exception(Resources.ErrorChangeUIMultipleRecords);
                }
            }
            EntityReportAction reportAction = processData.Action as EntityReportAction;
            PanelActionResult result = new PanelActionResult(ActionResultType.OpenUrl);
            result.Url = reportManager.GetReportStandalone(reportAction.ReportId.ToString(), 
                processData.Parameters,
				reportAction.ExportFormatType);
            WebReport wr = reportAction.Report as WebReport;
            if (wr != null)
            {
                result.UrlOpenMethod = wr.OpenMethod.ToString();
            }
            result.Request = new UIRequest();
            result.Request.Caption = processData.Action.Caption;
            if (processData.Action.RefreshAfterReturn != ReturnRefreshType.None)
            {
                result.RefreshOnReturnSessionId = processData.SessionFormIdentifier;
                result.RefreshOnReturnType = processData.Action.RefreshAfterReturn.ToString();
            }
            resultList.Add(result);
        }

        private void ExecuteChangeUIAction(ExecuteActionProcessData processData)
        {
            if (processData.Action == null
                || processData.Action.Mode != PanelActionMode.Always)
            {
                CheckSelectedRowsCountPositive(processData.SelectedItems.Count);
                if (processData.SelectedItems.Count > 1)
                {
                    throw new Exception(Resources.ErrorChangeUIMultipleRecords);
                }
            }
            PanelActionResult result = new PanelActionResult(ActionResultType.ChangeUI);
            UIRequest uir = RequestTools.GetActionRequest(processData.Parameters, 
                processData.SelectedItems, processData.Action);
            uir.FormSessionId = processData.SessionFormIdentifier;
            result.UIResult = uiManager.InitUI(
                request: uir,
                registerSession: false,
                addChildSession: true, 
                parentSession: sessionManager.GetSession(processData),
                basicUiService: basicUiService);
            resultList.Add(result);
        }

        protected override void ExecuteOpenFormAction(
            ExecuteActionProcessData processData)
        {
            if (processData.Action == null
                || processData.Action.Mode != PanelActionMode.Always)
            {
                CheckSelectedRowsCountPositive(processData.SelectedItems.Count);
            }
            PanelActionResult result = new PanelActionResult(
                ActionResultType.OpenForm);
            UIRequest uir = RequestTools.GetActionRequest(processData.Parameters, 
                processData.SelectedItems, processData.Action);
            if (processData.Action.RefreshAfterReturn == ReturnRefreshType.MergeModalDialogChanges)
            {
                uir.ParentSessionId = processData.SessionFormIdentifier;
                uir.SourceActionId = processData.Action.Id.ToString();
            }
            result.Request = uir;
            if (processData.Action.RefreshAfterReturn != ReturnRefreshType.None)
            {
                result.RefreshOnReturnSessionId = processData.SessionFormIdentifier;
                result.RefreshOnReturnType = processData.Action.RefreshAfterReturn.ToString();
            }
            resultList.Add(result);
            EntityWorkflowAction ewa = processData.Action as EntityWorkflowAction;
            if (ewa != null && ewa.CloseType != ModalDialogCloseType.None)
            {
                resultList.Add(new PanelActionResult(ActionResultType.DestroyForm));
            }
        }

        private void ExecuteSelectionDialogAction(ExecuteActionProcessData processData)
        {
            if (processData.Action == null || 
                processData.Action.Mode != PanelActionMode.Always)
            {
                CheckSelectedRowsCountPositive(processData.SelectedItems.Count);
            }
            resultList.Add(sessionManager.GetSession(processData).ExecuteAction(
                processData.ActionId));
        }

        protected override void SetTransactionId(
            ExecuteActionProcessData processData,
            string transactionId)
        {
            sessionManager.GetSession(processData).TransationId = transactionId;
        }

        protected override ActionResult MakeActionResult(ActionResultType type)
        {
            return new PanelActionResult(type);
        }
    }
}