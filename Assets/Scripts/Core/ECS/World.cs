using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ECS;
using UnityEngine;

namespace Core.ECS
{
	public class World: Entity, IWorld
    {
		public List<IEntity> Entities => _entities;
		
		private readonly List<IEntity> _entities = new List<IEntity>();

        private readonly Dictionary<int, ISystem> _systems = new Dictionary<int, ISystem>();

        public override void Initialize()
        {
            if (IsInitialize)
                return;

            foreach (var system in _systems.Values)
            {
                if (system is IAfterEntityInitialize afterInit)
                    afterInit.AfterEntityInitialize();
            }

            base.Initialize();
        }

        public override void DeInitialize()
        {
            if (!IsInitialize)
                return;

            foreach (var system in _systems.Values)
            {
                if (system is IAfterEntityDeInitialize afterDeInit)
                    afterDeInit.AfterEntityDeInitialize();
            }

            base.DeInitialize();
        }

        public void AddEntity(IEntity entity)
		{
			if (entity == null || _entities.Contains(entity))
                return;
            _entities.Add(entity);
            entity.Initialize();

        }

		public void RemoveEntity(IEntity entity)
		{
			if (entity == null || !_entities.Contains(entity))
                return;
            entity.DeInitialize();
            _entities.Remove(entity);
		}

        public EntityFilter Filter<T>() where T : class, IComponent
        {
            return new EntityFilter(this, ComponentType<T>.Info.Id);
        }

        public ISystem AddSystem(ISystem system, IWorld world = null)
        {
            var typeInfo = TypesResolver.GetTypeInfo(system.GetType());
            if (_systems.TryGetValue(typeInfo.Id, out var sys))
            {
                Debug.LogError($"System '{typeInfo.Type}' already added in Entity '{Guid}'");
                return sys;
            }

            system.World = world ?? this;
            _systems[typeInfo.Id] = system;

            if (IsInitialize)
            {
                if (system is IAfterEntityInitialize afterInit)
                    afterInit.AfterEntityInitialize();
            }



            return system;
        }

        public T AddSystem<T>(IWorld world = null) where T : class, ISystem, new()
        {
            return AddSystem(new T(), world) as T;
        }

        public void RemoveSystem(Type systemType)
        {
            var typeInfo = TypesResolver.GetTypeInfo(systemType);
            if (_systems.TryGetValue(typeInfo.Id, out var system))
            {
                if (system is IAfterEntityDeInitialize afterDeInit)
                    afterDeInit.AfterEntityDeInitialize();
                system.World = null;
                _systems.Remove(typeInfo.Id);
            }
        }

        public void RemoveSystem(ISystem system)
        {
            RemoveSystem(system.GetType());
        }

        public void RemoveSystem<T>() where T : class, ISystem, new()
        {
            RemoveSystem(typeof(T));
        }

        public T GetSystem<T>() where T : class, ISystem
        {
            if (_systems.TryGetValue(SystemType<T>.Info.Id, out var system))
            {
                return system as T;
            }
            return null;
        }

        public ISystem GetSystem(Type systemType)
        {
            if (_systems.TryGetValue(TypesResolver.GetTypeInfo(systemType).Id, out var system))
            {
                return system;
            }
            return null;
        }
        
        public IEnumerable<ISystem> GetSystems()
        {
            return _systems.Values;
        }
    }
}
