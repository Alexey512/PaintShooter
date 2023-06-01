using System;
using System.Collections.Generic;
using Core.ECS;
using UnityEngine;
using Zenject;
using static Zenject.CheatSheet;

namespace Game.Core.ECS
{
	public class ActorProvider: MonoBehaviour, IActor, IPoolable<IMemoryPool>
    {
        [SerializeField]
		private EntityConfig _config;
		
		[SerializeField]
		private EntityConfigAsset _configAsset;

        public Guid Guid => _entity?.Guid ?? Guid.Empty;

        public string Name => gameObject.name;

        public GameObject Owner => gameObject;

        private readonly IEntity _entity = new Entity();

		private DiContainer _container;

        private IWorld _world;

        private IMemoryPool _pool;

        [Inject]
		protected virtual void Construct(IWorld world, DiContainer container)
        {
            _world = world;

            _container = container;

			if (_configAsset != null)
			{
				_configAsset.Initialize(this);
			}
			else
			{
				_config?.Initialize(this);
			}

            _world?.AddEntity(this);
        }


		public void Initialize()
		{
            _entity?.Initialize();
		}

		public void DeInitialize()
		{
			_entity?.DeInitialize();
			_pool?.Despawn(this);
        }

        public void Update()
		{
			//_entity?.Update();
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

		public bool HasComponent<T>() where T : class, IComponent
		{
			return _entity?.HasComponent<T>() ?? false;
		}

        public bool HasComponent(int typeId)
        {
            return _entity?.HasComponent(typeId) ?? false;
        }

        public T FindComponent<T>() where T : class, IComponent
        {
            return _entity?.FindComponent<T>() ?? null;
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            gameObject.SetActive(true);
            _world?.AddEntity(this);
        }

        public void OnDespawned()
        {
            _pool = null;
			gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
			_world?.RemoveEntity(this);
        }
    }
}
