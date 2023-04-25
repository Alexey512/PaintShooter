using System;
using ECS;
using UnityEngine;

namespace Game.PaintShooter.Components
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
		public UnitColor Color = 0;
	}
}
