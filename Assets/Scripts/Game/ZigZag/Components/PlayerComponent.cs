using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS;

namespace Game.ZigZag.Components
{
	[Serializable]
	public class PlayerComponent: BaseComponent
	{
		public float Speed = 1.0f;
	}
}
