using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS;
using Game.Components;
using Game.Components.Slime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Systems.Slime
{
	[Serializable]
	public class SlimePlayerAttackSystem: SlimeAttackSystem
	{
		private SlimePlayerComponent _player;

		private Vector3? _attackPosition;

		public override void AfterEntityInitialize()
		{
			base.AfterEntityInitialize();

			_player = Owner.GetSingleComponent<SlimePlayerComponent>();
		}
		
		public override void Update()
		{
			if (_player == null)
				return;
			
			Vector3 playerPosition = Actor.GameObject.transform.position;

			_attackPosition = null;

			var enemies = EntityManager.Instance.GetComponents<SlimeEnemyComponent>();
			var nearEnemy = enemies.FirstOrDefault(e => e.Actor.GameObject.transform.position.x - playerPosition.x < _player.FightDistance);
			if (nearEnemy != null)
			{
				_attackPosition = nearEnemy.Actor.GameObject.transform.position;
			}

			base.Update();
		}

		protected override bool TryGetEnemyPosition(out Vector3 position)
		{
			if (_attackPosition == null)
			{
				position = Vector3.zero;
				return false;
			}
			position = _attackPosition.Value;
			return true;
		}
	}
}
