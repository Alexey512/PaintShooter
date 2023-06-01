using UnityEngine;

namespace Core.ECS
{
	public interface IActor: IEntity
	{
		public string Name { get; }
		
		GameObject Owner { get; }
	}
}
