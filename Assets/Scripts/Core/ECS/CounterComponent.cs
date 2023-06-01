using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ECS;
using UnityEngine;

namespace Core.ECS
{
	public class CounterComponent<T>: BaseComponent where T: IComparable
	{
		public Action<T, T> ValueChanged;

		[SerializeField]
		private T _value;

		public T Value
		{
			get => _value; set => SetValue(value);
		}

		public void SetValue(T value, bool silency = false)
		{
			if (_value.Equals(value))
				return;

			T oldValue = _value;
			_value = value;
			if (silency)
			{
				ValueChanged?.Invoke(oldValue, _value);
			}
		}
	}
}
