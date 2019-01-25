﻿#region license
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

namespace Origam.Server
{
    public class UIGridSortConfiguration
    {
        private string _field;
        private string _direction;

        public UIGridSortConfiguration(string field, string direction)
        {
            this.Field = field;
            this.Direction = direction;
        }

        public string Field {
            get
            {
                return _field;
            }
            set
            {
                _field = value;
            }
        }

        public string Direction {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
            }
        }
    }
}