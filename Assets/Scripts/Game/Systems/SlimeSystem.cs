using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Actors;
using ECS;
using Game.Components;
using Game.Units;
using UnityEngine;
using Zenject;

namespace Game.Systems
{
	[Serializable]
	public class SlimeSystem: BaseSystem, IAfterEntityInitialize
	{
		[Inject]
		private IFactory<BulletUnit> _bulletFactory;

		private SlimePlayerComponent _player;

		private bool _isMoving;

		private bool _isShooting;

		private float _shootTime;

		public void AfterEntityInitialize()
		{
			_player = Owner.GetSingleComponent<SlimePlayerComponent>();
		}

		public override void Update()
		{
			if (_player == null)
				return;
			
			var enemies = EntityManager.Instance.GetComponents<SlimeEnemyComponent>();

			Vector3 playerPosition = Actor.GameObject.transform.position;

			_isMoving = enemies == null || !enemies.Any((enemy) => enemy.Actor.GameObject.transform.position.x - playerPosition.x < _player.FightDistance);

			if (_isMoving)
			{
				Actor.GameObject.transform.position += Vector3.right * _player.Speed * Time.deltaTime;
			}
			else
			{
				if (_shootTime < 0.0f)
				{
					var enemyPosition = enemies.FirstOrDefault().Actor.GameObject.transform.position;
					Shoot(enemyPosition);

					_shootTime = _player.RateOfFire;
				}
				else
				{
					_shootTime -= Time.deltaTime;
				}				
			}
		}

		private void Shoot(Vector3 targetPoint)
		{
			if (_bulletFactory == null)
				return;

			var bullet = _bulletFactory.Create();
			if (bullet == null)
				return;

			var startPos = Actor.GameObject.transform.position;
			bullet.transform.position = startPos;
			var endPos = targetPoint;

			bullet.Move(endPos);
		}

		
	}
}
