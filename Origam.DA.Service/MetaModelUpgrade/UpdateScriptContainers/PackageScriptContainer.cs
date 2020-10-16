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
using System.Xml.Linq;
using Origam.Schema;

namespace Origam.DA.Service.MetaModelUpgrade.UpdateScriptContainers
{
    class PackageScriptContainer : UpgradeScriptContainer
    {
        public override string FullTypeName { get; } = typeof(Package).FullName;
        public override string[] OldPropertyXmlNames { get; }
        public override List<string> OldFullTypeNames { get; } 
            = new List<string>{"Origam.Schema.SchemaExtension"};

        public PackageScriptContainer() 
        {
            upgradeScripts.Add(new UpgradeScript(
                new Version("6.0.0"), 
                new Version("6.1.0"),
                (node, doc) =>
                {
                    // class name will be changed automatically
                }));
        }
    }    
}