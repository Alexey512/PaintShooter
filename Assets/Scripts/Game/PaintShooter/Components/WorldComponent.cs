using System;
using Core.ECS;

namespace Game.PaintShooter.Components
{
	[Serializable]
	public class WorldComponent: BaseComponent
	{
		public int UnitsCount = 0;
	}
}
