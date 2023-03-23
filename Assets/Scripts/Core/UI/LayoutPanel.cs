using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.UI
{
	public class LayoutPanel<T>: MonoBehaviour where T: MonoBehaviour
	{
		[SerializeField]
		private T _elementPref;

		[SerializeField]
		private Transform _elementsContainer;

		private readonly List<T> _elements = new List<T>();

		[Inject]
		private DiContainer _container;

		protected List<T> Elements => _elements;

		public T AddElement()
		{
			if (_container == null)
				return null;
			
			if (_elementPref == null)
				return null;

			var elementObj = _container.InstantiatePrefab(_elementPref.gameObject, _elementsContainer);
			if (elementObj == null)
				return null;

			elementObj.SetActive(true);

			var element = elementObj.GetComponent<T>();
			if (element == null)
			{
				Destroy(elementObj);
				return null;
			}

			_elements.Add(element);
			return element;
		}

		public void Clear()
		{
			foreach (var element in _elements)
			{
				Destroy(element.gameObject);	
			}
			_elements.Clear();
		}
	}
}
