using Core.ECS;
using Game.PaintShooter.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.PaintShooter.Systems
{
    [Serializable]
    public class MoveBulletSystem: BaseSystem, IUpdateSystem
    {
        public void OnUpdate(float deltaTime)
        {
            foreach (var bullet in World.Filter<BulletComponent>().Select(e => e.FindComponent<BulletComponent>()))
            {
                bullet.Position += bullet.Direction * bullet.Speed * deltaTime;
                bullet.Actor.Owner.transform.position = bullet.Position;

                bullet.LeftTime -= deltaTime;
                if (bullet.LeftTime < 0)
                {
                    World.RemoveEntity(bullet.Owner);
                }
            }
        }
    }
}
