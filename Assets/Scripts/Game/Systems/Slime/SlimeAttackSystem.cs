using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utility;
using ECS;
using Game.Components;
using Game.Units;
using Game.Visual.Weapon;
using UnityEngine;
using Zenject;

namespace Game.Systems.Slime
{
	public abstract class SlimeAttackSystem: BaseSystem, IAfterEntityInitialize
	{
		[Inject]
		private IFactory<BulletUnit> _bulletFactory;
		
		private IWeaponContainer _weaponContainer;

		private WeaponComponent _weapon;

		private readonly IntervalCallback _attack = new IntervalCallback();

		private bool _isInitialize;
		
		public override void Initialize()
		{
			_weaponContainer = Actor.GameObject.GetComponent<IWeaponContainer>();
		}

		public virtual void AfterEntityInitialize()
		{
			_weapon = Actor.GetSingleComponent<WeaponComponent>();
			_isInitialize = _weapon != null && _weaponContainer != null && _bulletFactory != null;

			if (_isInitialize)
			{
				_attack.SetCallback(Attack);
				_attack.IsRunning = true;
			}
		}

		public override void Update()
		{
			if (!_isInitialize)
				return;

			_attack.Interval = _weapon.RateOfFire;
			_attack?.Update();
		}

		protected abstract bool TryGetEnemyPosition(out Vector3 position);

		private void Attack()
		{
			Vector3 attackPosition;
			if (!TryGetEnemyPosition(out attackPosition))
			{
				return;
			}

			var bullet = _bulletFactory.Create();
			if (bullet == null)
				return;

			bullet.transform.position = _weaponContainer.Position;
			bullet.Move(attackPosition);
		}
	}
}
