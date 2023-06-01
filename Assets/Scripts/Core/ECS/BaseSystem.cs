using System;

namespace Core.ECS
{
	[Serializable]
	public abstract class BaseSystem: ISystem
	{
		public Guid Guid { get; set; }
		
		public IWorld World { get; set; }
	}
}
