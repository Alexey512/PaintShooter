using System;
using Core.Paint;
using ECS;
using Game.Units;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Systems
{
	[Serializable]
	public class CollisionPaintSystem: BaseSystem, IAfterEntityInitialize, IAfterEntityDeInitialize
	{
		private ICollisionTrigger _collisionTrigger;

		[Inject]
		private IPaintManager _paintManager;

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
			var bush = _paintManager.DefaultBrush.Clone();
			bush.Channel = Random.Range(0, 4);
			bush.Scale = Random.Range(2.0f, 3.0f);
			bush.Rotation = Random.Range(-180, 180);

			_paintManager.PaintCollision(collision, bush);
		}
	}
}
