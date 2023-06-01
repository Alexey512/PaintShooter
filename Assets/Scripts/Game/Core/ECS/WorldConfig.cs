using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ECS;
using UnityEngine;

namespace Game.Core.ECS
{
    [Serializable]
    public class WorldConfig: EntityConfig, IWorldConfig
    {

        [SerializeReference, SubclassSelector]
        private List<ISystem> _systems = new List<ISystem>();

        public List<ISystem> Systems => _systems;


        public void Initialize(IWorld world)
        {
            foreach (var system in _systems)
            {
                if (system == null)
                    continue;
                world.AddSystem(system);
            }

            base.Initialize(world);
        }
    }
}
