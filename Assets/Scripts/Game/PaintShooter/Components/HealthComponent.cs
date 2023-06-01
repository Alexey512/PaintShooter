using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ECS;

namespace Game.PaintShooter.Components
{
	[Serializable]
	public class HealthComponent: CounterComponent<int>
    {
    }
}
