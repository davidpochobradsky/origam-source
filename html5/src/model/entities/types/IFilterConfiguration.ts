import { ISetting as IFilterSettingString } from "gui/Components/ScreenElements/Table/FilterSettings/HeaderControls/FilterSettingsString";

// TODO: Extract types so that model layer does not depend on view layer?

type ISetting = IFilterSettingString;

export interface IFilterTerm {
  propertyId: string;
  setting: ISetting;
}

export interface IFilterConfigurationData {}

export interface IFilterConfiguration extends IFilterConfigurationData {
  $type_IFilterConfigurationData: 1;

  isFilterControlsDisplayed: boolean;
  filtering: IFilterTerm[];
  getSettingByPropertyId(propertyId: string): IFilterTerm | undefined;
  setFilter(term: IFilterTerm): void;
  clearFilters(): void;

  onFilterDisplayClick(event: any): void;

  parent?: any;
}
