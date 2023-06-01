using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ECS;
using UnityEngine;

namespace Game.PaintShooter.Components
{
    public enum CollisionType
    {
        None,
        Enter,
        Exit,
        Stay
    }

    public class CollisionInfo
    {
        public CollisionType CollisionType;

        public Collision Collision;
    }

    [Serializable]
    public class CollisionComponent: BaseComponent
    {
        [HideInInspector]
        public List<CollisionInfo> Collisions = new List<CollisionInfo>();
    }
}
