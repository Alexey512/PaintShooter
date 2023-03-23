using System;

namespace ECS
{
	public interface ISystem: IDisposable
	{
		public Guid GUID { get; set; }

		public IEntity Owner { get; set; }

		void Initialize();

		void Update();
	}
}
