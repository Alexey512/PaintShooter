using Core.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.PaintShooter.Components
{
    [Serializable]
    public class BulletComponent : BaseComponent
    {
        [HideInInspector]
        public Vector3 Position = Vector3.zero;

        [HideInInspector]
        public Vector3 Direction = Vector3.zero;

        [HideInInspector]
        public float LeftTime = 0f;

        public float Speed = 10f;

        public float LifeTime = 15f;
    }
}
