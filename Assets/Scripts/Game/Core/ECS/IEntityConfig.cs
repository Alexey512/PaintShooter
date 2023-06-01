using System.Collections.Generic;
using Core.ECS;

namespace Game.Core.ECS
{
	public interface IEntityConfig
	{
		List<IComponent> Components { get; }
	}
}
