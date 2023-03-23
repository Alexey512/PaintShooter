using System;
using System.Collections.Generic;
using System.Linq;

namespace ECS
{
	public class TypedCollection<T> where T : class
	{
		private readonly Dictionary<Type, List<T>> _items = new Dictionary<Type, List<T>>();

		public void AddItem(T item)
		{
			if (!_items.TryGetValue(item.GetType(), out var items))
			{
				items = new List<T>();
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

		public List<TU> GetItems<TU>() where TU : class, T
		{
			List<TU> result = new List<TU>();
			if (_items.TryGetValue(typeof(TU), out var items))
			{
				foreach (var item in items)
				{
					result.Add(item as TU);
				}
			}
			return result;
		}

		public TU GetSingleItem<TU>() where TU : class, T
		{
			return GetItems<TU>().FirstOrDefault();
		}
	}
}
