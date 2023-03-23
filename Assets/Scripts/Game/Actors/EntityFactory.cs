using ECS;
using Zenject;

namespace Actors
{
	public class EntityFactory: IFactory<IEntity>
	{
		private readonly DiContainer _container;

		public EntityFactory(DiContainer container)
		{
			_container = container;
		}
		
		public IEntity Create()
		{
			var entity = _container.Instantiate<Entity>();
			entity.Initialize();
			return entity;
		}
	}
}
