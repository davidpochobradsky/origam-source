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

using Origam.DA.Common;
using System;
using Origam.DA.ObjectPersistence;

namespace Origam.Schema.TestModel
{
	/// <summary>
	/// Summary description for TestCase.
	/// </summary>
	[SchemaItemDescription("Test Case", 26)]
    [ClassMetaVersion("6.0.0")]
	public class TestCase : AbstractSchemaItem, ISchemaItemFactory
	{
		public const string CategoryConst = "TestCase";

		public TestCase() : base() {}

		public TestCase(Guid schemaExtensionId) : base(schemaExtensionId) {}

		public TestCase(Key primaryKey) : base(primaryKey)	{}

		#region Overriden AbstractSchemaItem Members
		
		[EntityColumn("ItemType")]
		public override string ItemType
		{
			get
			{
				return CategoryConst;
			}
		}

		public override string Icon
		{
			get
			{
				return "26";
			}
		}

		public override bool CanMove(Origam.UI.IBrowserNode2 newNode)
		{
			// can move test cases between scenarios
			if(newNode is TestScenario)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		#endregion

		#region ISchemaItemFactory Members

		public override Type[] NewItemTypes
		{
			get
			{
				return new Type[] {typeof(TestCaseAlternative), typeof(TestCaseStep)};
			}
		}

		public override AbstractSchemaItem NewItem(Type type, Guid schemaExtensionId, SchemaItemGroup group)
		{
			AbstractSchemaItem item;

			if(type == typeof(TestCaseAlternative))
			{
				item = new TestCaseAlternative(schemaExtensionId);
				item.Name = "NewTestCaseAlternative";
			}
			else if(type == typeof(TestCaseStep))
			{
				item = new TestCaseStep(schemaExtensionId);
				item.Name = "NewTestCaseCheck";
			}
			else
				throw new ArgumentOutOfRangeException("type", type, ResourceUtils.GetString("ErrorTestCaseAlternative"));

			item.Group = group;
			item.PersistenceProvider = this.PersistenceProvider;
			this.ChildItems.Add(item);
			return item;
		}

		#endregion

		#region Properties
		private string _role;

		[EntityColumn("LS01")]
		public string Role
		{
			get
			{
				return _role;
			}
			set
			{
				_role = value;
			}
		}
		#endregion

	}
}
