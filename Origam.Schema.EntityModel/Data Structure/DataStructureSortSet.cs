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

using Origam.DA.Common;
using System;
using Origam.DA.ObjectPersistence;
using System.Xml.Serialization;

namespace Origam.Schema.EntityModel
{
	/// <summary>
	/// Summary description for DataQuery.
	/// </summary>
	[SchemaItemDescription("Sort Set", "Sort Sets", "icon_sort-set.png")]
    [HelpTopic("Sort+Sets")]
	[XmlModelRoot(CategoryConst)]
    [ClassMetaVersion("1.0.0")]
	public class DataStructureSortSet : AbstractSchemaItem, ISchemaItemFactory
	{
		public const string CategoryConst = "DataStructureSortSet";

		public DataStructureSortSet() : base() {}

		public DataStructureSortSet(Guid schemaExtensionId) : base(schemaExtensionId) {}

		public DataStructureSortSet(Key primaryKey) : base(primaryKey)	{}
	
		#region Overriden AbstractDataEntityColumn Members
		
		[EntityColumn("ItemType")]
		public override string ItemType
		{
			get
			{
				return CategoryConst;
			}
		}

		public override bool UseFolders
		{
			get
			{
				return false;
			}
		}

		public override bool CanMove(Origam.UI.IBrowserNode2 newNode)
		{
			return (newNode as ISchemaItem).PrimaryKey.Equals(this.ParentItem.PrimaryKey);
		}


		#endregion

		#region ISchemaItemFactory Members

		public override Type[] NewItemTypes
		{
			get
			{
				return new Type[] {typeof(DataStructureSortSetItem)};
			}
		}

		public override AbstractSchemaItem NewItem(Type type, Guid schemaExtensionId, SchemaItemGroup group)
		{
			AbstractSchemaItem item;

			if(type == typeof(DataStructureSortSetItem))
			{
				item = new DataStructureSortSetItem(schemaExtensionId);
				item.Name = "NewSortSetItem";

			}
			else
				throw new ArgumentOutOfRangeException("type", type, ResourceUtils.GetString("ErrorDataStructureSortUnknownType"));

			item.Group = group;
			item.RootProvider = this;
			item.PersistenceProvider = this.PersistenceProvider;
			this.ChildItems.Add(item);

			return item;
		}

		#endregion
	}
}
