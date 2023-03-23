using System;
using System.Collections.Generic;
using System.Linq;

namespace ECS
{
	public class EntityManager: IEntityManager
	{
		public static IEntityManager Instance => _instance.Value;

		private static readonly Lazy<EntityManager> _instance = new Lazy<EntityManager>(() => new EntityManager());

		private readonly List<IEntity> _entities = new List<IEntity>();

		private readonly TypedCollection<IComponent> _components = new TypedCollection<IComponent>();
		
		private readonly TypedCollection<ISystem> _systems = new TypedCollection<ISystem>();

		public void RegisterEntity(IEntity entity)
		{
			_entities.Add(entity);

			foreach (var component in entity.GetAllComponents())
			{
				RegisterComponent(component);
			}

			foreach (var system in entity.GetAllSystems())
			{
				RegisterSystem(system);
			}
		}

		public void UnRegisterEntity(IEntity entity)
		{
			_entities.Remove(entity);

			foreach (var component in entity.GetAllComponents())
			{
				UnRegisterComponent(component);
			}

			foreach (var system in entity.GetAllSystems())
			{
				UnRegisterSystem(system);
			}
		}

		public void RegisterComponent(IComponent component)
		{
			_components.AddItem(component);
		}

		public void UnRegisterComponent(IComponent component)
		{
			_components.RemoveItem(component);
		}

		public void RegisterSystem(ISystem system)
		{
			_systems.AddItem(system);
		}

		public void UnRegisterSystem(ISystem system)
		{
			_systems.RemoveItem(system);
		}

		public IEnumerable<T> GetComponents<T>() where T : class, IComponent
		{
			return _components.GetItems<T>();
		}

		public IEnumerable<T> GetSystems<T>() where T : class, ISystem
		{
			return _systems.GetItems<T>();
		}

		public IEnumerable<IEntity> GetEntities()
		{
			return _entities;
		}
	}
}
