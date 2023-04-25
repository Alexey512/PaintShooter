using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.PaintShooter.Visual.Weapon
{
	public class WeaponContainer: MonoBehaviour, IWeaponContainer
	{
		[SerializeField]
		private Transform _owner;

		public Transform Owner => _owner;

		public Vector3 Position => _owner != null ? _owner.position : Vector3.zero;
		
		//TODO: добавить выбор направления
		public Vector3 Direction => _owner != null ? _owner.right : Vector3.zero;
	}
}
