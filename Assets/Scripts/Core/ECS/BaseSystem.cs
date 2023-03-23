using System;

namespace ECS
{
	[Serializable]
	public abstract class BaseSystem: ISystem
	{
		public Guid GUID { get; set; }
		
		public IEntity Owner { get; set; }

		public IActor Actor => Owner as IActor;

		public virtual void Initialize() {}

		public virtual void Update() {}

		public virtual void Dispose()
		{
			Owner = null;
		}
	}
}
