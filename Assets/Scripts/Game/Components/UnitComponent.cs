using System;
using ECS;
using UnityEngine;

namespace Game.Components
{
	public enum UnitColor
	{
		Color1,
		Color2,
		Color3,
		Color4
	}
	
	[Serializable]
	public class UnitComponent: BaseComponent
	{
		[SerializeField]
		public UnitColor Color = 0;
	}
}
