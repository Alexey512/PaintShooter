using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS;
using Scripts.Core.Data;
using UnityEngine;

namespace Game.ZigZag.Components
{
	[Serializable]
	public class TileComponent: BaseComponent
	{
		public ObservableProperty<int> Size = new ObservableProperty<int>();

		public ObservableProperty<Vector2> Position = new ObservableProperty<Vector2>();

		public Material Material;
	}
}
