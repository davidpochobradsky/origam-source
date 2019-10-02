#region license
/*
Copyright 2005 - 2019 Advantage Solutions, s. r. o.

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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Origam.DA.Service;
using Origam.Schema;

namespace Origam.DA.ObjectPersistence
{
    public abstract class AbstractPersistenceProvider : IPersistenceProvider
    {

        private readonly Queue<object> transactionEndEventQueue = new Queue<object>();
        public virtual ICompiledModel CompiledModel
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public event EventHandler<IPersistent> InstancePersisted;

        public void RunInTransaction(Action action)
        {
            BeginTransaction();
            action();
            EndTransaction();
        }

        public virtual void BeginTransaction()
        {
        }

        public abstract object Clone();

        public abstract void DeletePackage(Guid packageId);
        public virtual bool IsInTransaction { get; }

        public abstract void Dispose();

        public virtual void EndTransaction()
        {
            while (transactionEndEventQueue.Count > 0)
            {
                object sender = transactionEndEventQueue.Dequeue();
                InstancePersisted?.Invoke(this, (IPersistent)sender);
            }
        }

        public virtual void EndTransactionDontSave()
        {
            
        }

        public abstract object RetrieveValue(Guid instanceId, Type parentType, string fieldName);
        public virtual void RestrictToLoadedPackage(bool b)
        {
        }

        public abstract ILocalizationCache LocalizationCache { get;}

        public abstract void FlushCache();

        public abstract void RemoveFromCache(IPersistent instance);
        public abstract List<T> RetrieveList<T>(IDictionary<string, object> filter=null);

        public abstract List<T> RetrieveListByType<T>(string itemType);

        public abstract List<T> RetrieveListByPackage<T>(Guid packageId);

        public abstract T[] FullTextSearch<T>(string text);

        public ArrayList GetReference(Key key)
        {
            try
            {
                RestrictToLoadedPackage(false);
                ArrayList listOfReferences = null;
                if (ReferenceIndexManager.UseIndex)
                {
                    listOfReferences = new ArrayList();
                    Guid guid = Guid.Parse(key.ToString());
                    List<KeyValuePair<Guid, KeyValuePair<Guid, Type>>> list =
                        ReferenceIndexManager
                        .GetReferenceIndex()
                        .Where(x => x.Key == guid)
                        .ToList();
                    foreach (KeyValuePair<Guid, KeyValuePair<Guid, Type>> keyValue in list)
                    {
                        KeyValuePair<Guid, Type> kvalue = keyValue.Value;
                        object refInstance = RetrieveInstance(kvalue.Value, new ModelElementKey(kvalue.Key));
                        if (!listOfReferences.Contains(refInstance))
                        {
                            listOfReferences.Add(refInstance);
                        }
                    }
                }
                return listOfReferences;
            }
            finally
            {
                RestrictToLoadedPackage(true);
            }
        }

        private IEnumerable<object> FindUsages(AbstractSchemaItem item, bool ignoreErrors, Key key)
        {
            List<object> foundUsages = new List<object>();
            try
            {
                ArrayList dep = item.GetDependencies(ignoreErrors);
                foreach (AbstractSchemaItem depItem in dep)
                {
                    if (depItem != null)
                    {
                        if (depItem.PrimaryKey.Equals(key))
                        {
                            foundUsages.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ResourceUtils.GetString("ErrorWhenDependencies",
                    item.ItemType, item.Path,
                    Environment.NewLine + Environment.NewLine + ex.Message), ex);
            }
            return foundUsages;
        }

        public abstract List<T> RetrieveListByParent<T>(Key primaryKey,
            string parentTableName,
            string childTableName, bool useCache);

        public abstract List<T> RetrieveListByGroup<T>(Key primaryKey);

        public void OnTransactionEnded(object sender)
        {
            if (InTransaction)
            {
                transactionEndEventQueue.Enqueue(sender);
            }
        }

        public virtual void Persist(IPersistent obj)
        {
            if (!InTransaction)
            { 
                InstancePersisted?.Invoke(this, obj);
            }
        }

        public virtual List<string> Files(IPersistent persistentObject)
        {
            return new List<string>();
        }

        public abstract bool InTransaction { get; }

        public abstract void RefreshInstance(IPersistent persistentObject);

        public abstract object RetrieveInstance(Type type, Key primaryKey);

        public abstract object RetrieveInstance(Type type, Key primaryKey, bool useCache);

        public abstract object RetrieveInstance(Type type, Key primaryKey, bool useCache, bool throwNotFoundException);
    }
}
