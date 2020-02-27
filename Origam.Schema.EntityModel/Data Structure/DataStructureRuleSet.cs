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

using Origam.DA.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using Origam.DA.ObjectPersistence;
using System.Xml.Serialization;

namespace Origam.Schema.EntityModel
{
	/// <summary>
	/// Summary description for DataQuery.
	/// </summary>
	[SchemaItemDescription("Rule Set", "Rule Sets", "icon_rule-set.png")]
    [HelpTopic("Rule+Sets")]
	[XmlModelRoot(CategoryConst)]
    [ClassMetaVersion("1.0.0")]
	public class DataStructureRuleSet : AbstractSchemaItem, ISchemaItemFactory
	{
		public const string CategoryConst = "DataStructureRuleSet";
        private object _lock = new object();

		public DataStructureRuleSet() : base() {}

		public DataStructureRuleSet(Guid schemaExtensionId) : base(schemaExtensionId) {}

		public DataStructureRuleSet(Key primaryKey) : base(primaryKey)	{}
	
		#region Public Methods
		public ArrayList Rules()
		{
            ArrayList result = this.ChildItemsByType(DataStructureRule.CategoryConst);
            // add all child rulesets
            foreach (DataStructureRuleSetReference childRuleSet in this.ChildItemsByType(DataStructureRuleSetReference.CategoryConst))
            {
                if (childRuleSet.RuleSet != null)
                {
                    result.AddRange(childRuleSet.RuleSet.Rules());
                }
            }
            return result;
        }

        public void AddUniqueRuleSetIds(HashSet<Guid> ruleSetUniqIds, DataStructureRuleSetReference curRuleSetReference)
        {
            if (!ruleSetUniqIds.Add(this.Id))
            {
                throw new NullReferenceException(String.Format(
                    "Ruleset `{0}' ({1}) found twice. Circular ruleset reference found.",
                     this.Name, this.Id));
            }
            //
            ArrayList references = this.ChildItemsByType(DataStructureRuleSetReference.CategoryConst);
            bool addCurrent = true;
            foreach (DataStructureRuleSetReference ruleSetReference
                in this.ChildItemsByType(DataStructureRuleSetReference.CategoryConst))
            {
                if (curRuleSetReference != null && curRuleSetReference.Id == ruleSetReference.Id)
                {
                    // current already processed
                    addCurrent = false;
                }
                ruleSetReference.RuleSet.AddUniqueRuleSetIds(ruleSetUniqIds, curRuleSetReference);
            }
            // add current ruleset virtually - if we are in proper parent ruleset
            if (curRuleSetReference != null && curRuleSetReference.ParentItemId == this.Id && addCurrent)
            {
                curRuleSetReference.RuleSet.AddUniqueRuleSetIds(ruleSetUniqIds, curRuleSetReference);
            }
        }

    public ArrayList Rules(string entityName)
		{
			ArrayList result = new ArrayList();

			foreach(DataStructureRule rule in this.Rules())
			{
				if(rule.Entity.Name == entityName & rule.RuleDependencies.Count == 0)
				{
					result.Add(rule);
				}
			}

			return result;
		}

		public Hashtable RulesDepending(string entityName)
		{
			Hashtable result = new Hashtable();

			foreach(DataStructureRule rule in this.Rules())
			{
				if(rule.Entity.Name == entityName & rule.RuleDependencies.Count > 0)
				{
					result[rule.PrimaryKey] = rule;
				}
			}

			return result;
		}

#if ORIGAM_CLIENT
		private static Hashtable _ruleCache = new Hashtable();
#endif

		public ArrayList Rules(string entityName, Guid fieldId, bool includeOtherEntities)
		{
            ArrayList result;
#if ORIGAM_CLIENT
			string cacheId = this.Id.ToString() + entityName + fieldId.ToString() + includeOtherEntities.ToString();
            lock (_lock)
            {
                if (_ruleCache.Contains(cacheId))
                {
                    return _ruleCache[cacheId] as ArrayList;
                }
#endif
                result = new ArrayList();
                foreach (DataStructureRule rule in this.Rules())
                {
                    foreach (DataStructureRuleDependency dep in rule.RuleDependencies)
                    {
                        if (((includeOtherEntities == false & rule.Entity.Name == entityName) | includeOtherEntities) && dep.Entity.Name == entityName && dep.FieldId == fieldId)
                        {
                            result.Add(rule);
                        }
                    }
                }
#if ORIGAM_CLIENT
                _ruleCache.Add(cacheId, result);
            }
#endif
            return result;
		}
		#endregion

		#region Overriden AbstractDataEntityColumn Members
		
		[EntityColumn("ItemType")]
		public override string ItemType
		{
			get
			{
				return CategoryConst;
			}
		}

		public override bool UseFolders
		{
			get
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
				return new Type[] {typeof(DataStructureRule), typeof(DataStructureRuleSetReference)};
			}
		}

		public override AbstractSchemaItem NewItem(Type type, Guid schemaExtensionId, SchemaItemGroup group)
		{
			AbstractSchemaItem item;

			if(type == typeof(DataStructureRule))
			{
				item = new DataStructureRule(schemaExtensionId);
				item.Name = "NewRule";

			}
            else if (type == typeof(DataStructureRuleSetReference))
            {
                item = new DataStructureRuleSetReference(schemaExtensionId);
                item.Name = "NewRuleSetReference";
            }
			else
				throw new ArgumentOutOfRangeException("type", type, ResourceUtils.GetString("ErrorDataStructureRuleSetUnknownType"));

			item.Group = group;
			item.RootProvider = this;
			item.PersistenceProvider = this.PersistenceProvider;
			this.ChildItems.Add(item);

			return item;
		}

		#endregion
	}
}
