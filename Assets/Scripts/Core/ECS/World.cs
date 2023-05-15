using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS;

namespace ECS
{
	public class World: Entity
	{
		public List<IEntity> Entities => _entities;
		
		private readonly List<IEntity> _entities = new List<IEntity>();

		public void AddEntity(IEntity entity)
		{
			_entities.Add(entity);
		}

		public void RemoveEntity(IEntity entity)
		{
			_entities.Remove(entity);
		}


	}
}
