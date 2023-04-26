using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Core.Factories;
using Game.Core.UI;
using Game.ZigZag.Configs;
using Game.ZigZag.Events;
using Game.ZigZag.Factories;
using UnityEngine;
using Zenject;

namespace Game.ZigZag
{
	public class ZigZagBootstart: MonoInstaller
	{
		[SerializeField]
		private ZigZagFactory _actorsFactory;

		[SerializeField]
		private GameConfig _gameConfig;

		//private GameStateMachine _stateMachine;
		
		public override void InstallBindings()
		{
			SignalBusInstaller.Install(Container);
			Container.DeclareSignal<TapScreenEvent>();
			
			//Container.DeclareSignal<GameOverEvent>();
			//Container.DeclareSignal<ResumeGameEvent>();

			Container.Bind<IGameConfig>().To<GameConfig>().FromScriptableObject(_gameConfig).AsSingle();
			Container.Bind<IActorsFactory<ActorType>>().To<ZigZagFactory>().FromScriptableObject(_actorsFactory).AsSingle();

			
			//Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
			//Container.Bind<ILoadScreen>().To<LoadScreen>().FromComponentOn(_loadScreen.gameObject).AsSingle();
		}

		public override void Start()
		{
			//_stateMachine = Container.Instantiate<GameStateMachine>();
			//_stateMachine.Enter();
		}
	}
}
