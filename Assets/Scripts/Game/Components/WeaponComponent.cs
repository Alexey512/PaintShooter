using System;
using ECS;
using UnityEngine;

namespace Game.Components
{
	[Serializable]
	public class WeaponComponent: BaseComponent
	{
		public float RateOfFire = 1.0f;
	}
}
