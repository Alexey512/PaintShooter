using System;
using ECS;

namespace Game.PaintShooter.Systems
{
	[Serializable]
	public class CreateUnitsSystem: BaseSystem, IAfterEntityInitialize
	{
		public override void Initialize()
		{
			
		}

		public void AfterEntityInitialize()
		{
			//TODO: inialize bots
		}
	}
}
