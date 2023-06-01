using System;

namespace Core.ECS
{
	[Serializable]
	public class BaseComponent: IComponent
	{
		public IEntity Owner { get; set; }

		public IActor Actor => Owner as IActor;
	}
}
