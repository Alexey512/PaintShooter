﻿namespace Core.ECS
{
	public interface IComponent
	{
		public IEntity Owner { get; set; }

		public IActor Actor { get; }
	}
}
