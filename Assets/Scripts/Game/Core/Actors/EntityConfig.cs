using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS;
using UnityEngine;

namespace Game.Core.Actors
{
	[Serializable]
	public class EntityConfig: IEntityConfig
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
				if (component == null)
					continue;
				entity.AddComponent(component);
			}

			foreach (var system in _systems)
			{
				if (system == null)
					continue;
				entity.AddSystem(system);
			}

			entity.Initialize();
		}
	}
}
