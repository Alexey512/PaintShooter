using System.Collections.Generic;
using ECS;

namespace Actors
{
	public interface IEntityConfig
	{
		List<IComponent> Components { get; }
		
		List<ISystem> Systems { get; }

		void Initialize(IEntity entity);
	}
}
