using System;

namespace ECS
{
	[Serializable]
	public class BaseComponent: IComponent
	{
		public IEntity Owner { get; set; }
	}
}
