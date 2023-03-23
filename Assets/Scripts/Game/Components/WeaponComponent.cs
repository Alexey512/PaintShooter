using System;
using ECS;
using UnityEngine;

namespace Game.Components
{
	[Serializable]
	public class WeaponComponent: BaseComponent
	{
		[SerializeField]
		public float ShootInterval = 1.0f;
	}
}
