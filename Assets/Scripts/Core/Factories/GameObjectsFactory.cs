using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.Factories
{
	public class GameObjectsFactory: ScriptableObject, IGameObjectsFactory
	{
		[SerializeField]
		private GameObject _prefab;

		protected DiContainer Container;

		private readonly Stack<GameObject> _objects = new Stack<GameObject>();

		public void Initialize(DiContainer container)
		{
			Container = container;
		}

		public GameObject Instantiate()
		{
			if (_prefab == null)
				return null;

			if (_objects.Count > 0)
			{
				var obj = _objects.Pop();
				obj.SetActive(true);
				return obj;
			}

			return Container.InstantiatePrefab(_prefab);
		}

		public void Release(GameObject obj)
		{
			if (obj == null)
				return;
			
			obj.SetActive(false);
			_objects.Push(obj);
		}
	}
}
