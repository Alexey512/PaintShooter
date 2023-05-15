using System;
using System.Collections.Generic;
using ECS;
using UnityEngine;
using Zenject;

namespace Game.Core.Actors
{
	public class Actor: MonoBehaviour, IActor
	{
		[SerializeField]
		private EntityConfig _config;
		
		[SerializeField]
		private EntityConfigAsset _configAsset;
		
		private readonly IEntity _entity = new Entity();

		private DiContainer _container;

		public GameObject GameObject => gameObject;

		public string Name => gameObject.name;

		[Inject]
		private void Construct(DiContainer container)
		{
			_container = container;

			if (_configAsset != null)
			{
				_configAsset.Initialize(this);
			}
			else
			{
				_config?.Initialize(this);
			}
		}

		#region IEntity

		public void Dispose()
		{
			_entity?.Dispose();
		}

		public Guid GUID => _entity?.GUID ?? Guid.Empty;
		public void Initialize()
		{
			_entity?.Initialize();
		}

		public void DeInitialize()
		{
			_entity?.DeInitialize();
		}

		public void Update()
		{
			_entity?.Update();
		}

		public void Clear()
		{
			_entity?.Clear();
		}

		public IComponent AddComponent(IComponent component, IEntity owner = null)
		{
			_container?.Inject(component);
			return _entity?.AddComponent(component, this);
		}

		public T AddComponent<T>(IEntity owner = null) where T : class, IComponent, new()
		{
			return _entity?.AddComponent<T>(this);
		}

		public void RemoveComponent(Type componentType)
		{
			_entity?.RemoveComponent(componentType);
		}

		public void RemoveComponent(IComponent component)
		{
			_entity?.RemoveComponent(component);
		}

		public void RemoveComponent<T>() where T : class, IComponent, new()
		{
			_entity?.RemoveComponent<T>();
		}

		public T GetSingleComponent<T>() where T : class, IComponent
		{
			return _entity?.GetSingleComponent<T>() ?? null;
		}

		public IEnumerable<IComponent> GetAllComponents()
		{
			return _entity?.GetAllComponents() ?? null;
		}

		public bool HasComponent<T>() where T : class, IComponent
		{
			return _entity?.HasComponent<T>() ?? false;
		}

		public bool HasComponent(int typeId)
		{
			return _entity?.HasComponent(typeId) ?? false;
		}

		public ISystem AddSystem(ISystem system, IEntity owner = null)
		{
			_container?.Inject(system);
			return _entity?.AddSystem(system, this);
		}

		public T AddSystem<T>(IEntity owner = null) where T : class, ISystem, new()
		{
			return _entity?.AddSystem<T>(this);
		}

		public void RemoveSystem(Type systemType)
		{
			_entity?.RemoveSystem(systemType);
		}

		public void RemoveSystem(ISystem system)
		{
			_entity?.RemoveSystem(system);
		}

		public void RemoveSystem<T>() where T : class, ISystem, new()
		{
			_entity?.RemoveSystem<T>();
		}

		public T GetSingleSystem<T>() where T : class, ISystem
		{
			return _entity?.GetSingleSystem<T>() ?? null;
		}

		public IEnumerable<ISystem> GetAllSystems()
		{
			return _entity?.GetAllSystems() ?? null;
		}

		#endregion
	}
}
