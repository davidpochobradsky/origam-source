#region license
/*
Copyright 2005 - 2017 Advantage Solutions, s. r. o.

This file is part of ORIGAM.

ORIGAM is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

ORIGAM is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with ORIGAM.  If not, see<http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Xml;

using Origam.Rule;
using Origam.Schema.EntityModel;
using Origam.DA;
using Origam.DA.Service;
using Origam;
using Origam.Schema.GuiModel;
using Origam.Schema;
using Origam.Schema.RuleModel;
using core = Origam.Workbench.Services.CoreServices;

namespace Origam.Server
{
    public abstract class SessionStore : IDisposable
    {
        internal static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private UIService _service;
        private static Ascii85 _ascii85 = new Ascii85();
        private string _transactionId = null;
        private bool _isDelayedLoading = false;
        private Guid _id;
        private SessionStore _parentSession;
        private Guid _formId;
        private string _name;
        private string _title;
        private DataStructureRuleSet _ruleSet;
        private DataStructureSortSet _sortSet;
        private RuleEngine _ruleEngine;
        private DatasetRuleHandler _ruleHandler;
        private DataSet _data;
        private DataSet _dataList;
        private string _dataListEntity;
        private Guid _dataListDataStructureEntityId;
        private IList<string> _dataListLoadedColumns = new List<string>();
        private XmlDataDocument _xmlData;
        private DateTime _cacheExpiration;
        private UIRequest _request;
        private IList<SessionStore> _childSessions = new List<SessionStore>();
        private SessionStore _activeSession;
        private IList<FormNotification> _notifications = new List<FormNotification>();
        private bool _refreshOnInitUI;
        private SaveRefreshType _refreshAfterSaveType;
        internal object _lock = new object();
        private int _registerEventsCounter = 0;
        private object _currentRecordId = null;
        private bool _isPagedLoading = false;
        private Dictionary<string, bool> _entityHasRuleDependencies = new Dictionary<string, bool>();
        private IList<string> _dirtyEnabledEntities = new List<string>();
        private bool _isModalDialog = false;
        private ArrayList _pendingChanges = null;
        private bool _isModalDialogCommited = false;
        private IEndRule _confirmationRule = null;
        private IDictionary<string, IDictionary> _variables = new Dictionary<string, IDictionary>();
        private bool _supressSave = false;
        private bool _refreshPortalAfterSave = false;
        private bool _isExclusive = false;

        public const string LIST_LOADED_COLUMN_NAME = "___ORIGAM_IsLoaded";
        public const string ACTION_SAVE = "SAVE";
        public const string ACTION_REFRESH = "REFRESH";
        public const string ACTION_NEXT = "NEXT";
        public const string ACTION_QUERYNEXT = "QUERYNEXT";
        public const string ACTION_ABORT = "ABORT";
        public const string ACTION_REPEAT = "REPEAT";

        public SessionStore(UIService service, UIRequest request, string name)
        {
            this.Name = name;
            this.Service = service;
            if (request.FormSessionId == null)
            {
                this.Id = Guid.NewGuid();
            }
            else
            {
                this.Id = new Guid(request.FormSessionId);
            }
            this.Request = request;
            this.IsModalDialog = request.IsModalDialog;
            _ruleHandler = new DatasetRuleHandler();
            _ruleEngine = new RuleEngine(null, null);
            this.CacheExpiration = DateTime.Now.AddMinutes(5);
        }

        public string TransationId
        {
            get { return _transactionId; }
            set { 
                _transactionId = value;

                if (this.RuleEngine != null)
                {
                    this.RuleEngine.TransactionId = _transactionId;
                }
            }
        }

        public UIService Service
        {
            get { return _service; }
            set { _service = value; }
        }

        public SessionStore ParentSession
        {
            get { return _parentSession; }
            set { _parentSession = value; }
        }

        public ArrayList PendingChanges
        {
            get { return _pendingChanges; }
            set { _pendingChanges = value; }
        }

        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual string Title
        {
            get 
            {
                return _title ?? this.Request.Caption;
            }
            set 
            {
                _title = value; 
            }
        }

        public object CurrentRecordId
        {
            get { return _currentRecordId; }
            set { _currentRecordId = value; }
        }

        public bool IsPagedLoading
        {
            get { return _isPagedLoading; }
            set { _isPagedLoading = value; }
        }

        public SessionStore ActiveSession
        {
            get { return _activeSession; }
            set { _activeSession = value; }
        }

        public RuleEngine RuleEngine
        {
            get { return _ruleEngine; }
        }

        public DatasetRuleHandler RuleHandler
        {
            get { return _ruleHandler; }
        }

        public DataSet Data
        {
            get { return _data; }
        }

        public DataSet DataList
        {
            get { return _dataList; }
        }

        public string DataListEntity
        {
            get { return _dataListEntity; }
        }

        public Guid DataListDataStructureEntityId
        {
            get { return _dataListDataStructureEntityId; }
        }

        public IList<string> DataListLoadedColumns
        {
            get { return _dataListLoadedColumns; }
        }

        public DataSet InitialData
        {
            get
            {
                return DataList == null ? Data : DataList;
            }
        }

        public UIRequest Request
        {
            get { return _request; }
            set { _request = value; }
        }

        public XmlDataDocument XmlData
        {
            get { return _xmlData; }
        }

        public DataStructureRuleSet RuleSet
        {
            get { return _ruleSet; }
            set 
            {
                _ruleSet = value;

                InitEntityDependencies();
            }
        }

        public bool HasRules
        {
            get
            {
                // has ruleset
                if(this.RuleSet != null) return true;

                // has some lookup fields that are processed (looked up on changes) 
                // by the rule engine
                if (this.Data != null)
                {
                    foreach (DataTable table in this.Data.Tables)
                    {
                        foreach (DataColumn column in table.Columns)
                        {
                            if (column.ExtendedProperties.Contains(Const.OriginalFieldId))
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
        }

        public DataStructureSortSet SortSet
        {
            get { return _sortSet; }
            set { _sortSet = value; }
        }

        public DateTime CacheExpiration
        {
            get { return _cacheExpiration; }
            set { _cacheExpiration = value; }
        }

        public Guid FormId
        {
            get { return _formId; }
            set { _formId = value; }
        }

        public IList<FormNotification> Notifications
        {
            get { return _notifications; }
            set { _notifications = value; }
        }

        public bool RefreshOnInitUI
        {
            get { return _refreshOnInitUI; }
            set { _refreshOnInitUI = value; }
        }

        public SaveRefreshType RefreshAfterSaveType
        {
            get { return _refreshAfterSaveType; }
            set { _refreshAfterSaveType = value; }
        }

        public bool RefreshPortalAfterSave
        {
            get { return _refreshPortalAfterSave; }
            set { _refreshPortalAfterSave = value; }
        }

        public IList<SessionStore> ChildSessions
        {
            get { return _childSessions; }
        }

        public bool IsDelayedLoading
        {
            get { return _isDelayedLoading; }
            set { _isDelayedLoading = value; }
        }

        public bool IsModalDialog
        {
            get { return _isModalDialog; }
            set { _isModalDialog = value; }
        }

        public bool IsModalDialogCommited
        {
            get { return _isModalDialogCommited; }
            set { _isModalDialogCommited = value; }
        }

        public bool SuppressSave
        {
            get { return _supressSave; }
            set { _supressSave = value; }
        }

        public IEndRule ConfirmationRule
        {
            get { return _confirmationRule; }
            set { _confirmationRule = value; }
        }

        public IDictionary<string, IDictionary> Variables
        {
            get
            {
                return _variables;
            }
        }

        public IList<string> DirtyEnabledEntities
        {
            get
            {
                return _dirtyEnabledEntities;
            }
        }

        public virtual bool SupportsFormXmlAsync
        { 
            get
            {
                return false;
            }
        }

        public bool IsExclusive
        {
            get
            {
                return _isExclusive;
            }
            set
            {
                _isExclusive = value;
            }
        }

        public virtual string HelpTooltipFormId
        {
            get
            {
                return FormId.ToString();
            }
        }

        public void AddChildSession(SessionStore ss)
        {
            this.ChildSessions.Add(ss);
            ss.ParentSession = this;
        }
	
        public void Clear()
        {
            lock (_lock)
            {
                UnregisterEvents();
                _xmlData = null;
                _data = null;
                RegisterEvents();
            }
        }

        public void SetDataList(DataSet list, string entity, DataStructure listDataStructure)
        {
            _dataList = list;

            if (this.DataList != null)
            {
                _dataListEntity = entity;
                foreach (DataStructureEntity e in listDataStructure.Entities)
                {
                    if (e.Name == entity)
                    {
                        _dataListDataStructureEntityId = e.Id;
                                break;
                            }
                        }
                RemoveNullConstraints(this.DataList);
            }
        }

        public abstract bool HasChanges();

        public void SetDataSource(object dataSource)
        {
            // finish with the old data
            UnregisterEvents();

            try
            {
                // set the new data
                if (dataSource is DataSet)
                {
                    _data = dataSource as DataSet;

                    bool selfJoinExists = false;
                    foreach (DataRelation r in _data.Relations)
                    {
                        if (r.ParentTable.Equals(r.ChildTable))
                        {
                            selfJoinExists = true;
                            break;
                        }
                    }

                    // no XML for self joins (incompatible with XmlDataDocument)
                    if (!selfJoinExists)
                    {
                        _xmlData = new XmlDataDocument(_data);
                    }
                }
                else if (dataSource is XmlDataDocument)
                {
                    _xmlData = dataSource as XmlDataDocument;
                    _data = _xmlData.DataSet;
                }
                else if (dataSource == null)
                {
                    _xmlData = null;
                    _data = null;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("dataSource", dataSource, "Invalid session data format.");
                }
            }
            finally
            {
                // wire the new data's events
                RegisterEvents();
            }

            if (this.Data != null)
            {
                RemoveNullConstraints(this.Data);
                DatasetGenerator.ApplyDynamicDefaults(this.Data, this.Request.Parameters);

                InitEntityDependencies();
            }
        }

        private void InitEntityDependencies()
        {
            _entityHasRuleDependencies.Clear();

            if (this.Data != null)
            {
                foreach (DataTable table in this.Data.Tables)
                {
                    bool hasDependencies = HasColumnDependencies(table);

                    if (!hasDependencies)
                    {
                        hasDependencies = HasRuleDependencies(table);
                    }

                    _entityHasRuleDependencies[table.TableName] = hasDependencies;
                }
            }
        }

        private bool HasRuleDependencies(DataTable table)
        {
            bool result = false;

            // rule dependencies
            if (this.RuleSet != null)
            {
                foreach (DataStructureRule rule in this.RuleSet.Rules())
                {
                    if (rule.Entity.Name != table.TableName)
                    {
                        int count = 0;
                        foreach (DataStructureRuleDependency dependency in rule.RuleDependencies)
                        {
                            count++;
                            if (dependency.Entity.Name == table.TableName)
                            {
                                result = true;
                                break;
                            }
                        }

                        if (result) break;

                        // if there are no dependencies, this entity refreshes on any updates,
                        // that means also on updates from our table
                        result = (count == 0);
                    }
                }

            }

            return result;
        }

        private bool HasColumnDependencies(DataTable table)
        {
            string childRelationExpression = "CHILD(" + table.TableName.ToUpper() + ")";
            bool result = false;

            // check column expression dependencies
            foreach (DataTable otherTable in this.Data.Tables)
            {
                foreach (DataColumn col in otherTable.Columns)
                {
                    if (col.Expression != "" && col.Expression != null
                        && (
                            col.Expression.ToUpper().Contains(childRelationExpression)
                            || 
                                (
                                    col.Expression.Contains("Parent.")
                                )
                            )
                        )
                    {
                        result = true;
                        break;
                    }
                }

                if (result) break;
            }

            return result;
        }

        public void RegisterEvents()
        {
            _registerEventsCounter--;

            if (_xmlData != null)
            {
                if (_registerEventsCounter == 0)
                {
                    _ruleHandler.RegisterDatasetEvents(_xmlData, _ruleSet, _ruleEngine);
                }
            }
        }

        public void UnregisterEvents()
        {
            _registerEventsCounter++;

            if (_xmlData != null)
            {
                _ruleHandler.UnregisterDatasetEvents(_xmlData);
            }
        }

        public virtual IList RestoreData(object parentId)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadColumns(IList<string> columns)
        {

        }

        public abstract void Init();
        public abstract object ExecuteAction(string actionId);
        public abstract XmlDocument GetFormXml();

        public virtual void PrepareFormXml()
        {
            throw new NotSupportedException();
        }

        #region IDisposable Members

        public void Dispose()
        {
            Analytics.SetProperty("OrigamFormId", this.FormId);
            Analytics.SetProperty("OrigamFormName", this.Name);
            Analytics.Log("UI_CLOSEFORM");

            if (this.ParentSession != null)
            {
                this.ParentSession.ChildSessions.Remove(this);
            }

            foreach (SessionStore child in this.ChildSessions)
            {
                child.ParentSession = null;
                child.Dispose();
            }

            OnDispose();

            this.Clear();
            _ruleHandler = null;
            _ruleSet = null;
            _ruleEngine = null;
            _service = null;
            _parentSession = null;
            _activeSession = null;
        }

        public virtual void OnDispose()
        {
        }

        #endregion

        #region Private Methods
        private static void RemoveNullConstraints(DataSet dataset)
        {
            foreach (DataTable table in dataset.Tables)
            {
                foreach (DataColumn col in table.Columns)
                {
                    if (col.AllowDBNull == false & IsKey(col) == false)
                    {
                        col.AllowDBNull = true;
                    }
                }
            }
        }

        private static bool IsKey(DataColumn column)
        {
            // primary key
            bool found = IsInColumns(column, column.Table.PrimaryKey);
            if (found) return true;

            // parent relations
            found = IsInRelations(column, column.Table.ParentRelations);
            if (found) return true;

            // child relations
            return IsInRelations(column, column.Table.ChildRelations);
        }

        private static bool IsInRelations(DataColumn column, DataRelationCollection relations)
        {
            foreach (DataRelation relation in relations)
            {
                if (IsRelationKey(column, relation)) return true;
            }

            return false;
        }

        private static bool IsRelationKey(DataColumn column, DataRelation relation)
        {
            // parent columns
            bool found = IsInColumns(column, relation.ParentColumns);

            if (found) return true;

            // child columns
            return IsInColumns(column, relation.ChildColumns);
        }

        private static bool IsInColumns(DataColumn searchedColumn, DataColumn[] columns)
        {
            foreach (DataColumn col in columns)
            {
                if (col.Equals(searchedColumn)) return true;
            }

            return false;
        }

        internal ArrayList GetChangesByRow(string requestingGrid, DataRow row, int operation, bool hasErrors, bool hasChanges)
        {
            return GetChangesByRow(requestingGrid, row, operation, null, true, hasErrors, hasChanges);
        }

        internal ArrayList GetChangesByRow(string requestingGrid, DataRow row, int operation, Hashtable ignoreKeys, bool includeRowStates, bool hasErrors, bool hasChanges)
        {
            ArrayList listOfChanges = new ArrayList();
            DataRow rootRow = DatasetTools.RootRow(row);
            DatasetTools.CheckRowErrorRecursive(rootRow, null, false);

            // when there is an error, copy it to the list entity, too
            if (this.DataList != null)
            {
                DataRow listRow = GetListRow(this.DataListEntity, DatasetTools.PrimaryKey(rootRow)[0]);
                CloneErrors(rootRow, listRow);
            }

            if (_entityHasRuleDependencies[row.Table.TableName] || operation == 4 || hasErrors)
            {
                // entity has some dependencies (e.g. calculated columns in other tables)
                // so we return also the parents and children of this row
                GetChangesRecursive(listOfChanges, requestingGrid, rootRow, operation, row, true, ignoreKeys, includeRowStates);
            }
            else
            {
                // this entity has no dependencies in other tables, we only
                // return data from this row
                ChangeInfo ci = GetChangeInfo(requestingGrid, row, operation, includeRowStates);
                listOfChanges.Add(ci);
            }

            try
            {
                this.UnregisterEvents();
                if (this.SuppressSave && hasChanges)
                {
                    // set "saved" flag also if the form is read only (it might actually change data
                    // e.g. for virtual fields, these are not read only even in read only screens)
                    // but we still don't want a dirty flag because the user would be asked to save
                    // data when closing the screen
                    this.Data.AcceptChanges();
                }
                else
                {
                    // If the updates did not cause any changes, e.g. because only non-dirty-enabled
                    // entity data were changed, we send an info to reset the dirty flag.
                    // Non-dirty enabled entities are those that get not saved, e.g. entities in a workflow
                    // session store that are not in the save-data structure for the workflow form.
                    foreach (DataTable table in this.Data.Tables)
                    {
                        if (!this.DirtyEnabledEntities.Contains(table.TableName))
                        {
                            table.AcceptChanges();
                        }
                    }
                }
            }
            finally
            {
                this.RegisterEvents();
            }

            if (! hasChanges)
            {
                listOfChanges.Add(ChangeInfo.SavedChangeInfo());
            }

            return listOfChanges;
        }

        private static void CloneErrors(DataRow sourceRow, DataRow destinationRow)
        {
            destinationRow.ClearErrors();

            if (sourceRow.HasErrors)
            {
                destinationRow.RowError = destinationRow.RowError;
                foreach (DataColumn col in sourceRow.GetColumnsInError())
                {
                    destinationRow.SetColumnError(col.ColumnName, sourceRow.GetColumnError(col));
                }
            }
        }

        public ArrayList GetChanges(string entity, object id, int operation, bool hasErrors, bool hasChanges)
        {
            return GetChangesByRow(null, this.GetSessionRow(entity, id), operation, hasErrors, hasChanges);
        }

        public ArrayList GetChanges(string entity, object id, int operation, Hashtable ignoreKeys, bool includeRowStates, bool hasErrors, bool hasChanges)
        {
            return GetChangesByRow(null, this.GetSessionRow(entity, id), operation, ignoreKeys, includeRowStates, hasErrors, hasChanges);
        }

        private void GetChangesRecursive(ArrayList changes, string requestingGrid, DataRow row, int operation, DataRow changedRow, bool allDetails, Hashtable ignoreKeys, bool includeRowStates)
        {
            if (row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Detached)
            {
                object rowKey = DatasetTools.PrimaryKey(row)[0];
                string ignoreRowIndex = row.Table.TableName + rowKey.ToString();
                if (row.Equals(changedRow))
                {
                    ChangeInfo ci = GetChangeInfo(requestingGrid, row, operation, includeRowStates);
                    changes.Add(ci);
                }
                else if (ignoreKeys == null || ! ignoreKeys.Contains(ignoreRowIndex))
                {
                    // check if this is a child of the copied row
                    bool isParentRow = ! IsChildRow(row, changedRow);

                    // always parent rows because calculated fields do not change the RowState
                    if (allDetails || isParentRow || row.RowState != DataRowState.Unchanged || row.HasErrors)
                    {
                        int op = operation;

                        if (op == 4)
                        {
                            // this is a parent row of the copied row, we set the status Update
                            if (isParentRow) op = 0;
                        }
                        else
                        {
                            // no copy (in that case we leave copy status), then this
                            // is update, because it is not the actual changed row
                            op = 0;
                        }

                        ChangeInfo ci = GetChangeInfo(null, row, op, includeRowStates);
                        changes.Add(ci);
                        // we processed it once so we do not want to get it again in a next iteration
                        if (ignoreKeys != null)
                        {
                            ignoreKeys.Add(ignoreRowIndex, null);
                        }
                    }
                }

                foreach (DataRelation childRelation in row.Table.ChildRelations)
                {
                    foreach (DataRow childRow in row.GetChildRows(childRelation))
                    {
                        // check recursion
                        foreach (DataRelation parentRelation in row.Table.ParentRelations)
                        {
                            foreach (DataRow parentRow in row.GetParentRows(parentRelation))
                            {
                                if (parentRow.Equals(childRow))
                                {
                                    // Recursion found - this row has been checked already.
                                    return;
                                }
                            }
                        }

                        GetChangesRecursive(changes, requestingGrid, childRow, operation, changedRow, allDetails, ignoreKeys, includeRowStates);

                    }
                }
            }
        }

        private static bool IsChildRow(DataRow row, DataRow changedRow)
        {
            bool found = false;
            DataRow parentRow = row;
            while (parentRow != null && parentRow.Table.ParentRelations.Count > 0)
            {
                parentRow = parentRow.GetParentRow(parentRow.Table.ParentRelations[0]);

                if (changedRow.Equals(parentRow))
                {
                    // One of the parent rows is the copied row, 
                    // so this one is copied as well, so we leave the copy status.
                    found = true;
                    break;
                }
            }
            return found;
        }

        internal ChangeInfo GetChangeInfo(string requestingGrid, DataRow row, int operation)
        {
            return GetChangeInfo(requestingGrid, row, operation, true);
        }

        internal ChangeInfo GetChangeInfo(string requestingGrid, DataRow row, int operation, bool includeRowState)
        {
            ChangeInfo ci = new ChangeInfo();
            ci.Entity = row.Table.TableName;
            ci.Operation = (operation == 4 ? 1 : operation);        // 4 = copy = create
            ci.RequestingGrid = requestingGrid;
            ci.ObjectId = row[row.Table.PrimaryKey[0]];
            // for create-update we return the updated state (read-only + colors)
            if (operation >= 0)
            {
                string[] columns = GetColumnNames(row.Table);
                ci.WrappedObject = GetRowData(row, columns);
                if (includeRowState)
                {
                    ci.State = RowStates(ci.Entity, new object[] { ci.ObjectId })[0] as RowSecurityState;
                }
            }
            return ci;
        }

        internal ChangeInfo GetDeletedInfo(string requestingGrid, string tableName, object objectId)
        {
            ChangeInfo ci = new ChangeInfo();
            ci.Entity = tableName;
            ci.Operation = -1;
            ci.RequestingGrid = requestingGrid;
            ci.ObjectId = objectId;

            return ci;
        }

        internal static string ConvertTextToUnixStyle(string text)
        {
            return text.Replace("\r\n", "\r");
        }

        internal static ArrayList GetRowData(DataRow row, string[] columns)
        {
            return GetRowData(row, columns, true);
        }

        internal static ArrayList GetRowData(DataRow row, string[] columns, bool withErrors)
        {
            ArrayList result = new ArrayList(columns.Length);
            foreach (string col in columns)
            {
                if (col != LIST_LOADED_COLUMN_NAME)
                {
                    object value = null;
                    DataColumn dataColumn = row.Table.Columns[col];

					if (IsWriteOnly(dataColumn))
					{
						value = null;
					}
					else
					{
						if (IsColumnArray(dataColumn))
						{
							value = GetRowColumnArrayValue(row, dataColumn);
						}
						else
						{
							value = GetRowColumnValue(row, dataColumn);
						}
					}
                    result.Add(value);
                }
            }
            if (withErrors)
            {
                result.Add(GetRowErrors(row));
            }
            return result;
        }

        public static bool IsColumnArray(DataColumn dataColumn)
        {
            if (dataColumn.ExtendedProperties.Contains(Const.OrigamDataType))
            {
                return ((OrigamDataType)dataColumn.ExtendedProperties[Const.OrigamDataType]) == OrigamDataType.Array;
            }
            else
            {
                return false;
            }
        }

		public static bool IsWriteOnly(DataColumn dataColumn)
		{
			if (dataColumn.ExtendedProperties.Contains(Const.IsWriteOnlyAttribute))
			{
				return ((bool)dataColumn.ExtendedProperties[Const.IsWriteOnlyAttribute]) == true;
			}
			else
			{
				return false;
			}
		}

		private static object GetRowErrors(DataRow row)
        {
            object value = null;

            if (row.HasErrors)
            {
                ErrorList errorList = new ErrorList();
                errorList.RowError = row.RowError;

                foreach (DataColumn col in row.GetColumnsInError())
                {
                    errorList.FieldErrors.Add(col.Ordinal, row.GetColumnError(col));
                }

                value = errorList;
            }
            return value;
        }

        public static ArrayList GetRowColumnArrayValue(DataRow row, DataColumn dataColumn)
        {
            string relatedTableName = (string)dataColumn.ExtendedProperties[Const.ArrayRelation];
            string relatedColumnName = (string)dataColumn.ExtendedProperties[Const.ArrayRelationField];
            DataTable relatedTable = row.Table.DataSet.Tables[relatedTableName];

            DataRow[] childRows = row.GetChildRows(relatedTableName);
            ArrayList list = new ArrayList(childRows.Length);

            foreach (DataRow childRow in childRows)
            {
                list.Add(childRow[relatedColumnName]);
            }

            return list;
        }

        private static object GetRowColumnValue(DataRow row, DataColumn col)
        {
            object value = null;

            object o = row[col];
            string text = o as string;

            if (text != null)
            {
                if (text != "")
                {
                    value = ConvertTextToUnixStyle(text);
                }
            }
            //else if (o is Guid)
            //{
            //    value = ShortGuid(guid.Value);
            //    value = new FlashGuid((Guid)o);
            //}
            else
            {
                value = o;
            }
            return value;
        }

        internal DataTable GetSessionEntity(string entity)
        {
            DataSet sourceData = (this.Data == null ? this.DataList : this.Data);

            return GetTable(entity, sourceData);
        }

        internal DataTable GetTable(string entity, DataSet data)
        {
            if (!data.Tables.Contains(entity))
            {
                throw new Exception("Entity not found: " + entity);
            }

            return data.Tables[entity];
        }

        internal static object ShortGuid(Guid guid)
        {
            return _ascii85.Encode(guid.ToByteArray());
        }

        internal DataRow GetSessionRow(string entity, object id)
        {
            if (id == null)
            {
                throw new NullReferenceException("Cannot find row. Id is null");
            }
            DataRow row = null;

            // first we try to find the row in the data
            if (this.Data != null && this.Data.Tables.Contains(entity))
            {
                row = this.Data.Tables[entity].Rows.Find(id);
            }
            // not found, we try to find it in the list
            if (this.DataList != null && row == null)
            {
                row = GetListRow(entity, id);
            }
            if (row == null)
            {
                throw new ArgumentOutOfRangeException("id", id, Properties.Resources.ErrorRecordNotFound);
            }
            return row;
        }

        internal DataRow GetListRow(string entity, object id)
        {
            DataTable table = GetTable(entity, this.DataList);
            return table.Rows.Find(id);
        }

        internal void LazyLoadListRowData(object rowId, DataRow row)
        {
            lock (_lock)
            {
                if (!(bool)row[SessionStore.LIST_LOADED_COLUMN_NAME])
                {
                    // the row has not been loaded from the database yet, we load the
                    // whole row even though only some of the columns are needed because
                    // e.g. color or row-level security rules must be evaluated on all the
                    // columns
                    QueryParameterCollection pms = new QueryParameterCollection();
                    foreach (DataColumn col in row.Table.PrimaryKey)
                    {
                        pms.Add(new QueryParameter(col.ColumnName, rowId));
                    }
                    DataSet loadedRow = DatasetTools.CloneDataSet(row.Table.DataSet);
                    core.DataService.LoadRow(DataListDataStructureEntityId, pms, loadedRow, null);
                    if (loadedRow.Tables[row.Table.TableName].Rows.Count == 0)
                    {
                        throw new ArgumentOutOfRangeException(string.Format(
                            "Row {0} not found in {1}.", rowId, row.Table.TableName));
                    }
                    SessionStore.MergeRow(loadedRow.Tables[row.Table.TableName].Rows[0], row);
                    row[SessionStore.LIST_LOADED_COLUMN_NAME] = true;
                    row.AcceptChanges();
                }
            }
        }

        public ArrayList RowStates(string entity, object[] ids)
        {
            ArrayList result = new ArrayList();
            foreach (object id in ids)
            {
                if (id != null)
                {
                    object profileId = SecurityTools.CurrentUserProfile().Id;
                    DataRow row;
                    try
                    {
                        row = GetSessionRow(entity, id);
                    }
                    catch
                    {
                        // in case the id is not contained in the datasource anymore (e.g. form unloaded or new data piece loaded)
                        return new ArrayList();
                    }
                    lock (_lock)    // no update should be done in the meantime when rules are not handled
                    {
                        if (IsLazyLoadedRow(row))
                        {
                            // load lazily loaded rows in case they have not been loaded
                            // before calling for row-states
                            LazyLoadListRowData(id, row);
                        }
                        // we have to unregister dataset event handling, because row level security state will try to add a new row/delete it to check parent/child state
                        this.UnregisterEvents();
                        try
                        {
                            result.Add(this.RuleEngine.RowLevelSecurityState(row, profileId));
                        }
                        finally
                        {
                            this.RegisterEvents();
                        }
                    }
                }
            }
            return result;
        }

		public bool IsLazyLoadedRow(DataRow row)
		{
			return DataList != null && row.Table.DataSet == DataList;
		}

        public bool IsLazyLoadedEntity(string entity)
        {
            return DataListEntity != null && entity == DataListEntity;
        }
        
        #region CUD
		public virtual ArrayList CreateObject(string entity, IDictionary<string, object> values,
            IDictionary<string, object> parameters, string requestingGrid)
        {
            lock (_lock)
            {
                DataTable table = GetTable(entity, this.Data);
                UserProfile profile = SecurityTools.CurrentUserProfile();

                DataRow newRow;

                if (parameters.Count == 0)
                {
                    newRow = DatasetTools.CreateRow(null, table, null, profile.Id);
                }
                else
                {
                    object[] keys = new object[parameters.Count];
                    parameters.Values.CopyTo(keys, 0);

                    DataRelation relation = table.ParentRelations[0];
                    DataColumn parentKeyColumn = relation.ParentColumns[0];
                    DataRow parentRow = null;

                    if (parameters.Count == 1 && parentKeyColumn.Equals(relation.ParentTable.PrimaryKey[0]))
                    {
                        // if parent column is the primary key, then we just simply lookup the row by its primary key
                        parentRow = relation.ParentTable.Rows.Find(keys);
                    }
                    else
                    {
                        // if not, we have to construct a search
                        StringBuilder searchBuilder = new StringBuilder();

                        foreach (KeyValuePair<string, object> entry in parameters)
                        {
                            for (int i = 0; i < relation.ChildColumns.Length; i++)
                            {
                                if (relation.ChildColumns[i].ColumnName == entry.Key)
                                {
                                    parentKeyColumn = relation.ParentColumns[i];
                                }
                            }

                            if (parentKeyColumn == null)
                            {
                                throw new ArgumentOutOfRangeException("key", entry.Key, "Key not found in the parent table by the provided child key.");
                            }

                            string value = entry.Value.ToString();
                            if (parentKeyColumn.DataType == typeof(Guid) || parentKeyColumn.DataType == typeof(string))
                            {
                                value = DatasetTools.TextExpression(value);
                            }
                            else if (parentKeyColumn.DataType == typeof(DateTime))
                            {
                                value = DatasetTools.DateExpression(entry.Value);
                            }

                            if (searchBuilder.Length > 0)
                            {
                                searchBuilder.Append(" AND ");
                            }

                            searchBuilder.Append(parentKeyColumn.ColumnName);
                            searchBuilder.Append(" = ");
                            searchBuilder.Append(value);
                        }

                        DataRow[] rows = relation.ParentTable.Select(searchBuilder.ToString());
                        if (rows.Length == 1)
                        {
                            parentRow = rows[0];
                        }
                    }

                    newRow = DatasetTools.CreateRow(parentRow, table, relation, profile.Id);
                }

                // set any values passed by the client (e.g. when adding an entry into a calendar,
                // resource and date are known so they are handed over directly when adding a record
                if (values != null)
                {
                    foreach (KeyValuePair<string, object> entry in values)
                    {
                        newRow[entry.Key] = entry.Value;
                    }
                }

                table.Rows.Add(newRow);
                if (!this.RuleEngine.RowLevelSecurityState(newRow, profile.Id).AllowCreate)
                {
                    table.Rows.Remove(newRow);
                    throw new Exception(Properties.Resources.ErrorCreateRecordNotAllowed);
                }

                NewRowToDataList(newRow);

                ArrayList listOfChanges = GetChangesByRow(requestingGrid, newRow, 1, this.Data.HasErrors, this.Data.HasChanges());

                return listOfChanges;
            }
        }

        public virtual ArrayList UpdateObject(string entity, object id, string property, object newValue)
        {
            lock (_lock)
            {
                UserProfile profile = SecurityTools.CurrentUserProfile();
                DataRow row = GetSessionRow(entity, id);
                DataColumn dataColumn = row.Table.Columns[property];
                if (dataColumn == null)
                {
                    throw new NullReferenceException(
                        String.Format(Properties.Resources.ErrorColumnNotFound, property));
                }
                if (IsColumnArray(dataColumn))
                {
                    UpdateRowColumnArray(newValue, profile, row, dataColumn);
                }
                else
                {
                    UpdateRowColumn(property, newValue, profile, row);
                }
                ArrayList listOfChanges = GetChangesByRow(null, row, 0, this.Data.HasErrors, this.Data.HasChanges());
                if (! this.Data.HasChanges())
                {
                    listOfChanges.Add(ChangeInfo.SavedChangeInfo());                
                }
                return listOfChanges;
            }
        }

        private static void UpdateRowColumnArray(object newValue, UserProfile profile, DataRow row, DataColumn dataColumn)
        {
            string relatedTableName = (string)dataColumn.ExtendedProperties[Const.ArrayRelation];
            string relatedColumnName = (string)dataColumn.ExtendedProperties[Const.ArrayRelationField];
            DataTable relatedTable = row.Table.DataSet.Tables[relatedTableName];

            DataRow[] childRows = row.GetChildRows(relatedTableName);

            Array newArray = (Array)newValue;
            // handle null value (sent e.g. when updating dependent fields)
            // null = empty array
            if (newArray == null)
            {
                newArray = new object[] { };
            }
            ArrayList rowsToDelete = new ArrayList();
            ArrayList valuesToAdd = new ArrayList();

            // values to add
            foreach (object arrayValue in newArray)
            {
                bool found = false;
                foreach (DataRow childRow in childRows)
                {
                    if (childRow[relatedColumnName] == arrayValue)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    valuesToAdd.Add(arrayValue);
                }
            }

            // values to delete
            foreach (DataRow childRow in childRows)
            {
                bool found = false;
                {
                    foreach (object arrayValue in newArray)
                        if (childRow[relatedColumnName] == arrayValue)
                        {
                            found = true;
                            break;
                        }
                }
                if (!found)
                {
                    rowsToDelete.Add(childRow);
                }
            }

            foreach (DataRow rowToDelete in rowsToDelete)
            {
                rowToDelete.Delete();
            }

            foreach (object valueToAdd in valuesToAdd)
            {
                DataRow newRow = DatasetTools.CreateRow(row, relatedTable, row.Table.ChildRelations[relatedTableName], profile.Id);
                UpdateRowValue(relatedColumnName, valueToAdd, newRow);
                relatedTable.Rows.Add(newRow);
            }
        }

        private void UpdateRowColumn(string property, object newValue, UserProfile profile, DataRow row)
        {
            UpdateRowValue(property, newValue, row);

            DatasetTools.UpdateOrigamSystemColumns(row, row.RowState == DataRowState.Added, profile.Id);

            // update the data list
            if (this.DataList != null)
            {
                DataRow dataRow = row;

                while (dataRow != null && dataRow.Table.TableName != this.DataListEntity)
                {
                    if (dataRow.Table.ParentRelations.Count > 0)
                    {
                        dataRow = dataRow.GetParentRow(dataRow.Table.ParentRelations[0]);
                    }
                    else
                    {
                        dataRow = null;
                    }
                }

                object[] pk = DatasetTools.PrimaryKey(dataRow);
                DataRow listRow = this.DataList.Tables[this.DataListEntity].Rows.Find(pk);

                MergeRow(dataRow, listRow);
            }
        }

        private static void UpdateRowValue(string property, object newValue, DataRow row)
        {
            if (newValue == null
                || (row.Table.Columns[property].DataType == typeof(string) & string.Empty.Equals(newValue))
                || (row.Table.Columns[property].DataType == typeof(Guid) & string.Empty.Equals(newValue))
                )
            {
                row[property] = DBNull.Value;
            }
            else
            {
                row[property] =  newValue;
            }
        }

        public virtual ArrayList DeleteObject(string entity, object id)
        {
            lock (_lock)
            {
                DataRow row = GetSessionRow(entity, id);
                DataRow rootRow = DatasetTools.RootRow(row);
                DataSet dataset = row.Table.DataSet;

                // get the changes for the deleted row before we actually deleted, because then the row would be inaccessible
                ArrayList deletedItems = new ArrayList();
                Dictionary<string, List<DeletedRowInfo>> backup = BackupDeletedRows(row);
                object[] listRowBackup = null;

                deletedItems.Add(GetChangeInfo(null, row, -1));
                AddChildDeletedItems(deletedItems, row);

                // get the parent rows for the rule handler in order to update them
                ArrayList parentRows = new ArrayList();
                foreach (DataRelation relation in row.Table.ParentRelations)
                {
                    parentRows.AddRange(row.GetParentRows(relation, DataRowVersion.Default));
                }

                try
                {
                    // .NET BUGFIX: Dataset does not refresh aggregated calculated columns on delete, we have to raise change event
                    if (Origam.DA.DatasetTools.IsRowAggregated(row))
                    {
                        row.BeginEdit();
                        foreach (DataColumn col in row.Table.Columns)
                        {
                            if (col.ReadOnly == false & (col.DataType == typeof(int) | col.DataType == typeof(float) | col.DataType == typeof(decimal) | col.DataType == typeof(long)))
                            {
                                object zero = Convert.ChangeType(0, col.DataType);
                                if (!row[col].Equals(zero)) row[col] = 0;
                            }
                        }
                        row.EndEdit();
                    }
                }
                catch
                {
                }

                try
                {
                    // DELETE THE ROW
                    row.Delete();

                    // handle rules for the data changes after the row has been deleted
                    this.RuleHandler.OnRowDeleted((DataRow[])parentRows.ToArray(typeof(DataRow)), row, this.XmlData, this.RuleSet, this.RuleEngine);

                    ArrayList listOfChanges = new ArrayList();


                    // get the changes - from root - e.g. recalculated totals after deletion
                    if (rootRow.Table.TableName != entity)
                    {
                        if (_entityHasRuleDependencies[rootRow.Table.TableName])
                        {
                            listOfChanges.AddRange(GetChangesByRow(null, rootRow, 0, this.Data.HasErrors, this.Data.HasChanges()));
                        }
                    }

                    // include the deletions
                    listOfChanges.AddRange(deletedItems);


                    if (IsLazyLoadedEntity(entity))
                    {
                        // delete the row from the list
                        DataTable table = GetTable(this.DataListEntity, this.DataList);
                        row = table.Rows.Find(id);
                        listRowBackup = row.ItemArray;

                        row.Delete();
                        table.AcceptChanges();

                        // save the data
                        listOfChanges.AddRange((IList)this.ExecuteAction(SessionStore.ACTION_SAVE));
                    }
                    return listOfChanges;
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        log.ErrorFormat("Caught an exception when trying to delete an object {0},{1} from session store: `{2}'",
                            entity, id, ex.ToString());
                    }

                    this.UnregisterEvents();

                    try
                    {
                        // delete the root row because we have a backup from the root row
                        if (rootRow.RowState != DataRowState.Deleted)
                        {
                            rootRow.Delete();
                        }

                        // we reset the changes
                        dataset.AcceptChanges();

                        // and then we import all the rows from the root row down
                        foreach (KeyValuePair<string, List<DeletedRowInfo>> tablePair in backup)
                        {
                            foreach (DeletedRowInfo info in tablePair.Value)
                            {
                                info.ImportData(dataset.Tables[tablePair.Key]);
                            }
                        }

                        // we also return the list row if it has been deleted
                        if (listRowBackup != null)
                        {
                            this.DataList.Tables[this.DataListEntity].Rows.Add(listRowBackup).AcceptChanges();
                        }
                    }
                    finally
                    {
                        this.RegisterEvents();
                    }

                    throw;
                }
            }
        }

        public List<ArrayList> GetDataForMatrix(string entity, string[] rows, string[] columns)
        {
            lock (_lock)
            {
                List<ArrayList> result = new List<ArrayList>();
                DataTable table = GetTable(entity, DataList);
                if (table.PrimaryKey.Length != 1)
                {
                    throw new Exception("There must be exactly 1 primary key column for GetDataForMatrix.");
                }
                foreach (string rowId in rows)
                {
                    object pk = GetPrimaryKey(table, rowId);
                    DataRow row = table.Rows.Find(pk);
                    if (row == null)
                    {
                        throw new ArgumentOutOfRangeException(
                            string.Format("Could not find row {0} in entity {1}.",
                            rowId, entity));
                    }
                    LazyLoadListRowData(pk, row);
                    ArrayList resultRow = SessionStore.GetRowData(row, columns, false);
                    resultRow.Insert(0, rowId);
                    result.Add(resultRow);
                }
                return result;
            }
        }

        public static object GetPrimaryKey(DataTable table, string rowId)
        {
            object pk;
            if (table.PrimaryKey[0].DataType == typeof(Guid))
            {
                pk = new Guid(rowId);
            }
            else
            {
                pk = rowId;
            }
            return pk;
        }

        private static Dictionary<string, List<DeletedRowInfo>> BackupDeletedRows(DataRow row)
        {
            Dictionary<string, List<DeletedRowInfo>> backup = new Dictionary<string, List<DeletedRowInfo>>();
            if (!backup.ContainsKey(row.Table.TableName)) backup.Add(row.Table.TableName, new List<DeletedRowInfo>());

            backup[row.Table.TableName].Add(new DeletedRowInfo(row));
            BackupChildRows(row, backup);
            return backup;
        }

        private static void BackupChildRows(DataRow parentRow, Dictionary<string, List<DeletedRowInfo>> backup)
        {
            foreach (DataRelation relation in parentRow.Table.ChildRelations)
            {
                foreach (DataRow childRow in parentRow.GetChildRows(relation))
                {
                    if (!backup.ContainsKey(childRow.Table.TableName)) backup.Add(childRow.Table.TableName, new List<DeletedRowInfo>());
                    backup[childRow.Table.TableName].Add(new DeletedRowInfo(childRow));
                    BackupChildRows(childRow, backup);
                }
            }
        }

        public ArrayList CopyObject(string entity, object originalId, 
            string requestingGrid, ArrayList entities, 
            IDictionary<string, object> forcedValues)
        {
            lock (_lock)
            {
                if (originalId == null)
                {
                    throw new NullReferenceException("Original record not set. Cannot copy record.");
                }

                DataTable table = GetTable(entity, this.Data);
                DataRow row = GetSessionRow(entity, originalId);

                UserProfile profile = SecurityTools.CurrentUserProfile();

                ArrayList toSkip = new ArrayList();
                foreach (DataTable t in this.Data.Tables)
                {
                    if (!entities.Contains(t.TableName) && ! IsArrayChild(t))
                    {
                        toSkip.Add(t.TableName);
                    }
                }

                DataSet tmpDS = DatasetTools.CloneDataSet(row.Table.DataSet, false);

                DatasetTools.GetDataSlice(tmpDS, new List<DataRow>{row}, profile.Id, true, toSkip);

                try
                {
                    DataRow newTmpRow = tmpDS.Tables[table.TableName].Rows[0];
                    if (!this.RuleEngine.RowLevelSecurityState(newTmpRow, profile.Id).AllowCreate)
                    {
                        throw new Exception(Properties.Resources.ErrorCreateRecordNotAllowed);
                    }
                    // set any values passed by the client (e.g. when adding an entry into a calendar,
                    // resource and date are known so they are handed over directly when adding a record
                    if (forcedValues != null)
                    {
                        foreach (KeyValuePair<string, object> entry in forcedValues)
                        {
                            newTmpRow[entry.Key] = entry.Value;
                        }
                    }
                    this.UnregisterEvents();
                    this.Data.EnforceConstraints = false;
                    if(IsLazyLoadedEntity(entity))
                    {
                        // we are copying on the root of delayed loaded form
                        // so we clear the dataset completely and merge back only the copy
                        DatasetTools.Clear(this.Data);
                    }
                    MergeParams mergeParams = new MergeParams(profile.Id);
                    DatasetTools.MergeDataSet(this.Data, tmpDS, null, mergeParams);
                    this.RegisterEvents();

                    object[] pk = DatasetTools.PrimaryKey(newTmpRow);
                    if (IsLazyLoadedEntity(entity))
                    {
                        this.CurrentRecordId = pk[0];
                    }
                    DataRow newRow = table.Rows.Find(pk);
                    this.RuleHandler.OnRowCopied(newRow, this.XmlData, this.RuleSet, this.RuleEngine);

                    NewRowToDataList(newRow);

                    return GetChangesByRow(requestingGrid, newRow, 4, this.Data.HasErrors, this.Data.HasChanges());
                }
                finally
                {
                    this.UnregisterEvents();
                    this.RegisterEvents();
                    this.Data.EnforceConstraints = true;
                }
            }
        }

        private bool IsArrayChild(DataTable table)
        {
            if(table.ParentRelations.Count == 1)
            {
                foreach (DataColumn column in table.ParentRelations[0].ParentTable.Columns)
                {
                    if (column.ExtendedProperties.Contains(Const.ArrayRelation) 
                        && (string)column.ExtendedProperties[Const.ArrayRelation] 
                            == table.ParentRelations[0].RelationName)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        internal void NewRowToDataList(DataRow newRow)
        {
            lock (_lock)
            {
                // merge to the list
                if (IsLazyLoadedEntity(newRow.Table.TableName))
                {
                    DataTable listTable = this.DataList.Tables[this.DataListEntity];
                    DataRow newListRow = listTable.NewRow();
                    MergeRow(newRow, newListRow);
                    listTable.Rows.Add(newListRow);
                    newListRow.AcceptChanges();

                    OnNewRecord(newRow.Table.TableName, DatasetTools.PrimaryKey(newRow)[0]);
                }
            }
        }

        internal void UpdateListRow(DataRow r)
        {
            lock (_lock)
            {
                if (this.DataList != null)
                {
                    // find the list row
                    DataRow listRow = this.DataList.Tables[r.Table.TableName].Rows.Find(DatasetTools.PrimaryKey(r));

                    if (listRow != null)
                    {
                        MergeRow(r, listRow);
                        // accept changes, the list row is read-only anyway, but...
                        listRow.AcceptChanges();
                    }
                }
            }
        }

        internal virtual void OnNewRecord(string entity, object id)
        {

        }

        internal static void MergeRow(DataRow r, DataRow listRow)
        {
            listRow.BeginEdit();
            // merge in the changed data
            foreach (DataColumn col in listRow.Table.Columns)
            {
                if ((col.Expression == null || col.Expression == "")
                    && col.ColumnName != LIST_LOADED_COLUMN_NAME)
                {
                    bool wasReadOnly = col.ReadOnly;
                    col.ReadOnly = false;
                    listRow[col] = r[col.ColumnName];
                    if (wasReadOnly) col.ReadOnly = true;
                }
            }
            listRow.EndEdit();
        }

        internal static string[] GetColumnNames(DataTable table)
        {
            string[] columns = new string[table.Columns.Count];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                columns[i] = table.Columns[i].ColumnName;
            }
            return columns;
        }

        /// <summary>
        /// Gets data by parent record ID (only for delayed data loading).
        /// </summary>
        /// <param name="sessionFormIdentifier"></param>
        /// <param name="entity"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public virtual ArrayList GetData(string childEntity, object parentRecordId, object rootRecordId)
        {
            throw new Exception("GetData not available for " + this.GetType().Name);
        }

        public virtual ArrayList GetRowData(string entity, object id, bool ignoreDirtyState)
        {
            throw new Exception("GetRowData not available for " + this.GetType().Name);
        }
        #endregion

        private void AddChildDeletedItems(ArrayList deletedItems, DataRow deletedRow)
        {
            foreach (DataRelation child in deletedRow.Table.ChildRelations)
            {
                foreach (DataRow childRow in deletedRow.GetChildRows(child))
                {
                    deletedItems.Add(GetChangeInfo(null, childRow, -1));

                    AddChildDeletedItems(deletedItems, childRow);
                }
            }
        }

        #endregion

        internal ArrayList UpdateObjectBatch(string entity, string property, Hashtable values)
        {
            ArrayList result = new ArrayList();
 
            lock (_lock)
            {
                foreach (DictionaryEntry entry in values)
                {
                    result.AddRange(UpdateObject(entity, entry.Key, property, entry.Value));
                }
            }

            return result;
        }

        internal ArrayList UpdateObjectEx(string entity, object id, Hashtable values)
        {
            ArrayList result = new ArrayList();

            lock (_lock)
            {
                foreach (DictionaryEntry entry in values)
                {
                    result.AddRange(UpdateObject(entity, id, (string)entry.Key, entry.Value));
                }
            }

            return result;
        }
    }
}
