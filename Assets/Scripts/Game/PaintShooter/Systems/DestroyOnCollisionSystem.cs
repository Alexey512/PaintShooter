using System;
using System.Collections.Generic;
using Core.CoroutineActions;
using ECS;
using Game.Core.Units;
using UnityEngine;

namespace Game.PaintShooter.Systems
{
	[Serializable]
	public class DestroyOnCollisionSystem: BaseSystem, IAfterEntityInitialize, IAfterEntityDeInitialize
	{
		private ICollisionTrigger _collisionTrigger;

		private bool _isCollided;

		public void AfterEntityInitialize()
		{
			_collisionTrigger = Actor.GameObject.GetComponentInChildren<ICollisionTrigger>();
			if (_collisionTrigger != null)
			{
				_collisionTrigger.CollisionEnter += OnCollisionEnter;
			}
		}

		public void AfterEntityDeInitialize()
		{
			if (_collisionTrigger != null)
			{
				_collisionTrigger.CollisionEnter -= OnCollisionEnter;
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (_isCollided)
				return;

			_isCollided = true;

			new Sequence
			{
				Actions = new List<CoroutineAction>
				{
					new Wait { Time = 0.01f },
					new DestroyGameObject { Owner = Actor.GameObject }
				}
			}.Execute();
		}
	}
}
