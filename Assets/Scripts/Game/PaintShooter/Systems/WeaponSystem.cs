using System;
using System.Linq;
using Core.Input;
using Core.ECS;
using Game.Core.ECS;
using Game.PaintShooter.Components;
using UnityEngine;
using Zenject;
using Game.PaintShooter.Factories;

namespace Game.PaintShooter.Systems
{
	[Serializable]
	public class WeaponSystem: BaseSystem, IAfterEntityInitialize, IUpdateSystem
	{
        [Inject] 
        private BulletFactory _bulletFactory;

        private InputController _input;

        private EntityFilter _weaponFilter;

        private bool _isInitialized;

        public void AfterEntityInitialize()
        {
            //TODO: Inject
            _input = GameObject.FindObjectOfType<InputController>();

            _weaponFilter = World.Filter<WeaponComponent>();
        }

		private void Shoot()
        {
			var bulletEntity = _bulletFactory.Create();
			if (bulletEntity == null)
				return;

			if (_input.ActionPoint != null)
			{
				var startPos = _input.ActionPoint.transform.position;

                var bullet = bulletEntity.FindComponent<BulletComponent>();
                bullet.Position = startPos;
                bullet.Direction = _input.ActionPoint.forward.normalized;
                bullet.LeftTime = bullet.LifeTime;

                bullet.Actor.Owner.transform.position = startPos;
            }
		}

        public void OnUpdate(float deltaTime)
        {
            if (_input == null)
                return;

            if (_input.Shoot)
            {
                Shoot();
                _input.Shoot = false;
            }
        }
    }
}
