using System;
using Core.Paint;
using Core.ECS;
using Game.Core.Units;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using Game.PaintShooter.Components;
using System.Linq;

namespace Game.PaintShooter.Systems
{
	[Serializable]
	public class CollisionPaintSystem: BaseSystem, IUpdateSystem
	{
		[Inject]
		private IPaintManager _paintManager;

		private void OnCollisionEnter(Collision collision)
		{
			var bush = _paintManager.DefaultBrush.Clone();
			bush.Channel = Random.Range(0, 4);
			bush.Scale = Random.Range(2.0f, 3.0f);
			bush.Rotation = Random.Range(-180, 180);

			_paintManager.PaintCollision(collision, bush);
		}

        public void OnUpdate(float deltaTime)
        {
            foreach (var collisionComponent in World.Filter<CollisionComponent>().With<BulletComponent>().Select(e => e.FindComponent<CollisionComponent>()))
            {
                foreach (var collision in collisionComponent.Collisions.Where(c => c.CollisionType == CollisionType.Enter))
                {
                    OnCollisionEnter(collision.Collision);
                }
            }
        }
    }
}
