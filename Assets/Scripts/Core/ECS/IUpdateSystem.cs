using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ECS
{
    public interface IUpdateSystem
    {
        void OnUpdate(float deltaTime);
    }

    public interface ILateUpdateSystem
    {
        void OnLateUpdate(float deltaTime);
    }

    public interface IFixedUpdateSystem
    {
        void OnFixedUpdate(float deltaTime);
    }
}
