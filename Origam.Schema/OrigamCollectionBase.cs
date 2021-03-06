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
using System.Collections;

namespace Origam.Schema
{

	[Serializable]
	public abstract class OrigamCollectionBase : IList, ICollection, IEnumerable
	{
		// Fields
		private ArrayList list;

		// Methods
		protected OrigamCollectionBase()
		{
			this.list = new ArrayList();
		}

		protected OrigamCollectionBase(int capacity)
		{
			this.list = new ArrayList(capacity);
		}

		public void Clear()
		{
			this.OnClear();
			this.InnerList.Clear();
			this.OnClearComplete();
		}

		public IEnumerator GetEnumerator()
		{
			return this.InnerList.GetEnumerator();
		}

		protected virtual void OnClear()
		{
		}

		protected virtual void OnClearComplete()
		{
		}

		protected virtual void OnInsert(int index, object value)
		{
		}

		protected virtual void OnInsertComplete(int index, object value)
		{
		}

		protected virtual void OnRemove(int index, object value)
		{
		}

		protected virtual void OnRemoveComplete(int index, object value)
		{
		}

		protected virtual void OnSet(int index, object oldValue, object newValue)
		{
		}

		protected virtual void OnSetComplete(int index, object oldValue, object newValue)
		{
		}

		protected virtual void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
		}

		public void RemoveAt(int index)
		{
			if ((index < 0) || (index >= this.InnerList.Count))
			{
				throw new ArgumentOutOfRangeException("index", "ArgumentOutOfRange_Index");
			}
			object obj2 = this.InnerList[index];
			this.OnValidate(obj2);
			this.OnRemove(index, obj2);
			this.InnerList.RemoveAt(index);
			try
			{
				this.OnRemoveComplete(index, obj2);
			}
			catch
			{
				this.InnerList.Insert(index, obj2);
				throw;
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			this.InnerList.CopyTo(array, index);
		}

		int IList.Add(object value)
		{
			this.OnValidate(value);
			this.OnInsert(this.InnerList.Count, value);
			int index = this.InnerList.Add(value);
			try
			{
				this.OnInsertComplete(index, value);
			}
			catch
			{
				this.InnerList.RemoveAt(index);
				throw;
			}
			return index;
		}

		bool IList.Contains(object value)
		{
			return this.InnerList.Contains(value);
		}

		int IList.IndexOf(object value)
		{
			return this.InnerList.IndexOf(value);
		}

		void IList.Insert(int index, object value)
		{
			if ((index < 0) || (index > this.InnerList.Count))
			{
				throw new ArgumentOutOfRangeException("index", "ArgumentOutOfRange_Index");
			}
			this.OnValidate(value);
			this.OnInsert(index, value);
			this.InnerList.Insert(index, value);
			try
			{
				this.OnInsertComplete(index, value);
			}
			catch
			{
				this.InnerList.RemoveAt(index);
				throw;
			}
		}

		void IList.Remove(object value)
		{
			this.OnValidate(value);
			int index = this.InnerList.IndexOf(value);
			if (index < 0)
			{
				throw new ArgumentException("Arg_RemoveArgNotFound");
			}
			this.OnRemove(index, value);
			this.InnerList.RemoveAt(index);
			try
			{
				this.OnRemoveComplete(index, value);
			}
			catch
			{
				this.InnerList.Insert(index, value);
				throw;
			}
		}

		// Properties
		public int Capacity
		{
			get
			{
				return this.InnerList.Capacity;
			}
			set
			{
				this.InnerList.Capacity = value;
			}
		}

		public int Count
		{
			get
			{
				if (this.list != null)
				{
					return this.list.Count;
				}
				return 0;
			}
		}

		protected ArrayList InnerList
		{
			get
			{
				if (this.list == null)
				{
					this.list = new ArrayList();
				}
				return this.list;
			}
		}

		protected IList List
		{
			get
			{
				return this;
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InnerList.IsSynchronized;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				return this.InnerList.SyncRoot;
			}
		}

		bool IList.IsFixedSize
		{
			get
			{
				return this.InnerList.IsFixedSize;
			}
		}

		bool IList.IsReadOnly
		{
			get
			{
				return this.InnerList.IsReadOnly;
			}
		}

		object IList.this[int index]
		{
			get
			{
				if ((index < 0) || (index >= this.InnerList.Count))
				{
					throw new ArgumentOutOfRangeException("index", "ArgumentOutOfRange_Index");
				}
				return this.InnerList[index];
			}
			set
			{
				if ((index < 0) || (index >= this.InnerList.Count))
				{
					throw new ArgumentOutOfRangeException("index", "ArgumentOutOfRange_Index");
				}
				this.OnValidate(value);
				object oldValue = this.InnerList[index];
				this.OnSet(index, oldValue, value);
				this.InnerList[index] = value;
				try
				{
					this.OnSetComplete(index, oldValue, value);
				}
				catch
				{
					this.InnerList[index] = oldValue;
					throw;
				}
			}
		}
	}
}