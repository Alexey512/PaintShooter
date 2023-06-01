using Core.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using Zenject;

namespace Game.Core.ECS
{
    public class WorldProvider: MonoBehaviour, IWorld
    {
        [SerializeField]
        private WorldConfig _config;

        private readonly World _world = new World();

        private readonly List<IUpdateSystem> _updateSystems = new List<IUpdateSystem>();

        private readonly List<IFixedUpdateSystem> _fixedUpdateSystems = new List<IFixedUpdateSystem>();

        private readonly List<ILateUpdateSystem> _lateUpdateSystems = new List<ILateUpdateSystem>();

        private DiContainer _container;

        [Inject]
        protected virtual void Construct(DiContainer container)
        {
            _container = container;
            _config?.Initialize(this);

            Initialize();
        }

        public Guid Guid => _world?.Guid ?? Guid.Empty;

        public void Initialize()
        {
            _world?.Initialize();
        }

        public void DeInitialize()
        {
            _world?.DeInitialize();
        }

        public void Clear()
        {
            _world?.Clear();
        }

        public EntityFilter Filter<T>() where T : class, IComponent
        {
            return _world?.Filter<T>() ?? new EntityFilter(_world);
        }

        public void AddEntity(IEntity entity)
        {
            _world?.AddEntity(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            _world?.RemoveEntity(entity);
        }

        public IComponent AddComponent(IComponent component, IEntity owner = null)
        {
            _container?.Inject(component);
            return _world?.AddComponent(component, this);
        }

        public T AddComponent<T>(IEntity owner = null) where T : class, IComponent, new()
        {
            return _world?.AddComponent<T>(this);
        }

        public void RemoveComponent(Type componentType)
        {
            _world?.RemoveComponent(componentType);
        }

        public void RemoveComponent(IComponent component)
        {
            _world?.RemoveComponent(component);
        }

        public void RemoveComponent<T>() where T : class, IComponent, new()
        {
            _world?.RemoveComponent<T>();
        }

        public bool HasComponent<T>() where T : class, IComponent
        {
            return _world?.HasComponent<T>() ?? false;
        }

        public bool HasComponent(int typeId)
        {
            return _world?.HasComponent(typeId) ?? false;
        }

        public T FindComponent<T>() where T : class, IComponent
        {
            return _world?.FindComponent<T>() ?? null;
        }

        public ISystem AddSystem(ISystem system, IWorld world = null)
        {
            if (system == null)
                return null;
            _container?.Inject(system);
            _world?.AddSystem(system, this);
            OnAddSystem(system);
            return system;
        }

        public T AddSystem<T>(IWorld world = null) where T : class, ISystem, new()
        {
            var system = _world?.AddSystem<T>(this);
            OnAddSystem(system);
            return system;
        }

        public void RemoveSystem(Type systemType)
        {
            OnRemoveSystem(GetSystem(systemType));
            _world?.RemoveSystem(systemType);
        }

        public void RemoveSystem(ISystem system)
        {
            OnRemoveSystem(system);
            _world?.RemoveSystem(system);
        }

        public void RemoveSystem<T>() where T : class, ISystem, new()
        {
            OnRemoveSystem(GetSystem<T>());
            _world?.RemoveSystem<T>();
        }

        public T GetSystem<T>() where T : class, ISystem
        {
            return _world?.GetSystem<T>();
        }

        public ISystem GetSystem(Type systemType)
        {
            return _world?.GetSystem(systemType);
        }

        public IEnumerable<ISystem> GetSystems()
        {
            return _world?.GetSystems();
        }

        private void OnAddSystem(ISystem system)
        {
            if (system == null)
                return;

            if (system is IUpdateSystem updateSystem)
            {
                _updateSystems.Add(updateSystem);
            }
            if (system is IFixedUpdateSystem fixedUpdateSystem)
            {
                _fixedUpdateSystems.Add(fixedUpdateSystem);
            }
            if (system is ILateUpdateSystem lateUpdateSystem)
            {
                _lateUpdateSystems.Add(lateUpdateSystem);
            }
        }

        private void OnRemoveSystem(ISystem system)
        {
            if (system == null)
                return;

            if (system is IUpdateSystem updateSystem)
            {
                _updateSystems.Remove(updateSystem);
            }
            if (system is IFixedUpdateSystem fixedUpdateSystem)
            {
                _fixedUpdateSystems.Remove(fixedUpdateSystem);
            }
            if (system is ILateUpdateSystem lateUpdateSystem)
            {
                _lateUpdateSystems.Remove(lateUpdateSystem);
            }
        }

        private void Update()
        {
            foreach (var system in _updateSystems)
            {
                system.OnUpdate(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            foreach (var system in _fixedUpdateSystems)
            {
                system.OnFixedUpdate(Time.deltaTime);
            }
        }

        private void LateUpdate()
        {
            foreach (var system in _lateUpdateSystems)
            {
                system.OnLateUpdate(Time.deltaTime);
            }
        }
    }
}
