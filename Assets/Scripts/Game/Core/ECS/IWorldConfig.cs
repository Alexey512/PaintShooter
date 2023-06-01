using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ECS;

namespace Game.Core.ECS
{
    public interface IWorldConfig: IEntityConfig
    {
        List<ISystem> Systems { get; }
    }
}
