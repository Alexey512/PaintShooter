using System;
using Core.ECS;
using Game.Core.ECS;
using Game.PaintShooter.Components;
using UnityEngine;

namespace Game.Core.Units
{
    public class CollisionTrigger: MonoBehaviour
    {
        [SerializeField]
        private ActorProvider _actor;

        private void OnCollisionEnter(Collision collision)
        {
            AddCollision(CollisionType.Enter, collision);
        }

        private void OnCollisionExit(Collision collision)
		{
            AddCollision(CollisionType.Exit, collision);
        }

        private void OnCollisionStay(Collision collision)
		{
            AddCollision(CollisionType.Stay, collision);
        }

        private void AddCollision(CollisionType type, Collision collision)
        {
            if (_actor == null)
                return;
            var collisionComponent = _actor.AddComponent<CollisionComponent>();
            collisionComponent.Collisions.Add(new CollisionInfo { CollisionType = type, Collision = collision });
        }
	}
}
