#region license
/*
Copyright 2005 - 2021 Advantage Solutions, s. r. o.

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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MoreLinq;
using Origam.DA.Service.FileSystemModeCheckers;
using Origam.Extensions;

namespace Origam.DA.Service.FileSystemModelCheckers
{
    public class DuplicateIdChecker: IFileSystemModelChecker
    {
        private DirectoryInfo topDirectory;
        private readonly DuplicateTracker duplicateTracker = new DuplicateTracker();

        public DuplicateIdChecker(FilePersistenceProvider filePersistenceProvider)
        {
            topDirectory = filePersistenceProvider.TopDirectory;
        }

        public ModelErrorSection GetErrors()
        {
           topDirectory
               .GetAllFilesInSubDirectories()
               .Where(OrigamFile.IsPersistenceFile)
               .ForEach(PuIdsToDuplicateTracker);

           List<string> errorMessages = duplicateTracker    
               .GetDuplicates()
               .Select(duplicate =>
               {
                   string filePaths = string.Join(
                       "\n", duplicate.Files.Select(file => $"\"file://{file.FullName}\""));
                   return $"Object with Id: {duplicate.ObjectId} is defined in more than one file:\n{filePaths}";
               })
               .ToList();

           return new ModelErrorSection("Duplicate Ids", errorMessages);
        }

        private void PuIdsToDuplicateTracker(FileInfo file)
        {
            string text = File.ReadAllText(file.FullName);

            var idRegex = "x:id=\"([0-9A-Fa-f]{8}[-]([0-9A-Fa-f]{4}[-]){3}[0-9A-Fa-f]{12})\"";
            foreach (Match match in Regex.Matches(text, idRegex))
            {
                Guid id = Guid.Parse(match.Groups[1].Value);
                duplicateTracker.Add(id, file);
            }
        }
    }

    class DuplicateTracker
    {
        private readonly Dictionary<Guid, List<FileInfo>> idToFilesDictionary 
            = new Dictionary<Guid, List<FileInfo>>();

        public void Add(Guid id, FileInfo file)
        {
            if (!idToFilesDictionary.ContainsKey(id))
            {
                idToFilesDictionary.Add(id, new List<FileInfo>());
            }

            idToFilesDictionary[id].Add(file);
        }

        public IEnumerable<DuplicateInfo> GetDuplicates()
        {
            return idToFilesDictionary
                .Where(pair => pair.Value.Count > 1)
                .Select(pair => new DuplicateInfo(pair.Key, pair.Value));
        }
    }

    class DuplicateInfo
    {
        public DuplicateInfo(Guid objectId, List<FileInfo> files)
        {
            ObjectId = objectId;
            Files = files;
        }

        public Guid ObjectId { get;  }
        public List<FileInfo> Files { get;}
    }
}