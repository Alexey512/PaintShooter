using System.Collections.Generic;

namespace ECS
{
	public interface IEntityManager
	{
		void RegisterEntity(IEntity entity);

		void UnRegisterEntity(IEntity entity);

		void RegisterComponent(IComponent component);

		void UnRegisterComponent(IComponent component);

		void RegisterSystem(ISystem system);

		void UnRegisterSystem(ISystem system);

		IEnumerable<T> GetComponents<T>() where T : IComponent;

		IEnumerable<T> GetSystems<T>() where T : ISystem;

		IEnumerable<IEntity> GetEntities();
	}
}
