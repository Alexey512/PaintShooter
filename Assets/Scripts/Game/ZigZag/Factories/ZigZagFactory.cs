using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Core.Factories;
using UnityEngine;

namespace Game.ZigZag.Factories
{
	[CreateAssetMenu(fileName = "ZigZagFactory", menuName = "ZigZag/ZigZag Factory", order = -1)]
	public class ZigZagFactory: ActorsFactory<ActorType>
	{
	}
}
