using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.PaintShooter.Visual.Weapon
{
	public interface IWeaponContainer
	{
		Vector3 Position { get; }

		Vector3 Direction { get; }
	}
}
