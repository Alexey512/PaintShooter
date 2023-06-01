using Core.ECS;
using Game.Core.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Game.PaintShooter.Factories
{
    public class ActorFactory: PlaceholderFactory<ActorProvider>
    {
    }

    public class ActorPool : MonoPoolableMemoryPool<IMemoryPool, ActorProvider>
    {
    }
}
