using System;
using System.Collections.Generic;
using System.Linq;
using Core.Factories;
using ECS;
using Game.Core.Actors;
using UnityEngine;
using Utility;
using Zenject;

namespace Game.Core.Factories
{
	[CreateAssetMenu(fileName = "ActorsFactory", menuName = "Game/Actors Factory", order = -1)]
	public class ActorsFactory<T>: ScriptableObject, IActorsFactory<T> where T : Enum
	{
		[SerializeField] 
		private List<Value> _factories;

		[Serializable]
		public class Value
		{
			public T Key;
			public GameObjectsFactory Factory;
		}

		[Inject]
		private void Construct(DiContainer container)
		{
			foreach (var factory in _factories)
			{
				factory?.Factory?.Initialize(container);
			}
		}

		public IActor Create(T type)
		{
			var factory = _factories.FirstOrDefault(i => i.Key.Equals(type));
			if (factory == null || factory.Factory == null)
				return null;

			var obj = factory.Factory.Instantiate();
			return obj != null ? obj.GetComponent<IActor>() : null;
		}

		public void Release(T type, IActor actor)
		{
			if (actor == null)
				return;

			var factory = _factories.FirstOrDefault(i => i.Key.Equals(type));
			if (factory == null || factory.Factory == null)
			{
				GameObject.Destroy(actor.GameObject);
				return;
			}

			factory.Factory.Release(actor.GameObject);
		}
	}
}
