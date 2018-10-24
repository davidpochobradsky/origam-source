﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;
using NUnit.Framework;
using Origam.DA;
using Origam.DA.ObjectPersistence;
using Origam.DA.ObjectPersistence.Providers;
using Origam.DA.Service;
using Origam.DA.Service_net2Tests;
using Origam.Workbench.Services;



namespace Origam.Workbench.ServicesTests
{
    [TestFixture]
    public class FileStorageDocumentationServiceTests
    {
        [Test]
        public void ShouldAddTwoDocumenattionItems()
        {
            var sut = GetFileStorageDocumentationService(WritingTestFiles);
            DocumentationComplete dataSet = GetTestDataSet("inputDataSet_2Items.xml");
            sut.SaveDocumentation(dataSet);
            XmlDocument xmlDocument = GetOutDocument();
            Assert.That(xmlDocument.FirstChild.ChildNodes, Has.Count.EqualTo(2));
        }

        [Test]
        public void ShouldUpdateOneDocumentationItem()
        {
            var sut = GetFileStorageDocumentationService(WritingTestFiles);
            DocumentationComplete dataSet = GetTestDataSet("inputDataSet_1UpdatedItem.xml");
            sut.SaveDocumentation(dataSet);
            XmlDocument xmlDocument = GetOutDocument();
            Assert.That(xmlDocument.FirstChild.ChildNodes, Has.Count.EqualTo(2));

            XmlNode updatedNode = xmlDocument.FirstChild.ChildNodes[0];
            Assert.That(updatedNode.ChildNodes[0].InnerText == "Updated text");
        }

        [Test]
        public void ShouldReadDatSet()
        {
            var sut = GetFileStorageDocumentationService(ReadingTestFiles);
            DocumentationComplete loadedSet =
                sut.LoadDocumentation(new Guid("df7c2a53-c56a-426a-b748-08e656ae46db"));
            
            Assert.That(loadedSet.Tables[0].Rows, Has.Count.EqualTo(2));
        }

        [Test]
        public void ShouldLoadDocumennationOfSpecifiedType()
        {
            var sut = GetFileStorageDocumentationService(ReadingTestFiles);
            string loadedDoc = sut.GetDocumentation(
                new Guid("df7c2a53-c56a-426a-b748-08e656ae46db"),
                DocumentationType.USER_SHORT_HELP);
            
            Assert.That(loadedDoc == "Short help");
        }
        
        [Test]
        public void ShoudThrowBecauseCategoryNameIsWrong()
        {
            var sut = GetFileStorageDocumentationService(
                GetDirectory("WrongCategoryName"));
            
            Assert.Throws<ArgumentException>(() =>
                {
                    sut.LoadDocumentation(
                        new Guid("df7c2a53-c56a-426a-b748-08e656ae46db"));
                });
        }
        
        [Test]
        public void ShoudThrowBecauseAStringCannotBeParsedToGuid()
        {
            var sut = GetFileStorageDocumentationService(
                GetDirectory("WrongGuid"));
            
            Assert.Throws<ArgumentException>(() =>
            {
                sut.LoadDocumentation(
                    new Guid("df7c2a53-c56a-426a-b748-08e656ae46db"));
            });
        }
        
        [Test]
        public void ShoudThrowBecauseANodeNameIsWrong()
        {
            var sut = GetFileStorageDocumentationService(
                GetDirectory("WrongNodeName"));
            
            Assert.Throws<ArgumentException>(() =>
            {
                sut.LoadDocumentation(
                    new Guid("df7c2a53-c56a-426a-b748-08e656ae46db"));
            });
        }

        private DirectoryInfo GetDirectory(string dirName)
        {
            string pathToDir =
                Path.Combine(ReadingTestFiles.FullName, dirName);
            var directoryInfo = new DirectoryInfo(pathToDir);
            return directoryInfo;
        }

        private FileStorageDocumentationService GetFileStorageDocumentationService(
            DirectoryInfo starageDir)
        {
            var mockFileProvider = new MockFileProvider(starageDir);
            var fileStorageDocumentationService =
                new FileStorageDocumentationService(mockFileProvider, 
                    new FileEventQueue(new FilePersistenceIndex(new OrigamPathFactory(null))
                        ,new NullWatchDog()));
            return fileStorageDocumentationService;
        }

        private XmlDocument GetOutDocument()
        {
            string outFilePath = Path.Combine(WritingTestFiles.FullName, ".origamDoc");
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(outFilePath);
            return xmlDocument;
        }

        private DocumentationComplete GetTestDataSet(string name)
        {
            string testInputPath =
                Path.Combine(WritingTestFiles.FullName, name);
            var dataSet = new DocumentationComplete();
            dataSet.ReadXml(testInputPath);
            return dataSet;
        }

        protected static readonly DirectoryInfo projectDir =
            new DirectoryInfo(TestContext.CurrentContext.TestDirectory)
                .Parent
                .Parent;

        protected DirectoryInfo WritingTestFiles {
            get
            {
                string relativeToFilesDir = "WritingTestFiles";
            
                string path = Path.Combine(projectDir.FullName, relativeToFilesDir);
                Directory.CreateDirectory(path);
                return new DirectoryInfo(path);
            }
        }
        protected DirectoryInfo ReadingTestFiles {
            get
            {
                string relativeToFilesDir = "ReadingTestFiles";
            
                string path = Path.Combine(projectDir.FullName, relativeToFilesDir);
                Directory.CreateDirectory(path);
                return new DirectoryInfo(path);
            }
        }
    }

    internal class MockFileProvider: IFilePersistenceProvider
    {
        private readonly DirectoryInfo testDir;

        public MockFileProvider(DirectoryInfo testDir)
        {
            this.testDir = testDir;
        }
        
        public DirectoryInfo GetParentPackageDirectory(Guid itemId) =>
            testDir;

        public bool Has(Guid id) => throw new NotImplementedException();
        public DirectoryInfo TopDirectory { get; }
        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public event EventHandler InstancePersisted;
        public void OnInstancePersisted(object sender)
        {
            throw new NotImplementedException();
        }

        public ICompiledModel CompiledModel { get; set; }
        public void RefreshInstance(IPersistent persistentObject)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromCache(IPersistent instance)
        {
            throw new NotImplementedException();
        }

        public List<T> RetrieveList<T>(IDictionary<string, object> filter = null)
        {
            throw new NotImplementedException();
        }

        public List<T> RetrieveListByType<T>(string itemType)
        {
            throw new NotImplementedException();
        }

        public List<T> RetrieveListByPackage<T>(Guid packageId)
        {
            throw new NotImplementedException();
        }

        public List<T> FullTextSearch<T>(string text)
        {
            throw new NotImplementedException();
        }

        public void Persist(IPersistent obj)
        {
            throw new NotImplementedException();
        }

        public string DebugInfo()
        {
            throw new NotImplementedException();
        }

        public void DebugShow()
        {
            throw new NotImplementedException();
        }

        public string DebugChangesInfo()
        {
            throw new NotImplementedException();
        }

        public void DebugChangesShow()
        {
            throw new NotImplementedException();
        }

        public void FlushCache()
        {
            throw new NotImplementedException();
        }

        public void DeletePackage(Guid packageId)
        {
            throw new NotImplementedException();
        }

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void EndTransaction()
        {
            throw new NotImplementedException();
        }

        public object RetrieveValue(Guid instanceId, Type parentType, string fieldName)
        {
            throw new NotImplementedException();
        }

        public void RestrictToLoadedPackage(bool b)
        {
            throw new NotImplementedException();
        }

        public ILocalizationCache LocalizationCache { get; }

        public List<T> RetrieveListByGroup<T>(Key primaryKey)
        {
            throw new NotImplementedException();
        }

        public List<T> RetrieveListByParent<T>(Key primaryKey, string parentTableName,
            string childTableName, bool useCache)
        {
            throw new NotImplementedException();
        }

        public object RetrieveInstance(Type type, Key primaryKey, bool useCache,
            bool throwNotFoundException)
        {
            throw new NotImplementedException();
        }

        public object RetrieveInstance(Type type, Key primaryKey, bool useCache)
        {
            throw new NotImplementedException();
        }

        public object RetrieveInstance(Type type, Key primaryKey)
        {
            throw new NotImplementedException();
        }
    }
}