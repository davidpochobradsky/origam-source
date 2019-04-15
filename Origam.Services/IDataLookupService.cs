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
using System.Data;
using Origam.Services;
using Origam.UI;

namespace Origam.Workbench.Services
{
	public interface IDataLookupService : IWorkbenchService
	{
		object GetDisplayText(Guid lookupId, object lookupValue, string transactionId);
		object GetDisplayText(Guid lookupId, object lookupValue, 
            bool useCache, bool returnMessageIfNull, string transactionId);
		object GetDisplayText(Guid lookupId, Hashtable parameters, 
            bool useCache, bool returnMessageIfNull, string transactionId);
        object CreateRecord(Guid lookupId, Hashtable values, string transactionId);
		DataTable GetList(LookupListRequest request);
		DataView GetList(Guid lookupId, string transactionId);
		DataView GetList(Guid lookupId, Hashtable parameters, string transactionId);
		object LinkTarget(ILookupControl lookupControl, object value);
		Hashtable LinkParameters(object linkTarget, object value);
		IMenuBindingResult GetMenuBinding(Guid lookupId, object value);
		bool HasMenuBindingWithSelection(Guid lookupId);
		DataTable GetAllValues(Guid lookupId, Hashtable keys);
		Hashtable GetAllValuesDistinct(Guid lookupId, Hashtable keys);
		
		string ValueFromRow(DataRow row, string[] columns);
	}
}
