using System;

namespace ECS
{
	[Serializable]
	public class BaseComponent: IComponent
	{
		public int TypeId { get; private set; }

		public IEntity Owner { get; set; }

		public IActor Actor => Owner as IActor;

		public BaseComponent()
		{
			Owner = null;
			TypeId = GetType().GetHashCode();
		}
	}
}
