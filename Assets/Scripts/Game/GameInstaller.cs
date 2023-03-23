using Core.Paint;
using Game.Factories;
using Game.Units;
using UnityEngine;
using Zenject;

namespace Game
{
	public class GameInstaller: MonoInstaller
	{
		[SerializeField]
		private BulletFactory _bulletFactory;

		[SerializeField]
		private PaintManager _paintManager;

		public override void InstallBindings()
		{
			Container.Bind<IFactory<BulletUnit>>().To<BulletFactory>().FromScriptableObject(_bulletFactory).AsSingle();
			Container.Bind<IPaintManager>().To<PaintManager>().FromComponentsOn(_paintManager.gameObject).AsSingle();
		}

		public override void Start()
		{
		}
	}
}
