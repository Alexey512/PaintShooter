using System.Collections.Generic;
using ECS;

namespace Game.Core.Actors
{
	public interface IEntityConfig
	{
		List<IComponent> Components { get; }
		
		List<ISystem> Systems { get; }

		void Initialize(IEntity entity);
	}
}
