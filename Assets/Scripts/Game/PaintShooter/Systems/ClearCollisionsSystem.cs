using Core.ECS;
using Game.PaintShooter.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.PaintShooter.Systems
{
    public class ClearCollisionsSystem : BaseSystem, IUpdateSystem
    {
        public void OnUpdate(float deltaTime)
        {
            foreach (var collisionComponent in World.Filter<CollisionComponent>().With<BulletComponent>().Select(e => e.FindComponent<CollisionComponent>()))
            {
                collisionComponent.Collisions.Clear();
            }
        }
    }
}
