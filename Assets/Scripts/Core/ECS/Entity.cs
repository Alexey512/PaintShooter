using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ECS
{
	public class Entity: IEntity
	{
		public Guid GUID { get; }
		
		private readonly Dictionary<Type, IComponent> _components = new Dictionary<Type, IComponent>();
		
		private readonly Dictionary<Type, ISystem> _systems = new Dictionary<Type, ISystem>();

		private bool _isInitialize = false;

		public bool IsInitialize => _isInitialize;

		public Entity()
		{
			GUID = Guid.NewGuid();
		}

		public void Dispose()
		{
			DeInitialize();
		}

		public void Initialize()
		{
			if (_isInitialize)
				return;
			
			EntityManager.Instance.RegisterEntity(this);

			foreach (var component in _components.Values)
			{
				if (component is IAfterEntityInitialize afterInit)
					afterInit.AfterEntityInitialize();
			}

			foreach (var system in _systems.Values)
			{
				if (system is IAfterEntityInitialize afterInit)
					afterInit.AfterEntityInitialize();
			}
		}

		public void DeInitialize()
		{
			if (!_isInitialize)
				return;

			foreach (var component in _components.Values)
			{
				if (component is IAfterEntityDeInitialize afterDeInit)
					afterDeInit.AfterEntityDeInitialize();
			}

			foreach (var system in _systems.Values)
			{
				if (system is IAfterEntityDeInitialize afterDeInit)
					afterDeInit.AfterEntityDeInitialize();
			}

			EntityManager.Instance.UnRegisterEntity(this);
		}

		public void Update()
		{
			foreach (var system in _systems.Values)
			{
				system.Update();
			}
		}

		public void Clear()
		{
			DeInitialize();
			
			_components.Clear();
			_systems.Clear();
		}

		#region Components

		public IComponent AddComponent(IComponent component, IEntity owner = null)
		{
			Type componentType = component.GetType();
			if (_components.TryGetValue(componentType, out var comp))
			{
				Debug.LogError($"Component '{componentType}' already added in Entity '{GUID}'");
				return comp;
			}

			component.Owner = owner ?? this;
			_components[componentType] = component;
			
			if (_isInitialize)
			{
				if (component is IAfterEntityInitialize afterInit)
					afterInit.AfterEntityInitialize();
			}
			
			return component;
		}

		public T AddComponent<T>(IEntity owner = null) where T : class, IComponent, new()
		{
			return AddComponent(new T(), owner) as T;
		}

		public void RemoveComponent(Type componentType)
		{
			if (_components.TryGetValue(componentType, out var component))
			{
				if (component is IAfterEntityDeInitialize afterDeInit)
					afterDeInit.AfterEntityDeInitialize();
				
				component.Owner = null;
				_components.Remove(componentType);
			}
		}

		public void RemoveComponent(IComponent component)
		{
			RemoveComponent(component.GetType());
		}

		public void RemoveComponent<T>() where T : class, IComponent, new()
		{
			RemoveComponent(typeof(T));
		}

		public T GetSingleComponent<T>() where T : class, IComponent
		{
			if (_components.TryGetValue(typeof(T), out var component))
			{
				return component as T;
			}
			return null;
		}
		
		public IEnumerable<IComponent> GetAllComponents() => _components.Values;
		public bool HasComponent<T>() where T : class, IComponent
		{
			return HasComponent(typeof(T).GetHashCode());
		}

		public bool HasComponent(int typeId)
		{
			return _components.Values.Any(c => c.TypeId == typeId);
		}

		#endregion

		#region Systems

		public ISystem AddSystem(ISystem system, IEntity owner = null)
		{
			Type systemType = system.GetType();
			if (_systems.TryGetValue(systemType, out var sys))
			{
				Debug.LogError($"System '{systemType}' already added in Entity '{GUID}'");
				return sys;
			}

			system.Owner = owner ?? this;
			system.Initialize();
			_systems[systemType] = system;
			
			if (_isInitialize)
			{
				if (system is IAfterEntityInitialize afterInit)
					afterInit.AfterEntityInitialize();
			}
			
			return system;
		}

		public T AddSystem<T>(IEntity owner = null) where T : class, ISystem, new()
		{
			return AddSystem(new T(), owner) as T;
		}

		public void RemoveSystem(Type systemType)
		{
			if (_systems.TryGetValue(systemType, out var system))
			{
				if (system is IAfterEntityDeInitialize afterDeInit)
					afterDeInit.AfterEntityDeInitialize();
				
				system.Owner = null;
				_systems.Remove(systemType);
			}
		}

		public void RemoveSystem(ISystem system)
		{
			_components.Remove(system.GetType());
		}

		public void RemoveSystem<T>() where T : class, ISystem, new()
		{
			_systems.Remove(typeof(T));
		}

		public T GetSingleSystem<T>() where T : class, ISystem
		{
			if (_systems.TryGetValue(typeof(T), out var system))
			{
				return system as T;
			}
			return null;
		}

		public IEnumerable<ISystem> GetAllSystems() => _systems.Values;

		#endregion
	}
}
