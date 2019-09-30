import { action, computed, observable } from "mobx";
import { getParentRow } from "model/selectors/DataView/getParentRow";
import { getSelectedRowId } from "model/selectors/TablePanelView/getSelectedRowId";
import { getDataSourceByEntity } from "../selectors/DataSources/getDataSourceByEntity";
import { getDataTable } from "../selectors/DataView/getDataTable";
import { getFormScreen } from "../selectors/FormScreen/getFormScreen";
import { getIsDialog } from "../selectors/getIsDialog";
import { IDataViewLifecycle } from "./DataViewLifecycle/types/IDataViewLifecycle";
import { IFormPanelView } from "./FormPanelView/types/IFormPanelView";
import { ITablePanelView } from "./TablePanelView/types/ITablePanelView";
import { IAction, IActionPlacement, IActionType } from "./types/IAction";
import { IDataTable } from "./types/IDataTable";
import { IDataView, IDataViewData } from "./types/IDataView";
import { IPanelViewType } from "./types/IPanelViewType";
import { IProperty } from "./types/IProperty";
import { getBindingToParent } from "model/selectors/DataView/getBindingToParent";
import { getDataSourceFieldByName } from "model/selectors/DataSources/getDataSourceFieldByName";
import { getEntity } from "model/selectors/DataView/getEntity";
import { getBindingParent } from "model/selectors/DataView/getBindingParent";
import { IRowState } from "./types/IRowState";
import { ILookupLoader } from "./types/ILookupLoader";

export class DataView implements IDataView {
  
  $type_IDataView: 1 = 1;

  constructor(data: IDataViewData) {
    Object.assign(this, data);
    this.properties.forEach(o => (o.parent = this));
    this.actions.forEach(o => (o.parent = this));
    this.dataTable.parent = this;
    this.lifecycle.parent = this;
    this.tablePanelView.parent = this;
    this.formPanelView.parent = this;
    this.lookupLoader.parent = this;
  }

  isReorderedOnClient: boolean = true;

  id = "";
  modelInstanceId = "";
  name = "";
  modelId = "";
  defaultPanelView = IPanelViewType.Table;
  isHeadless = false;
  disableActionButtons = false;
  showAddButton = false;
  showDeleteButton = false;
  showSelectionCheckboxes = false;
  isGridHeightDynamic = false;
  selectionMember = "";
  orderMember = "";
  isDraggingEnabled = false;
  entity = "";
  dataMember = "";
  isRootGrid = false;
  isRootEntity = false;
  isPreloaded = false;
  requestDataAfterSelectionChange = false;
  confirmSelectionChange = false;
  properties: IProperty[] = [];
  actions: IAction[] = [];

  @observable tableViewProperties: IProperty[] = [];
  dataTable: IDataTable = null as any;
  formViewUI: any;
  lifecycle: IDataViewLifecycle = null as any;
  tablePanelView: ITablePanelView = null as any;
  formPanelView: IFormPanelView = null as any;
  lookupLoader: ILookupLoader = null as any;

  @observable activePanelView: IPanelViewType = IPanelViewType.Table;
  @observable isEditing: boolean = false;

  @observable selectedRowId: string | undefined;
  @computed get selectedRowIndex(): number | undefined {
    return this.selectedRowId
      ? this.dataTable.getExistingRowIdxById(this.selectedRowId)
      : undefined;
  }
  @computed get selectedRow(): any[] | undefined {
    return this.selectedRowIndex !== undefined
      ? this.dataTable.getRowByExistingIdx(this.selectedRowIndex)
      : undefined;
  }
  @computed get isValidRowSelection(): boolean {
    return this.selectedRowIndex !== undefined;
  }

  @computed get panelViewActions() {
    return this.actions.filter(
      action => action.placement === IActionPlacement.PanelHeader
    );
  }

  @computed get toolbarActions() {
    return this.actions.filter(
      action =>
        action.placement === IActionPlacement.Toolbar &&
        action.type !== IActionType.SelectionDialogAction &&
        !getIsDialog(this)
    );
  }

  @computed get dialogActions() {
    return this.actions.filter(
      action =>
        action.type === IActionType.SelectionDialogAction || getIsDialog(this)
    );
  }

  get isWorking() {
    // TODO
    return false;
  }

  @computed get isAnyBindingAncestorWorking() {
    if (this.isBindingRoot) {
      return false;
    } else {
      return (
        this.bindingParent.isWorking ||
        this.bindingParent.isAnyBindingAncestorWorking
      );
    }
  }

  @computed
  get isBindingRoot() {
    return this.parentBindings.length === 0;
  }

  @computed get isBindingParent() {
    return this.childBindings.length > 0;
  }

  @computed get bindingParent() {
    return this.parentBindings[0].parentDataView;
  }

  @computed get bindingRoot(): IDataView {
    // TODO: If there ever is multiparent case, remove duplicates in the result
    let root: IDataView = this;
    while (!root.isBindingRoot) {
      root = root.bindingParent!;
    }
    return root;
  }

  @computed
  get parentBindings() {
    const screen = getFormScreen(this);
    return screen.getBindingsByChildId(this.modelInstanceId);
  }

  @computed
  get childBindings() {
    const screen = getFormScreen(this);
    return screen.getBindingsByParentId(this.modelInstanceId);
  }

  @computed get dataSource() {
    return getDataSourceByEntity(this, this.entity)!;
  }

  @computed get bindingParametersFromParent() {
    // debugger
    const parentRow = getParentRow(this);
    if (parentRow) {
      const parent = getBindingParent(this);
      const parentEntity = getEntity(parent);
      const parentDataTable = getDataTable(parent);

      const bindingToParent = getBindingToParent(this)!;
      const result: { [key: string]: string } = {};
      for (let bp of bindingToParent.bindingPairs) {
        const parentDataSourceField = getDataSourceFieldByName(
          parent,
          bp.parentPropertyId
        )!;
        result[
          bp.childPropertyId
        ] = parentDataTable.getCellValueByDataSourceField(
          parentRow,
          parentDataSourceField
        );
      }
      console.log(result)
      return result
    } else {
      return {};
    }
  }

  @action.bound
  onFormPanelViewButtonClick(event: any) {
    this.activePanelView = IPanelViewType.Form;
  }

  @action.bound
  onTablePanelViewButtonClick(event: any) {
    this.activePanelView = IPanelViewType.Table;
  }

  @action.bound selectNextRow() {
    const selectedRowId = getSelectedRowId(this);
    const newId = selectedRowId
      ? getDataTable(this).getNextExistingRowId(selectedRowId)
      : undefined;
    if (newId) {
      this.selectRowById(newId);
    }
  }

  @action.bound selectPrevRow() {
    const selectedRowId = getSelectedRowId(this);
    const newId = selectedRowId
      ? getDataTable(this).getPrevExistingRowId(selectedRowId)
      : undefined;
    if (newId) {
      this.selectRowById(newId);
    }
  }

  @action.bound onFieldChange(
    event: any,
    row: any[],
    property: IProperty,
    value: any
  ) {
    console.log("ofc");
    if (!property.readOnly) {
      getDataTable(this).setFormDirtyValue(row, property.id, value);
    }
  }

  @action.bound selectFirstRow() {
    const dataTable = getDataTable(this);
    const firstRow = dataTable.getFirstRow();
    if (firstRow) {
      this.selectRowById(dataTable.getRowId(firstRow));
    }
  }

  @action.bound selectRowById(id: string | undefined) {
    if (id !== this.selectedRowId) {
      //cannotChangeRowDialog(this);
      //return
      this.setSelectedRowId(id);
    }
  }

  @action.bound selectRow(row: any[]) {
    this.selectRowById(this.dataTable.getRowId(row));
  }

  @action.bound
  setSelectedRowId(id: string | undefined): void {
    this.selectedRowId = id;
  }

  setEditing(state: boolean): void {
    this.isEditing = state;
  }

  @action.bound start() {
    this.lifecycle.start();
  }

  parent?: any;
}
