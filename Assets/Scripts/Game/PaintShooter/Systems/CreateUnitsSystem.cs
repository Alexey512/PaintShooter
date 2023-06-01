using System;
using Core.ECS;

namespace Game.PaintShooter.Systems
{
	[Serializable]
	public class CreateUnitsSystem: BaseSystem, IAfterEntityInitialize
	{
		public void AfterEntityInitialize()
		{
			//TODO: inialize bots
		}
	}
}
