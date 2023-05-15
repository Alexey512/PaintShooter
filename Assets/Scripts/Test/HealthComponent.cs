using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Test
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	[System.Serializable]
	public struct HealthComponent : IComponent 
	{
		public int healthPoints;
	}
}
