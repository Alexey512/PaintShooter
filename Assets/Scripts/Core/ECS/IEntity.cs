using System;
using System.Collections.Generic;

namespace Core.ECS
{
	public interface IEntity
	{
		Guid Guid { get; }

		void Initialize();

		void DeInitialize();

		void Clear();
		
		IComponent AddComponent(IComponent component, IEntity owner = null);

		T AddComponent<T>(IEntity owner = null) where T : class, IComponent, new();

		void RemoveComponent(Type componentType);

		void RemoveComponent(IComponent component);

		void RemoveComponent<T>() where T : class, IComponent, new();

		bool HasComponent<T>() where T : class, IComponent;

        bool HasComponent(int typeId);

        T FindComponent<T>() where T : class, IComponent;
    }
}
