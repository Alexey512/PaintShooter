using System;
using System.Collections.Generic;
using System.Linq;
using Core.CoroutineActions;
using Core.ECS;
using Game.Core.Units;
using Game.PaintShooter.Components;
using UnityEngine;

namespace Game.PaintShooter.Systems
{
	[Serializable]
	public class DestroyOnCollisionSystem: BaseSystem, IUpdateSystem
    {
        public void OnUpdate(float deltaTime)
        {
            foreach (var collisionComponent in World.Filter<CollisionComponent>().With<BulletComponent>().Select(e => e.FindComponent<CollisionComponent>()))
            {
                if (collisionComponent.Collisions.Any(c => c.CollisionType == CollisionType.Enter))
                {
                    collisionComponent.Collisions.Clear();
                    World.RemoveEntity(collisionComponent.Actor);
                }
            }
        }
    }
}
