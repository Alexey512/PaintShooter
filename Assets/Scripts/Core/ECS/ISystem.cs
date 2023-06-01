using System;

namespace Core.ECS
{
	public interface ISystem
	{
		public IWorld World { get; set; }

        public Guid Guid { get; set; }
	}
}
