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
using System.Xml;
using System.Data;

using Origam.Workbench.Services;
using Origam.Schema.GuiModel;

namespace Origam.OrigamEngine.ModelXmlBuilders
{
	/// <summary>
	/// Summary description for AsPanelPropertyBuilder.
	/// </summary>
	public class AsPanelPropertyBuilder
	{
		public static XmlElement CreateProperty(XmlElement propertiesElement, XmlElement propertyNamesElement, Guid modelId, string bindingMember, string caption, 
			string gridCaption, DataTable table, bool readOnly, int left, int top, int width, int height, int captionLength, string captionPosition, 
			string gridColumnWidth, UIStyle style)
		{
			return CreateProperty("Property", propertiesElement, propertyNamesElement, modelId, bindingMember, caption, gridCaption, table, readOnly, 
				left, top, width, height, captionLength, captionPosition, gridColumnWidth, style);
		}

			public static XmlElement CreateProperty(string elementName, XmlElement propertiesElement, XmlElement propertyNamesElement, Guid modelId, string bindingMember, string caption, 
			string gridCaption, DataTable table, bool readOnly, int left, int top, int width, int height, int captionLength, string captionPosition,
            string gridColumnWidth, UIStyle style)
		{
            IDocumentationService documentationSvc = ServiceManager.Services.GetService(typeof(IDocumentationService)) as IDocumentationService;

			XmlElement propertyElement = propertiesElement.OwnerDocument.CreateElement(elementName);
			propertiesElement.AppendChild(propertyElement);

			if(propertyNamesElement != null)
			{
				XmlElement propertyNameElement = propertyNamesElement.OwnerDocument.CreateElement("string");
				propertyNamesElement.AppendChild(propertyNameElement);
				propertyNameElement.InnerText = bindingMember;
			}

			if(string.IsNullOrEmpty(caption)) caption = table.Columns[bindingMember].Caption;
			Guid id = (Guid)table.Columns[bindingMember].ExtendedProperties["Id"];

			string propertyDocumentation = documentationSvc
				.GetDocumentation(id, DocumentationType.USER_LONG_HELP)
				?.Replace("'", "\'")
				.Replace("\"", "\\\"")
				.Replace("\r\n", "\\r\\n")
				.Replace("\t", "\\t");

			if(propertyDocumentation != "" & propertyDocumentation != null)
			{
				XmlElement propertyDoc = propertyElement.OwnerDocument.CreateElement("ToolTip");
				propertyElement.AppendChild(propertyDoc);
				propertyDoc.InnerText = propertyDocumentation;
			}

			propertyElement.SetAttribute("Id", bindingMember);
			propertyElement.SetAttribute("ModelInstanceId", modelId.ToString());
			propertyElement.SetAttribute("Name", caption);
			if(!string.IsNullOrEmpty(gridCaption)) propertyElement.SetAttribute("GridColumnCaption", gridCaption);
			propertyElement.SetAttribute("ReadOnly", XmlConvert.ToString(readOnly));

			propertyElement.SetAttribute("X", XmlConvert.ToString(left));
			propertyElement.SetAttribute("Y", XmlConvert.ToString(top));
			propertyElement.SetAttribute("Width", XmlConvert.ToString(width));
			if(!string.IsNullOrEmpty(gridColumnWidth)) propertyElement.SetAttribute("GridColumnWidth", gridColumnWidth);
			propertyElement.SetAttribute("Height", XmlConvert.ToString(height));
			propertyElement.SetAttribute("CaptionLength", XmlConvert.ToString(captionLength));
			propertyElement.SetAttribute("CaptionPosition", captionPosition);

			if(style != null)
            {
                propertyElement.SetAttribute("Style", style.StyleDefinition());
            }

			return propertyElement;
		}
	}
}
