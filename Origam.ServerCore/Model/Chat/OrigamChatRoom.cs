﻿#region license
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
using System.Data;

namespace Origam.ServerCore.Model.Chat
{
    public class OrigamChatRoom
    {
        public OrigamChatRoom(Guid guid, string name)
        {
            this.id = guid;
            this.topic = name;
        }

        public Guid id { get; set; }
        public string topic { get; set; }

        internal static OrigamChatRoom CreateJson(DataSet ChatRoomDataSet)
        {
            OrigamChatRoom chatRoom = null;
            foreach (DataTable table in ChatRoomDataSet.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    chatRoom = new OrigamChatRoom(row.Field<Guid>("Id"), row.Field<string>("Name"));
                }
            }
            return chatRoom;
        }
    }
}