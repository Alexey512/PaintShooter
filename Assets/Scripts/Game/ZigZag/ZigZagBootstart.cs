using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Core.UI;
using UnityEngine;
using Zenject;

namespace Game.ZigZag
{
	public class ZigZagBootstart: MonoInstaller
	{
		[SerializeField]
		private LoadScreen _loadScreen;
		
		//private GameStateMachine _stateMachine;
		
		public override void InstallBindings()
		{
			SignalBusInstaller.Install(Container);
			//Container.DeclareSignal<LoadCompleteEvent>();
			//Container.DeclareSignal<GameOverEvent>();
			//Container.DeclareSignal<ResumeGameEvent>();
			
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
