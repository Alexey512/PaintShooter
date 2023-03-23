using System.Collections.Generic;
using Core.Utility;
using ECS;
using UnityEngine;

namespace Actors
{
	[CreateAssetMenu(fileName = "EntityConfig", menuName = "Game/Entity Config", order = -1)]
	public class EntityConfig: ScriptableObject, IEntityConfig
	{
		[SerializeReference, SubclassSelector]
		private List<IComponent> _components = new List<IComponent>();

		[SerializeReference, SubclassSelector]
		private List<ISystem> _systems = new List<ISystem>();

		public List<IComponent> Components => _components;

		public List<ISystem> Systems => _systems;

		public void Initialize(IEntity entity)
		{
			if (entity == null)
				return;

			entity.Clear();

			foreach (var component in _components)
			{
				entity.AddComponent(ObjectCloner.Clone(component));
			}

			foreach (var system in _systems)
			{
				entity.AddSystem(ObjectCloner.Clone(system));
			}

			entity.Initialize();
		}
	}
}
