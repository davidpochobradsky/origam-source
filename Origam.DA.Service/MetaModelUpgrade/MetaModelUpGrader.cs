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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using MoreLinq;
using Origam.DA.Common;
using Origam.Extensions;

namespace Origam.DA.Service.MetaModelUpgrade
{
    public class MetaModelUpGrader
    {
        private readonly ScriptContainerLocator scriptLocator;
        private readonly IFileWriter fileWriter;

        private readonly Version firstVersion = new Version("6.0.0");
        public MetaModelUpGrader(Assembly scriptAssembly, IFileWriter fileWriter)
        {
            scriptLocator = new ScriptContainerLocator(scriptAssembly);
            this.fileWriter = fileWriter;
        }

        public MetaModelUpGrader(IFileWriter fileWriter)
        {
            this.fileWriter = fileWriter;
            scriptLocator = new ScriptContainerLocator(GetType().Assembly);
        }

        public MetaModelUpGrader()
        {
            fileWriter = new FileWriter();
            scriptLocator = new ScriptContainerLocator(GetType().Assembly);
        }
        
        public void Upgrade(XFileData xFileData)
        {
            bool isVersion5 = xFileData.Document.FileElement
                .Attributes()
                .Any(attr => attr.Value == "http://schemas.origam.com/5.0.0/model-element");
            if (isVersion5)
            {
                new Version6UpGrader(scriptLocator ,xFileData.Document).Run();
            }
            xFileData.Document
                .ClassNodes
                .ToArray().ForEach(classNode => Upgrade(classNode, xFileData));
            
            xFileData.Document.FixNamespaces();
            WriteToFile(xFileData);
        }

        private void WriteToFile(XFileData xFileData)
        {
            string upgradedXmlString = OrigamDocumentSorter
                .CopyAndSort(xFileData.Document.XDocument)
                .ToBeautifulString();

            fileWriter.Write(xFileData.File, upgradedXmlString);
        }

        private void Upgrade(XElement classNode, XFileData xFileData)
        {
            IEnumerable<OrigamNameSpace> origamNameSpaces = GetOrigamNameSpaces(classNode);
            
            string nodeClass = OrigamNameSpace.Create(classNode?.Name.NamespaceName).FullTypeName;
            Versions persistedClassVersions = new Versions(origamNameSpaces);
            Versions currentClassVersions =
                Versions.GetCurrentClassVersions(nodeClass, persistedClassVersions);
            
            foreach (var pair in currentClassVersions)
            {
                string className = pair.Key;
                Version currentVersion = pair.Value;
                if (currentVersion == firstVersion)
                {
                    continue;
                }

                if (!persistedClassVersions.ContainsKey(className))
                {
                    RunUpgradeScripts(classNode, xFileData, className,
                        firstVersion, currentVersion);
                    continue;
                }

                if (persistedClassVersions[className] > currentVersion)
                {
                    throw new Exception($"Class version written in persisted object is greater than current version of the class. This should never happen, please check version of {classNode.Name} in {xFileData.File.FullName}");
                }

                if (persistedClassVersions[className] < currentVersion)
                {
                    RunUpgradeScripts(classNode, xFileData, className,
                        persistedClassVersions[className], currentVersion);
                }
            }
        }

        private static IEnumerable<OrigamNameSpace> GetOrigamNameSpaces(XElement classNode)
        {
            return classNode.Attributes()
                .Select(attr => attr.Name.NamespaceName)
                .Where(name => name != OrigamFile.ModelPersistenceUri)
                .Distinct()
                .Select(OrigamNameSpace.Create);
        }

        private void RunUpgradeScripts(XElement classNode,
            XFileData xFileData, string className,
            Version persistedClassVersion,
            Version currentClassVersion)
        {
            var upgradeScriptContainer = scriptLocator.FindByTypeName(className);
            upgradeScriptContainer.Upgrade(xFileData.Document, classNode, persistedClassVersion, currentClassVersion);
        }
    }
    public class Version6UpGrader
    {
        private readonly ScriptContainerLocator scriptLocator;
        private readonly OrigamXDocument xDocument;
        private static XNamespace oldPersistenceNamespace = "http://schemas.origam.com/1.0.0/model-persistence";
        private static XNamespace newPersistenceNamespace = "http://schemas.origam.com/model-persistence/1.0.0";

        public Version6UpGrader(ScriptContainerLocator scriptLocator,
            OrigamXDocument xDocument)
        {
            this.scriptLocator = scriptLocator;
            this.xDocument = xDocument;
        }

        public void Run()
        {
            RemoveOldDocumentNamespaces();

            foreach (XElement node in xDocument.ClassNodes)
            {
                IPropertyToNamespaceMapping namespaceMapping = GetNamespaceMapping(node);
                namespaceMapping.AddNamespacesToDocument(xDocument);
                
                RemoveTypeAttribute(node);
                node.Name = namespaceMapping.NodeNamespace.GetName(node.Name.LocalName);
                CopyAttributes(node, namespaceMapping);
            }
        }

        private string Version6NamespaceMapper(Type type)
        {
            string xmlNameSpaceWithCurrentVersion = XmlNamespaceTools.GetXmlNameSpace(type);
            return 
                string.Join(
                    "/", 
                    MoreEnumerable.SkipLast(xmlNameSpaceWithCurrentVersion
                                .Split("/"), 1)
                            .Concat(new []{"6.0.0"})
                    );
        }

        private static void CopyAttributes(XElement node,
            IPropertyToNamespaceMapping namespaceMapping)
        {
            List<XAttribute> atList = node
                .Attributes()
                .Where(attr => attr.Name.LocalName != "xmlns")
                .ToList();  
            
            node.Attributes().Remove();  
            foreach (XAttribute attribute in atList){
                XNamespace nameSpace = attribute.Name.Namespace == oldPersistenceNamespace
                    ? newPersistenceNamespace
                    : namespaceMapping.GetNamespaceByXmlAttributeName(attribute.Name.LocalName);
                node.Add(new XAttribute(nameSpace.GetName(attribute.Name.LocalName), attribute.Value));
            }
        }

        private IPropertyToNamespaceMapping GetNamespaceMapping(XElement node)
        {
            XName name = oldPersistenceNamespace.GetName("type");
            XAttribute typeAttribute = node?.Attribute(name);
            if (string.IsNullOrWhiteSpace(typeAttribute?.Value))
            {
                throw new Exception(
                    $"Cannot get type from node: {node?.Name} in \n{xDocument}");
            }
            
            Type type = Reflector.GetTypeByName(typeAttribute.Value);
            bool classIsDearOrRenamed = type == null;
            if (classIsDearOrRenamed)
            {
                var scriptContainer = scriptLocator.FindByTypeName(typeAttribute.Value);
                type = Reflector.GetTypeByName(scriptContainer.FullTypeName);
                
                bool classIsDead = type == null;
                if (classIsDead)
                {
                    return new DeadClassPropertyToNamespaceMapping(
                        scriptContainer.FullTypeName,
                        new Version(6,0,0));
                }
            }

            return new PropertyToNamespaceMapping(
                instanceType: type, 
                xmlNamespaceMapper: Version6NamespaceMapper) ;
        }  
        
        private void RemoveTypeAttribute(XElement node)
        {
            XName name = oldPersistenceNamespace.GetName("type");
            XAttribute typeAttribute = node?.Attribute(name);
            typeAttribute?.Remove();
        }

        private void RemoveOldDocumentNamespaces()
        {
            xDocument.FileElement
                .Attributes()
                .Where(
                    attr => attr.Value ==
                            "http://schemas.origam.com/5.0.0/model-element" ||
                            attr.Value == "http://schemas.origam.com/1.0.0/package")
                .Remove();
            xDocument.FileElement.Name = newPersistenceNamespace.GetName(xDocument.FileElement.Name.LocalName);
            xDocument.FileElement.Attribute(XNamespace.Xmlns + "x").Value = newPersistenceNamespace.ToString();
        }
    }

    public class UpgradeProgressInfo
    {
        public int TotalFiles { get;}
        public int FilesDone { get; }

        public UpgradeProgressInfo(int totalFiles, int filesDone)
        {
            TotalFiles = totalFiles;
            FilesDone = filesDone;
        }

    }
}