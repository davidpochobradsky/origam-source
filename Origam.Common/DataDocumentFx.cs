﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Origam
{
#if !NETSTANDARD
    public class DataDocumentFx : IDataDocument
    {
        private readonly XmlDataDocument xmlDataDocument;

        public DataDocumentFx(DataSet dataSet)
        {
            xmlDataDocument = new XmlDataDocument(dataSet);
        }

        public DataDocumentFx(XmlDocument xmlDoc)
        {
            xmlDataDocument = new XmlDataDocument();
            foreach (XmlNode childNode in xmlDoc.ChildNodes)
            {
                var importNode = xmlDataDocument.ImportNode(childNode, true);
                xmlDataDocument.AppendChild(importNode);
            }
        }

        public XmlDocument Xml => xmlDataDocument;

        public DataSet DataSet => xmlDataDocument.DataSet;
        public void Load(XmlNodeReader xmlNodeReader)
        {
            xmlDataDocument.Load(xmlNodeReader);
        }

        public object Clone()
        {
            return new DataDocumentFx(xmlDataDocument);
        }
    }
#endif
}
