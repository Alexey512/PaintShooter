using System;
using Core.ECS;
using UnityEngine;

namespace Game.PaintShooter.Components
{
	[Serializable]
	public class WeaponComponent: BaseComponent
	{
		public float RateOfFire = 1.0f;
	}
}
