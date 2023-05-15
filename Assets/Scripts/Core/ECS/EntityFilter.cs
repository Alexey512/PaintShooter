using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS
{
	public sealed class EntityFilter: IEnumerable<IEntity>
	{
		internal readonly List<int> IncludeTypes = new List<int>();

		internal readonly List<int> ExcludeTypes = new List<int>();

		internal World World;

		public EntityFilter(World world, int typeId)
		{
			World = world;
			IncludeTypes.Add(typeId);
		}

		public EntityFilter With<T>() where T : struct, IComponent 
		{
			IncludeTypes.Add(typeof(T).GetHashCode());
			return this;
		}

		public EntityFilter Without<T>() where T : class, IComponent 
		{
			ExcludeTypes.Add(typeof(T).GetHashCode());
			return this;
		}

		public IEnumerator<IEntity> GetEnumerator()
		{
			return new EntityEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public class EntityEnumerator: IEnumerator<IEntity>
	{
		private readonly EntityFilter _filter;

		private int _entityIndex;

		public EntityEnumerator(EntityFilter filter)
		{
			_filter = filter;
			_entityIndex = -1;
		}
		
		public bool MoveNext()
		{
			if (_entityIndex < _filter.World.Entities.Count - 1)
			{
				_entityIndex++;
				return true;
			}
			else
				return false;
		}

		public void Reset()
		{
			_entityIndex = -1;
		}

		public IEntity Current
		{
			get
			{
				if (_entityIndex < 0 || _entityIndex >= _filter.World.Entities.Count)
					throw new ArgumentException();
				return _filter.World.Entities[_entityIndex];
			}
		}

		object IEnumerator.Current => Current;

		public void Dispose()
		{
		}
	}
}
