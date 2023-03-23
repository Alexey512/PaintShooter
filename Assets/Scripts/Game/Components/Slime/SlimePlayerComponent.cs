using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS;
using UnityEngine;

namespace Game.Components.Slime
{
	[Serializable]
	public class SlimePlayerComponent: BaseComponent
	{
		public float Speed = 3.0f;

		public float FightDistance = 1.0f;
		
		public float RateOfFire = 1.0f;

	}
}
