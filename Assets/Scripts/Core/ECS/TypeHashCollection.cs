using System;
using System.Collections.Generic;
using System.Linq;

namespace ECS
{
	public class TypeHashCollection<T>
	{
		private readonly Dictionary<Type, HashSet<T>> _items = new Dictionary<Type, HashSet<T>>();
		private static readonly HashSet<T> Empty = new HashSet<T>();

		public void AddItem(T item)
		{
			if (!_items.TryGetValue(item.GetType(), out var items))
			{
				items = new HashSet<T>();
				_items[item.GetType()] = items;
			}
			items.Add(item);
		}

		public void RemoveItem(T item)
		{
			if (_items.TryGetValue(item.GetType(), out var items))
			{
				items.Remove(item);
			}
		}

		public IEnumerable<T> GetItems()
		{
			return _items.TryGetValue(typeof(T), out var items) ? items : Empty;
		}

		public T GetSingleItem()
		{
			return GetItems().FirstOrDefault();
		}
	}
}
