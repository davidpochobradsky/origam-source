using System;
using System.Linq;
using System.Xml.Linq;
using Origam.Extensions;

namespace Origam.DA.Service.NamespaceMapping
{
    class DeadClassPropertyToNamespaceMapping : IPropertyToNamespaceMapping
    {
        private string xmlNamespaceName;    

        public DeadClassPropertyToNamespaceMapping(string fullTypeName, Version version)
        {
            string typeName = fullTypeName.Split(".").Last();
            NodeNamespace =  XmlNamespaceTools.GetXmlNamespace(fullTypeName, version);
            xmlNamespaceName = XmlNamespaceTools.GetXmlNamespaceName(typeName);
        }

        public XNamespace NodeNamespace { get; }

        public XNamespace GetNamespaceByXmlAttributeName(string xmlAttributeName)
        {
            return NodeNamespace;
        }

        public void AddNamespacesToDocumentAndAdjustMappings(OrigamXDocument document)
        {
            xmlNamespaceName = document.AddNamespace(
                nameSpaceShortCut: xmlNamespaceName, 
                nameSpace: NodeNamespace.ToString());
        }
    }
}