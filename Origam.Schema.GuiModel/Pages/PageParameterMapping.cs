#region license
/*
Copyright 2005 - 2021 Advantage Solutions, s. r. o.

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
using System.ComponentModel;
using System.Xml.Serialization;
using Origam.DA.ObjectPersistence;
using Origam.Schema.EntityModel;


namespace Origam.Schema.GuiModel
{
	[SchemaItemDescription("Parameter Mapping", "Parameter Mappings", "file-mapping.png")]
    [HelpTopic("Parameter+Mapping")]
	[XmlModelRoot(CategoryConst)]
    [ClassMetaVersion("6.0.0")]
	public class PageParameterMapping : AbstractSchemaItem
	{
		public const string CategoryConst = "PageParameterMapping";

		public PageParameterMapping() : base() {Init();}
		public PageParameterMapping(Guid schemaExtensionId) : base(schemaExtensionId) {Init();}
		public PageParameterMapping(Key primaryKey) : base(primaryKey) {Init();}

		private void Init()
		{
		}

		#region Properties
		[Category("Mapping")]
		[EntityColumn("SS01")]
		[Description("Name of url query string parameter, e.g. in case http://my-api/my-page?searchstring=value the mapped parametr should be 'searchstring'")]
		[XmlAttribute ("mappedParameter")]
		public string MappedParameter { get; set; } = "";

		[EntityColumn("G01")]  
		public Guid DataConstantId;

		[Category("Mapping")]
		[TypeConverter(typeof(DataConstantConverter))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[XmlReference("defaultValue", "DataConstantId")]
		public DataConstant DefaultValue
		{
			get => (AbstractSchemaItem)this.PersistenceProvider.RetrieveInstance(typeof(AbstractSchemaItem), new ModelElementKey(this.DataConstantId)) as DataConstant;
			set
			{
				this.DataConstantId = (value == null ? Guid.Empty : (Guid)value.PrimaryKey["Id"]);
			}
		}

		[Category("Lists")]
		[EntityColumn("B01"), DefaultValue(false)]
		[XmlAttribute ("isList")]
		public bool IsList { get; set; } = false;

		[EntityColumn("G02")]  
		public Guid SeparatorDataConstantId;

		[Category("Lists")]
		[TypeConverter(typeof(DataConstantConverter))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[XmlReference("listSeparator", "SeparatorDataConstantId")]
		public DataConstant ListSeparator
		{
			get => (AbstractSchemaItem)this.PersistenceProvider.RetrieveInstance(typeof(AbstractSchemaItem), new ModelElementKey(this.SeparatorDataConstantId)) as DataConstant;
			set
			{
				this.SeparatorDataConstantId = (value == null ? Guid.Empty : (Guid)value.PrimaryKey["Id"]);
			}
		}

		[EntityColumn("ItemType")]
		public override string ItemType => CategoryConst;
		#endregion			
	}
}