using Core.Factories;
using Game.Units;
using UnityEngine;
using Zenject;

namespace Game.Factories
{
	[CreateAssetMenu(fileName = "BulletFactory", menuName = "Game/Bullet Factory", order = -1)]
	public class BulletFactory: GameObjectsFactory, IFactory<BulletUnit>
	{
		[Inject]
		private void Construct(DiContainer container)
		{
			Initialize(container);
		}

		public BulletUnit Create()
		{
			var obj = Instantiate();
			return obj != null ? obj.GetComponent<BulletUnit>() : null;
		}
	}
}
