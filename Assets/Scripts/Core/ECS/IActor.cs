using UnityEngine;

namespace ECS
{
	public interface IActor: IEntity
	{
		public string Name { get; }
		
		GameObject GameObject { get; }
	}
}
