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
using System.Collections.Generic;

namespace Origam.ServerCore
{
    public class MenuLookupIndex
    {
        private readonly Dictionary<Guid, ICollection<Guid>> menuToAllowedLookups = 
            new Dictionary<Guid, ICollection<Guid>>();
            
        public void AddIfNotPresent(Guid menuId, HashSet<Guid> containedLookups)
        {
            if (menuToAllowedLookups.ContainsKey(menuId)) return;
            menuToAllowedLookups.Add(menuId, containedLookups);
        }

        public bool IsAllowed(Guid menuItemId, Guid lookupId)
        {
            if (!menuToAllowedLookups.ContainsKey(menuItemId)) return false;
            return menuToAllowedLookups[menuItemId].Contains(lookupId);
        }

        public bool HasDataFor(Guid menuItemId)
        {
            return menuToAllowedLookups.ContainsKey(menuItemId);
        }
    }
}