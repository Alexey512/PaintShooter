using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.ECS
{
	public class Entity: IEntity
	{
		public Guid Guid { get; }
		
		private readonly Dictionary<int, IComponent> _components = new Dictionary<int, IComponent>();

		private bool _isInitialize = false;

		public bool IsInitialize => _isInitialize;

		public Entity()
		{
			Guid = Guid.NewGuid();
		}

		public void Dispose()
		{
			DeInitialize();
		}

		public virtual void Initialize()
		{
			if (_isInitialize)
				return;

            foreach (var component in _components.Values)
            {
                if (component is IAfterEntityInitialize afterDeInit)
                    afterDeInit.AfterEntityInitialize();
            }

            _isInitialize = true;
        }

		public virtual void DeInitialize()
		{
			if (!_isInitialize)
				return;

			foreach (var component in _components.Values)
			{
				if (component is IAfterEntityDeInitialize afterDeInit)
					afterDeInit.AfterEntityDeInitialize();
			}

            _isInitialize = false;
		}

		public void Clear()
		{
			DeInitialize();
			
			_components.Clear();
		}

		public IComponent AddComponent(IComponent component, IEntity owner = null)
		{
			var typeInfo = TypesResolver.GetTypeInfo(component.GetType());
			if (_components.TryGetValue(typeInfo.Id, out var comp))
			{
				return comp;
			}

			component.Owner = owner ?? this;
			_components[typeInfo.Id] = component;
			
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
            var typeInfo = TypesResolver.GetTypeInfo(componentType);
            if (_components.TryGetValue(typeInfo.Id, out var component))
			{
				if (component is IAfterEntityDeInitialize afterDeInit)
					afterDeInit.AfterEntityDeInitialize();
				
				component.Owner = null;
				_components.Remove(typeInfo.Id);
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
		
        public bool HasComponent<T>() where T : class, IComponent
        {
            return _components.ContainsKey(ComponentType<T>.Info.Id);
		}

        public bool HasComponent(int typeId)
        {
            return _components.ContainsKey(typeId);
        }

        public T FindComponent<T>() where T : class, IComponent
        {
            if (_components.TryGetValue(ComponentType<T>.Info.Id, out var component))
            {
                return component as T;
            }
            return null;
        }
    }
}
