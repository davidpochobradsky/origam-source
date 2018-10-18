﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using Origam.DA.Service;
using Origam.Gui.Win;
using Origam.Schema;
using Origam.Schema.GuiModel;
using Origam.Schema.MenuModel;
using Origam.Workbench.Services;

namespace Origam.MdExporter
{
    class XmlCreate
    {
        public string Xmlpath { get; set; }
        public XmlTextWriter Xmlwriter { get; set; }
        IDocumentationService documentation;

        public XmlCreate(string xmlpath,string filename, Workbench.Services.IDocumentationService documentation)
        {
            this.Xmlpath = xmlpath;
            this.documentation = documentation;

            Xmlwriter = new XmlTextWriter(xmlpath + "\\" + filename, Encoding.UTF8)
            {
                Formatting = Formatting.Indented
            };
            Xmlwriter.WriteStartDocument();
            Xmlwriter.WriteComment("This file is generated by the program.");
        }

        internal void CreateXml(AbstractSchemaItem menuitem)
        {
            WriteElement("menuitem", menuitem.Name);
            makeXml(menuitem);
            WriteEndElement();
            //closeXml();
        }

       
            private void makeXml(AbstractSchemaItem menuitem)
        {
            
            string caption = "";
            string gridCaption = "";
            string bindingMember = "";
            string panelTitle = "";
            int tabIndex = 0;
            string dataMember = "";

            FormReferenceMenuItem formItem = menuitem as FormReferenceMenuItem;
            if (formItem != null)
            {
                FormControlSet form = formItem.Screen;

                
                ControlSetItem control = (ControlSetItem)form.ChildItems[0];
               

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
                    var fstructura = form.DataStructure.ChildItems.ToList();
                   
                    DataSet dataset = new DatasetGenerator(false).CreateDataSet(form.DataStructure);
                    DataTable table = dataset.Tables[FormGenerator.FindTableByDataMember(dataset, dataMember)];

                    if (!table.Columns.Contains(bindingMember)) throw new Exception("Field '" + bindingMember + "' not found in a data structure for the form '" + control.RootItem.Path + "'");

                    if (caption == "") caption = table.Columns[bindingMember].Caption;
                    Guid id = (Guid)table.Columns[bindingMember].ExtendedProperties["Id"];

                    WriteElement("title",caption);
                        DocumentationComplete doc = documentation.LoadDocumentation(id);
                        if (doc != null)
                        {
                        var doctable = doc.Tables;

                        //var docfull = doc.Documentation;
                        //    var dataclumn = docfull.DataColumn;
                        WriteElement("description",caption);
                        }
                        //WriteEndElement();

                }

                    ArrayList sortedControls;

                if (control.ControlItem.IsComplexType)
                {
                    if (panelTitle != "")
                    {
                        WriteElement("Section ",panelTitle );
                    }

                    //AbstractDataEntity entity = GetEntity(ps, dataMember, dataset);
                    //writer.WriteStartElement("p");
                    //writer.WriteString("Entity ");
                    //DocTools.WriteElementLink(writer, entity, new DocEntity(null).FilterName);
                    //writer.WriteEndElement();

                    sortedControls = control.ControlItem.PanelControlSet.ChildItems[0].ChildItemsByType(ControlSetItem.ItemTypeConst);

                }
                else
                {
                    sortedControls = control.ChildItemsByType(ControlSetItem.ItemTypeConst);
                }

                sortedControls.Sort(new ControlSetItemComparer());

                foreach (ControlSetItem subControl in sortedControls)
                {
                    WriteElement("subControl", subControl.Name);
                    makeXml(subControl);
                    WriteEndElement();
                }
            }
           }

        internal void WriteElement(string v)
        {
            Xmlwriter.WriteStartElement(v);
        }

        public void WriteElement(string caption,string description)
        {
            Xmlwriter.WriteStartElement(caption);
            Xmlwriter.WriteElementString(caption, description);
        }

        public void WriteEndElement()
        {
            Xmlwriter.WriteEndElement();
        }

        public void CloseXml()
        {
            Xmlwriter.WriteEndDocument();
            Xmlwriter.Flush();
            Xmlwriter.Close();
        }
    }
}
