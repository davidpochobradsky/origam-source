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

using System.Collections.Generic;
using NUnit.Framework;
using Origam.DA.Service.MetaModelUpgrade;
using Origam.DA.ServiceTests.MetaModelUpgraderTests;

namespace Origam.DA.ServiceTests.ScriptContainersTests
{
    [TestFixture]
    public class PackageScriptContainerTests: ClassUpgradeTestBase
    {
        [Test]
        public void ShouldRenameSchemaExtensionToPackage()
        {
            XFileData xFileData = LoadFile("Security_V5.0.0_.origamPackage");
            var modelUpGrader = new MetaModelUpGrader(new NullFileWriter());
            modelUpGrader.Upgrade(xFileData);
        }
    }
}