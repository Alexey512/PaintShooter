using UnityEngine;

namespace Core.Factories
{
	public interface IGameObjectsFactory
	{
		public GameObject Instantiate();

		void Release(GameObject obj);
	}
}
