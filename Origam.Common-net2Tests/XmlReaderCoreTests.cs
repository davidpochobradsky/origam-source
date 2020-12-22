﻿using NUnit.Framework;
using System;
using System.Data;
using System.IO;
using System.Xml;

namespace Origam.Common_net2Tests
{
    [TestFixture]
    public class XmlReaderCoreTests
    {
        [Test]
        public void ShouldRemoveEmptyAttributes()
        {
            string xml = 
                "<root>" +
                 "    <tabulka cislo2=\"12222\" text=\"\" cislo=\"\" >" +
                 "        <cislo3>1</cislo3>" +
                 "    </tabulka>" +
                 "    <tabulka cislo2=\"\" text=\"eee\" cislo=\"\" cislo3=\"nic2\" />" +
                 "    <cislo3></cislo3>" +
                 "</root>";
            
            var tr = new StringReader(xml);
            XmlReaderCore xr = new XmlReaderCore(new XmlTextReader(tr));
            DataSet ds = new DataSet("root");
            var dt = ds.Tables.Add("tabulka");
            var dtCislo = dt.Columns.Add("cislo", typeof(int));
            dtCislo.ColumnMapping = MappingType.Attribute;
            dtCislo.AllowDBNull = true;
            var dtText = dt.Columns.Add("text", typeof(string));
            dtText.ColumnMapping = MappingType.Attribute;
            var dtText2 = dt.Columns.Add("cislo3", typeof(int));
            dtText2.ColumnMapping = MappingType.Element;
            var dtcislo2 = dt.Columns.Add("cislo2", typeof(int));
            dtcislo2.ColumnMapping = MappingType.Attribute;
            ds.ReadXml(xr);
            Assert.That(ds.Tables[0].Rows[0]["cislo2"], Is.EqualTo(12222));
            Assert.IsTrue(ds.Tables[0].Rows[0]["cislo"] == DBNull.Value);
            Assert.IsTrue(ds.Tables[0].Rows[0]["text"] == DBNull.Value);
            Assert.That(ds.Tables[0].Rows[0]["cislo3"], Is.EqualTo(1));

            Assert.IsTrue(ds.Tables[0].Rows[1]["cislo"] == DBNull.Value);
            Assert.IsTrue(ds.Tables[0].Rows[1]["cislo2"] == DBNull.Value);
            Assert.IsTrue(ds.Tables[0].Rows[1]["cislo3"] == DBNull.Value);
            Assert.That(ds.Tables[0].Rows[1]["text"], Is.EqualTo("eee"));
        }
        
        [Test]
        public void ShouldRemoveChildElement()
        {
            string xml = 
                "<ROOT>\n" +
                "    <NewTable1 Id=\"4a183ee2-edf4-4481-9f2c-561ff73a0944\">\n" +
                "        <Id2></Id2>\n" +
                "        <Name>Some Name</Name>\n" +
                "    </NewTable1>\n" +
                "</ROOT>";

            XmlReaderCore sut = new XmlReaderCore(new XmlTextReader(new StringReader(xml)));

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(sut);
            
            XmlNode root = xmlDocument.FirstChild;
            Assert.That(root.Name, Is.EqualTo("ROOT"));
            Assert.That(root.FirstChild.Name, Is.EqualTo("NewTable1"));
            Assert.That(root.FirstChild.ChildNodes, Has.Count.EqualTo(1));
            Assert.That(root.FirstChild.ChildNodes[0].Name, Is.EqualTo("Name"));
            Assert.That(root.FirstChild.ChildNodes[0].InnerText, Is.EqualTo("Some Name"));
        }
    }
}
