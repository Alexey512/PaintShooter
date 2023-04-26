using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS;
using Game.Core.Actors;
using Zenject;

namespace Game.Core.Factories
{
	public interface IActorsFactory<in T>: IFactory<T, IActor> where T : Enum
	{
		void Release(T type, IActor actor);
	}
}
