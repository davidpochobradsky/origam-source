﻿using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Origam.DA.ObjectPersistence;
using Origam.DA.ObjectPersistence.Providers;
using Origam.Extensions;

namespace Origam.DA.Service
{
    internal class InstanceWriter
    {
        private readonly ExternalFileManger externalFileManger;
        private readonly XmlDocument xmlDocument;


        public InstanceWriter(ExternalFileManger externalFileManger, XmlDocument xmlDocument)
        {
            this.externalFileManger = externalFileManger;
            this.xmlDocument = xmlDocument;
        }

        public void Write(IFilePersistent instance, ElementName elementName)
        {
            XmlElement elementToWriteTo = GetElementToWriteTo(instance, elementName);
            bool isLocalChild = elementToWriteTo.ParentNode.GetDepth() != 1;
            WriteToNode(elementToWriteTo, instance, isLocalChild);
        }

        private XmlElement GetElementToWriteTo(IFilePersistent instance, ElementName elementName)
        {
            XmlElement existingElement  = (XmlElement)xmlDocument    
                .GetAllNodes()
                .FirstOrDefault(x => XmlUtils.ReadId(x) == instance.Id);
            if (existingElement == null)
            {
                return FindElementToWriteTo(xmlDocument, instance, elementName, 0);
            }

            Guid? parentNodeId = XmlUtils.ReadId(existingElement.ParentNode);
            bool parentNodeIdInXmlDiffersFromParentIdInInstance =
                parentNodeId.HasValue && parentNodeId.Value != instance.FileParentId ||
                !parentNodeId.HasValue && instance.FileParentId != Guid.Empty;
            if (parentNodeIdInXmlDiffersFromParentIdInInstance)
            {
                
                MoveElementToNewLocation(existingElement, instance, elementName);
            }
            return existingElement;
        }

        private void MoveElementToNewLocation(XmlElement element,
            IFilePersistent instance, ElementName elementName)
        {
            element.ParentNode?.RemoveChild(element);
            XmlElement newElement = FindElementToWriteTo(xmlDocument, instance, elementName, 0);
            XmlNode parentNode = newElement.ParentNode;
            parentNode.RemoveChild(newElement);
            parentNode.AppendChild(element);
        }

        private XmlElement FindElementToWriteTo(XmlNode node, IFilePersistent instance,
            ElementName elementName, int depth)
        {
            Guid? parentId = XmlUtils.ReadId(node);
            foreach (XmlNode child in node.ChildNodes)
            {
                XmlElement element = child as XmlElement;
                Guid? id = XmlUtils.ReadId(child);
                if (element != null && id.HasValue && id.Value == instance.Id)
                {
                    return element;
                }
                else
                {
                    XmlElement foundEl = FindElementToWriteTo(child, instance, elementName, depth+1);
                    if (foundEl != null) return foundEl;
                }
            }
            if ((parentId.HasValue && parentId.Value == instance.FileParentId)
                || depth == 0)
            {
                if (depth == 0)
                {
                    node = node.LastChild;
                }
                // node does not exist, we add
                XmlElement element = node.OwnerDocument.CreateElement(
                    elementName.XmlElementName, elementName.XmlNamespace);
                node.AppendChild(element);
                return element;
            }
            return null;
        }
            
        private void WriteToNode(XmlElement node, IFilePersistent instance, 
            bool localChild)
        {
            node.RemoveAllAttributes();
            // Set all the remaining properties
            WriteXmlAttributes(node, instance);
            // references
            WriteXmlReferenceAttributes(node, instance);
            WriteXmlExternalFiles(node, instance);
            // persistence attributes
            node.SetAttribute(OrigamFile.IdAttribute,
                OrigamFile.ModelPersistenceUri.ToString(), instance.PrimaryKey["Id"].ToString());
            if (instance.IsFolder)
            {
                node.SetAttribute(OrigamFile.IsFolderAttribute,
                    OrigamFile.ModelPersistenceUri.ToString(), XmlConvert.ToString(instance.IsFolder));
            }
            if (instance.FileParentId != Guid.Empty && !localChild)
            {
                node.SetAttribute(OrigamFile.ParentIdAttribute,
                    OrigamFile.ModelPersistenceUri, instance.FileParentId.ToString());
            }
            node.SetAttribute(OrigamFile.TypeAttribute,
                OrigamFile.ModelPersistenceUri, instance.GetType().FullName);
        }
            
        private static void WriteXmlReferenceAttributes(XmlElement node,
            IFilePersistent instance)
        {
            IList references =
                Reflector.FindMembers(instance.GetType(), typeof(XmlReferenceAttribute));
            foreach (MemberAttributeInfo mi in references)
            {
                XmlReferenceAttribute attribute = mi.Attribute as XmlReferenceAttribute;
                PropertyInfo mpi = mi.MemberInfo as PropertyInfo;
                FieldInfo mfi = mi.MemberInfo as FieldInfo;
                object value = null;
                if (mpi != null)
                {
                    value = mpi.GetValue(instance);
                } else if (mfi != null)
                {
                    value = mfi.GetValue(instance);
                }
                IFilePersistent persistentValue = value as IFilePersistent;
                if (persistentValue == null && value != null)
                {
                    throw new Exception(
                        $"Reference must be {typeof(IFilePersistent)} interface ({mi.MemberInfo.Name})");
                }
                if (persistentValue != null)
                {
                    string subPath = persistentValue.Path ?? "";
                    if (subPath != "")
                    {
                        subPath += "/";
                    }
                    node.SetAttribute(attribute.AttributeName, attribute.Namespace,
                        persistentValue.RelativeFilePath.Replace("\\", "/")
                        + "#" + subPath
                        + persistentValue.PrimaryKey["Id"]);
                }
            }
        }
            
        private void WriteXmlAttributes(XmlElement node,
            IFilePersistent instance)
        {
            IList members = Reflector.FindMembers(instance.GetType(), typeof(XmlAttributeAttribute));
            foreach (MemberAttributeInfo memberInfo in members)
            {
                XmlAttributeAttribute attribute = (XmlAttributeAttribute)memberInfo.Attribute;
                object value = GetValueToWrite(instance, memberInfo);
                
                if (ShouldBeSkipped(value)) continue;
                if (Guid.Empty.Equals(value)) continue;
                node.SetAttribute(attribute.AttributeName,
                    attribute.Namespace,
                    XmlTools.ConvertToString(value));
            }
        }
            
        private bool ShouldBeSkipped(object value)
        {
            if (ReferenceEquals(value, null)) return true;
            if (value is Enum) return false;
            if (value is bool) return false;
            if (value is string strValue) return string.IsNullOrEmpty(strValue);    
            return value.IsDefault();
        }

        private void WriteXmlExternalFiles(XmlElement node,
            IFilePersistent instance)
        {
            IList references =
                Reflector.FindMembers(instance.GetType(),
                    typeof(XmlExternalFileReference));
            foreach (MemberAttributeInfo mi in references)
            {
                var attribute = (XmlExternalFileReference) mi.Attribute;

                MemberInfo containerMemberInfo = instance.GetType()
                    .GetField(attribute.ContainerName, BindingFlags.Public |
                                                       BindingFlags
                                                           .NonPublic |
                                                       BindingFlags
                                                           .Instance);

                if (containerMemberInfo == null)
                {
                    throw new Exception(
                        $"Could not find field {attribute.ContainerName} in Class {instance.GetType()}");
                }

                object containerObj =
                    ((FieldInfo) containerMemberInfo).GetValue(instance);

                if (!(containerObj is IPropertyContainer))
                {
                    throw new Exception(
                        $"Could not find field \"value\" in field {attribute.ContainerName} of class {instance.GetType()}. Make sure that the field {attribute.ContainerName} exists and is of type {typeof(PropertyContainer<>)}");
                }
                IPropertyContainer container = (IPropertyContainer) containerObj;

                string externalFileLink =
                    externalFileManger.AddAndReturnLink(
                        fieldName: attribute.ContainerName,
                        objectId: (Guid) instance.PrimaryKey["Id"],
                        data: container.GetValue(),
                        fileExtension: attribute.Extension);

                node.SetAttribute(attribute.ContainerName,
                    attribute.Namespace,
                    externalFileLink);
            }
        }
        private static object GetValueToWrite(IFilePersistent instance,
            MemberAttributeInfo mi)
        {
            PropertyInfo propertyInfo = mi.MemberInfo as PropertyInfo;
            FieldInfo fieldInfo = mi.MemberInfo as FieldInfo;
            object value = null;
            if (propertyInfo != null)
            {
                value = propertyInfo.GetValue(instance);
            } else if (fieldInfo != null)
            {
                value = fieldInfo.GetValue(instance);
            }
            return value;
        }
    }
}