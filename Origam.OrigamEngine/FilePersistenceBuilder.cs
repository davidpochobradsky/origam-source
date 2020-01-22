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
using Origam.Workbench.Services;
using System.Collections.Generic;
using Origam.DA;
using Origam.DA.ObjectPersistence;
using Origam.DA.ObjectPersistence.Providers;
using Origam.DA.Service;
using Origam.Schema;

namespace Origam.OrigamEngine
{
    public class FilePersistenceBuilder : IPersistenceBuilder
    {
        private static FilePersistenceService persistenceService;
        
        public IDocumentationService GetDocumentationService() =>
            new FileStorageDocumentationService(
                (IFilePersistenceProvider) persistenceService.SchemaProvider,
                persistenceService.FileEventQueue);

        public IPersistenceService GetPersistenceService() => 
            GetPersistenceService(watchFileChanges: true,
                checkRules: true);

        public IPersistenceService GetPersistenceService(bool watchFileChanges,
            bool checkRules)
        {
            persistenceService = CreateNewPersistenceService(watchFileChanges,
                checkRules);
            return persistenceService;
        }

        public FilePersistenceService CreateNewPersistenceService(bool watchFileChanges,
            bool checkRules)
        {
            List<ElementName> defaultFolders = new List<ElementName>
            {
                ElementNameFactory.Create(typeof(SchemaExtension)),
                ElementNameFactory.Create(typeof(SchemaItemGroup))
            };

            return new FilePersistenceService(
                defaultFolders: defaultFolders,
                watchFileChanges: watchFileChanges,
                checkRules: checkRules);
        }

        public FilePersistenceService CreateNoBinFilePersistenceService()
        {
            List<ElementName> defaultFolders = new List<ElementName>
            {
                ElementNameFactory.Create(typeof(SchemaExtension)),
                ElementNameFactory.Create(typeof(SchemaItemGroup))
            };

            return new FilePersistenceService(
                defaultFolders: defaultFolders,
                watchFileChanges: false,
                useBinFile: false);
        }

        public static void Clear()
        {
            persistenceService = null;
        }
    }
}
