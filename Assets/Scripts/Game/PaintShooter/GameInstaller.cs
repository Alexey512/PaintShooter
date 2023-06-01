using Core.Paint;
using Core.ECS;
using Game.Core.ECS;
using Game.PaintShooter.Components;
using Game.PaintShooter.Factories;
using UnityEngine;
using Zenject;
using static Zenject.CheatSheet;

namespace Game.PaintShooter
{
	public class GameInstaller: MonoInstaller
	{
		[SerializeField]
		private PaintManager _paintManager;

        [SerializeField]
        private WorldProvider _world;

        [SerializeField]
        private GameObject _bulletPrefab;

		public override void InstallBindings()
		{
            Container.BindFactory<ActorProvider, BulletFactory>()
                .FromPoolableMemoryPool<ActorProvider, BulletPool>(poolBinder => poolBinder
                    //.WithInitialSize(20)
                    .FromComponentInNewPrefab(_bulletPrefab)
                    .UnderTransformGroup("Bullets"));

            Container.Bind<IPaintManager>().To<PaintManager>().FromComponentsOn(_paintManager.gameObject).AsSingle();
            Container.Bind<IWorld>().To<WorldProvider>().FromComponentsOn(_world.gameObject).AsSingle();
        }

        public override void Start()
		{
        }
    }
}
