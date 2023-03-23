using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Actors;
using ECS;
using Game.Components;
using Game.Components.Slime;
using Game.Units;
using UnityEngine;
using Zenject;

namespace Game.Systems.Slime
{
	[Serializable]
	public class SlimePlayerMoveSystem: BaseSystem, IAfterEntityInitialize
	{
		private SlimePlayerComponent _player;

		private bool _isMoving;

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
		}
	}
}
