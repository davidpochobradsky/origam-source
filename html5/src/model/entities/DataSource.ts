import { IDataSource, IDataSourceData } from "./types/IDataSource";
import { IDataSourceField } from "./types/IDataSourceField";
import { IRowState } from "./types/IRowState";

export class DataSource implements IDataSource {
  $type_IDataSource: 1 = 1;

  constructor(data: IDataSourceData) {
    Object.assign(this, data);
    this.fields.forEach(o => (o.parent = this));
    this.rowState.parent = this;
  }

  parent?: any;

  entity: string = "";
  dataStructureEntityId: string = "";
  identifier: string = "";
  lookupCacheKey: string = "";
  fields: IDataSourceField[] = [];
  rowState: IRowState = null as any;

  getFieldByName(name: string): IDataSourceField | undefined {
    return this.fields.find(field => field.name === name);
  }

  getFieldByIndex(index: number): IDataSourceField | undefined {
    return this.fields.find(field => field.index === index);
  }
}
