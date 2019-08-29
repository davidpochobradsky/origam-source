import { IDataView } from "./IDataView";
import { IDataSource } from "./IDataSource";
import { IComponentBinding } from "./IComponentBinding";
import { IFormScreenLifecycle } from "./IFormScreenLifecycle";
import { IAction } from "./IAction";

export interface ILoadedFormScreenData {
  title: string;
  menuId: string;
  openingOrder: number;
  showInfoPanel: boolean;
  autoRefreshInterval: number;
  cacheOnClient: boolean;
  autoSaveOnListRecordChange: boolean;
  requestSaveAfterUpdate: boolean;
  dataViews: IDataView[];
  dataSources: IDataSource[];
  componentBindings: IComponentBinding[];
  screenUI: any;
  formScreenLifecycle: IFormScreenLifecycle;
  sessionId: string;
}

export interface ILoadedFormScreen extends ILoadedFormScreenData {
  $type_ILoadedFormScreen: 1;

  isDirty: boolean;

  isLoading: false;
  rootDataViews: IDataView[];
  dontRequestData: boolean;

  getBindingsByChildId(childId: string): IComponentBinding[];
  getBindingsByParentId(parentId: string): IComponentBinding[];
  getDataViewByModelInstanceId(modelInstanceId: string): IDataView | undefined;
  getDataViewByEntity(entity: string): IDataView | undefined;
  getDataSourceByEntity(entity: string): IDataSource | undefined;

  toolbarActions: Array<{ section: string; actions: IAction[] }>;
  dialogActions: IAction[];

  setDirty(state: boolean): void;

  printMasterDetailTree(): void;

  parent?: any;
}

export interface ILoadingFormScreenData {
  formScreenLifecycle: IFormScreenLifecycle;
}

export interface ILoadingFormScreen extends ILoadingFormScreenData {
  $type_ILoadingFormScreen: 1;

  isLoading: true;
  parent?: any;

  run(): void;
}

export type IFormScreen = ILoadingFormScreen | ILoadedFormScreen;

export const isILoadingFormScreen = (o: any): o is ILoadingFormScreen =>
  o.$type_ILoadingFormScreen;
export const isILoadedFormScreen = (o: any): o is ILoadedFormScreen =>
  o.$type_ILoadedFormScreen;