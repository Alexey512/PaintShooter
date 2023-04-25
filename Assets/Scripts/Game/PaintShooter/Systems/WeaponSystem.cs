using System;
using Core.Input;
using ECS;
using Game.PaintShooter.Components;
using Game.PaintShooter.Units;
using UnityEngine;
using Zenject;

namespace Game.PaintShooter.Systems
{
	[Serializable]
	public class WeaponSystem: BaseSystem, IAfterEntityInitialize
	{
		private bool _isInitialized;

		[Inject]
		private IFactory<BulletUnit> _bulletFactory;

		private InputController _input;

		public override void Initialize()
		{
			_input = GameObject.FindObjectOfType<InputController>();
		}

		public void AfterEntityInitialize()
		{
			var weapon = Owner.GetSingleComponent<WeaponComponent>();
			if (weapon == null)
				return;

			_isInitialized = true;
		}

		public override void Update()
		{
			if (!_isInitialized || _input == null)
				return;

			if (_input.Shoot)
			{
				Shoot();
				_input.Shoot = false;
			}
		}

		private void Shoot()
		{
			if (_bulletFactory == null)
				return;

			var bullet = _bulletFactory.Create();
			if (bullet == null)
				return;

			bullet.transform.position = Actor.GameObject.transform.position;

			if (_input.ActionPoint != null)
			{
				var startPos = _input.ActionPoint.transform.position;
				bullet.transform.position = startPos;
				//var endPos = _input.ActionPoint.rotation * Vector3.forward * 100.0f;
				var endPos = startPos + _input.ActionPoint.forward * 100.0f;

				bullet.Move(endPos);
			}
		}
	}
}
