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
	public class EntityConfig: IEntityConfig
	{
		[SerializeReference, SubclassSelector]
		private List<IComponent> _components = new List<IComponent>();

		public List<IComponent> Components => _components;

        public void Initialize(IEntity entity)
		{
			if (entity == null)
				return;

			entity.Clear();

			foreach (var component in _components)
			{
				if (component == null)
					continue;
				entity.AddComponent(component);
			}
		}
	}
}
