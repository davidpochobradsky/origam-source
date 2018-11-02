﻿#region license
/*
Copyright 2005 - 2018 Advantage Solutions, s. r. o.

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
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Mvp.Xml.Common.Xsl;
using Mvp.Xml.Exslt;
using Origam.DA.Service;
using Origam.Gui.Win;
using Origam.Schema;
using Origam.Schema.GuiModel;
using Origam.Schema.MenuModel;
using Origam.Workbench.Services;

namespace Origam.DocGenerator
{
    class DocCreate
    {
        private string DirectoryPath { get; set; }
        private XmlTextWriter Xmlwriter { get; set; }
        
        private IDocumentationService documentation;
        private MenuSchemaItemProvider menuprovider = new MenuSchemaItemProvider();
        private readonly FilePersistenceProvider persprovider;
        private readonly string XsltPath;
        private readonly MemoryStream mstream ;
        private readonly string RootFile ;
        private readonly string xmlsourcefile;

        public DocCreate(string path,string xslt, string rootfile, FileStorageDocumentationService documentation1,FilePersistenceProvider persprovider,string xmlfile)
        {
            if(string.IsNullOrEmpty(path))
            {
                 throw new Exception("Path for Export is not set!");
            }
            if (string.IsNullOrEmpty(xslt))
            {
                throw new Exception("Xslt template is not set!");
            }

            if (string.IsNullOrEmpty(rootfile))
            {
                throw new Exception("RootFileName is not set!");
            }

            xmlsourcefile = xmlfile;
            RootFile = rootfile;
            XsltPath = xslt;
            DirectoryPath = string.Join("", path.Split(Path.GetInvalidPathChars())); ;
            documentation = documentation1 ?? throw new Exception("Documentation  is not set!");
            menuprovider.PersistenceProvider = persprovider ?? throw new Exception("PersistenceProvider is not set!"); 
            this.persprovider = persprovider;
            mstream = new MemoryStream();
            CreateWriter();
        }

       
        private void CreateWriter()
        {
            Xmlwriter = new XmlTextWriter(mstream, Encoding.UTF8)
            {
                Formatting = Formatting.Indented
            };
            Xmlwriter.WriteStartDocument();
            Xmlwriter.WriteComment("This file is generated by the program.(" + DateTime.Now + ")");
        }

        private void MakeXml( ControlSetItem control, FormControlSet formItem, DataSet dataset,string dataMember)
        {
            string caption = "";
            string gridCaption = "";
            string bindingMember = "";
            string panelTitle = "";
            int tabIndex = 0;
            string section = "";

            foreach (PropertyValueItem property in control.ChildItemsByType(PropertyValueItem.ItemTypeConst))
            {
                if (property.ControlPropertyItem.Name == "TabIndex")
                {
                    tabIndex = property.IntValue;
                }

                if (property.ControlPropertyItem.Name == "Caption")
                {
                    caption = property.Value;
                }

                if (property.ControlPropertyItem.Name == "GridColumnCaption")
                {
                    gridCaption = property.Value;
                }

                if (property.ControlPropertyItem.Name == "PanelTitle")
                {
                    panelTitle = property.Value;
                }

                if (control.ControlItem.IsComplexType && property.ControlPropertyItem.Name == "DataMember")
                {
                    dataMember = property.Value;
                }
            }

            caption = (gridCaption == "" | gridCaption == null) ? caption : gridCaption;
            foreach (PropertyBindingInfo bindItem in control.ChildItemsByType(PropertyBindingInfo.ItemTypeConst))
            {
                bindingMember = bindItem.Value;
            }

            if (bindingMember != "")
            {
                DataTable table = dataset.Tables[FormGenerator.FindTableByDataMember(dataset, dataMember)];

                if (!table.Columns.Contains(bindingMember)) throw new Exception("Field '" + bindingMember + "' not found in a data structure for the form '" + control.RootItem.Path + "'");

                if (string.IsNullOrEmpty(caption))
                {
                    caption = table.Columns[bindingMember].Caption;
                }
                Guid id = (Guid)table.Columns[bindingMember].ExtendedProperties["Id"];
                WriteStartElement("Field", caption, control.ControlItem.Id.ToString(), control.ControlItem.GetType().Name);
                string docc = documentation.GetDocumentation(id, DocumentationType.USER_LONG_HELP);
                WriteElement("description", docc);
            }

            ArrayList sortedControls;
            if (control.ControlItem.IsComplexType)
            {
                string doc = documentation.GetDocumentation(control.ControlItem.PanelControlSet.Id, DocumentationType.USER_LONG_HELP);

                if (panelTitle!= "")
                {
                    section = panelTitle;
                }
                else
                {
                    section = "Panel";
                }
                if (string.IsNullOrEmpty(section))
                {
                    section = "Panel";
                }
                WriteStartElement("Section", section, control.ControlItem.Id.ToString(), control.ControlItem.GetType().Name);
                WriteElement("description", doc);
                sortedControls = control.ControlItem.PanelControlSet.ChildItems[0].ChildItemsByType(ControlSetItem.ItemTypeConst);
            }
            else
            {
                sortedControls = control.ChildItemsByType(ControlSetItem.ItemTypeConst);
            }

            sortedControls.Sort(new ControlSetItemComparer());
            foreach (ControlSetItem subControl in sortedControls)
            {
                MakeXml(subControl,formItem,dataset, dataMember);
            }
            if (bindingMember != "")
            {
                WriteEndElement();
            }
            if (section != "")
            {
                WriteEndElement();
            }
        }

        public void Run()
        {
            if(!string.IsNullOrEmpty(XsltPath))
            {
                MvpXslTransform processor = new MvpXslTransform(false);
               processor.Load(XsltPath);
            }

            List<AbstractSchemaItem> menulist = menuprovider.ChildItems.ToList();
            menulist.Sort();
            WriteStartElement("Menu");
            CreateXml(menulist[0]);
            WriteEndElement();
            CloseXml();
            if (!string.IsNullOrEmpty(xmlsourcefile))
            {
                SaveSchemaXml();
            }
            else
            {
                SaveXslt();
            }
        }

        private void SaveSchemaXml()
        {
            FileStream file = new FileStream(Path.Combine(DirectoryPath, RootFile),  FileMode.Create, FileAccess.Write);
            mstream.WriteTo(file);
            file.Close();
            mstream.Close();
        }

        private void SaveXslt()
        {
            mstream.Seek(0, SeekOrigin.Begin);
            XPathDocument doc = new XPathDocument(mstream);
            // XslCompiledTransform xslttransform = new XslCompiledTransform();
            MvpXslTransform xslttransform = new MvpXslTransform();
            xslttransform.Load(XsltPath);
            MultiXmlTextWriter multiWriter = new MultiXmlTextWriter(Path.Combine(DirectoryPath, RootFile), new UTF8Encoding(false))
            {
                Formatting = Formatting.Indented
            };
            
            xslttransform.Transform(new XmlInput(doc), null, new XmlOutput(multiWriter));
        }

        private void CreateXml(AbstractSchemaItem menuSublist)
        {
            foreach (AbstractSchemaItem menuitem in menuSublist.ChildItems)
            {
                WriteStartElement("Menuitem", menuitem.NodeText,menuitem.Id.ToString(),menuitem.GetType().Name);
                string doc = documentation.GetDocumentation(menuitem.Id, DocumentationType.USER_LONG_HELP);
                WriteElement("documentation", doc);
                if (menuitem is FormReferenceMenuItem formItem)
                {
                    FormControlSet form = formItem.Screen;
                    DataSet dataset = new DatasetGenerator(false).CreateDataSet(form.DataStructure);
                    MakeXml(form.ChildItems[0] as ControlSetItem, form, dataset,null);
                }
                CreateXml(menuitem);
                WriteEndElement();
            }
        }

        private void WriteStartElement(string element)
        {
            Xmlwriter.WriteStartElement(element);
        }

        private void WriteStartElement(string element,string displayName, string id , string typeitem)
        {
            Xmlwriter.WriteStartElement(element);
            Xmlwriter.WriteAttributeString("DisplayName", displayName);
            Xmlwriter.WriteAttributeString("Id", id);
            Xmlwriter.WriteAttributeString("Type", typeitem);
        }

        private void WriteElement(string caption,string description)
        {
            if (!string.IsNullOrEmpty(description))
            { 
                Xmlwriter.WriteElementString(caption, description);
            }
        }

        private void WriteEndElement()
        {
            Xmlwriter.WriteEndElement();
        }

        private void CloseXml()
        {
            Xmlwriter.WriteEndDocument();
            Xmlwriter.Flush();
        }
    }
}