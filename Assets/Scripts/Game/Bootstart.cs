using Game.States;
using Zenject;

namespace Game
{
	public class Bootstart: MonoInstaller
	{
		private GameStateMachine _stateMachine;
		
		public override void InstallBindings()
		{
			SignalBusInstaller.Install(Container);
			Container.DeclareSignal<LoadCompleteEvent>();
			Container.DeclareSignal<GameOverEvent>();
			Container.DeclareSignal<ResumeGameEvent>();
			
			Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
		}

		public override void Start()
		{
			_stateMachine = Container.Instantiate<GameStateMachine>();
			_stateMachine.Enter();
		}
	}
}
