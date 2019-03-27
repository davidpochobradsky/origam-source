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
using System.ComponentModel;
using System.Xml.Serialization;
using Origam.DA.ObjectPersistence;
using Origam.Schema.EntityModel;

namespace Origam.Schema.MenuModel
{
	/// <summary>
	/// Summary description for Menu.
	/// </summary>
	[SchemaItemDescription("Parameter Mapping", "Parameter Mappings", 3)]
    [HelpTopic("Menu+Parameter+Mapping")]
	[XmlModelRoot(ItemTypeConst)]
	public class SelectionDialogParameterMapping : AbstractSchemaItem
	{
		public const string ItemTypeConst = "SelectionDialogParameterMapping";

		public SelectionDialogParameterMapping() : base() {}

		public SelectionDialogParameterMapping(Guid schemaExtensionId) : base(schemaExtensionId) {}

		public SelectionDialogParameterMapping(Key primaryKey) : base(primaryKey)	{}

		#region Overriden AbstractSchemaItem Members
		
		[EntityColumn("ItemType")]
		[XmlAttribute(AttributeName = "itemType")] 
		public override string ItemType
		{
			get
			{
				return ItemTypeConst;
			}
		}

		[Browsable(false)]
		public override bool UseFolders
		{
			get
			{
				return false;
			}
		}

		public override string Icon
		{
			get
			{
				return "3";
			}
		}

		#endregion

		#region Properties
		[EntityColumn("G01")] 
		public Guid EntityFieldId;

		[TypeConverter(typeof(MenuSelectionDialogFieldConverter))]
		[XmlReference("selectionDialogField", "EntityFieldId")]
		public IDataEntityColumn SelectionDialogField
		{
			get
			{
				return (IDataEntityColumn)this.PersistenceProvider.RetrieveInstance(typeof(AbstractSchemaItem), new ModelElementKey(this.EntityFieldId));
			}
			set
			{
				this.EntityFieldId = value == null ? Guid.Empty : (Guid)value.PrimaryKey["Id"];
			}
		}
		#endregion
	}
}
