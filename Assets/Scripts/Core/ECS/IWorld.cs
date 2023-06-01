using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ECS
{
    public interface IWorld: IEntity
    {
        EntityFilter Filter<T>() where T : class, IComponent;

        void AddEntity(IEntity entity);

        void RemoveEntity(IEntity entity);

        ISystem AddSystem(ISystem system, IWorld world = null);

        T AddSystem<T>(IWorld world = null) where T : class, ISystem, new();

        void RemoveSystem(Type systemType);

        void RemoveSystem(ISystem system);

        void RemoveSystem<T>() where T : class, ISystem, new();

        T GetSystem<T>() where T : class, ISystem;

        ISystem GetSystem(Type systemType);

        IEnumerable<ISystem> GetSystems();
    }
}
