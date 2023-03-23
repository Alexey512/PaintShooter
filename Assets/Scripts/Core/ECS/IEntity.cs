using System;
using System.Collections.Generic;

namespace ECS
{
	public interface IEntity: IDisposable
	{
		Guid GUID { get; }

		void Initialize();

		void DeInitialize();

		void Clear();

		void Update();

		#region Components
		
		IComponent AddComponent(IComponent component, IEntity owner = null);

		T AddComponent<T>(IEntity owner = null) where T : class, IComponent, new();

		void RemoveComponent(Type componentType);

		void RemoveComponent(IComponent component);

		void RemoveComponent<T>() where T : class, IComponent, new();

		public T GetSingleComponent<T>() where T : class, IComponent;

		public IEnumerable<IComponent> GetAllComponents();
		
		#endregion

		#region Systems

		ISystem AddSystem(ISystem system, IEntity owner = null);

		T AddSystem<T>(IEntity owner = null) where T : class, ISystem, new();

		void RemoveSystem(Type systemType);

		void RemoveSystem(ISystem system);

		void RemoveSystem<T>() where T : class, ISystem, new();

		T GetSingleSystem<T>() where T : class, ISystem;

		IEnumerable<ISystem> GetAllSystems();

		#endregion
	}
}
