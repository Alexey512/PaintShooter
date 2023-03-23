using HSM;
using Zenject;

namespace Game.States
{
	public class GameStateMachine: StateMachine
	{
		private readonly DiContainer _container;

		private readonly SignalBus _signalBus;

		public GameStateMachine(DiContainer container, SignalBus signalBus)
		{
			_container = container;
			_signalBus = signalBus;
		}

		protected override void OnInitialize()
		{
			RegisterTransitionEvent<LoadCompleteEvent>();
			RegisterTransitionEvent<GameOverEvent>();
			RegisterTransitionEvent<ResumeGameEvent>();
			
			var loadState = AddState<LoadGameState>();
			var gameState = AddState<GameState>();
			var gameOverState = AddState<GameOverState>();

			loadState.AddTransition<LoadCompleteEvent>(gameState);
			gameState.AddTransition<GameOverEvent>(gameOverState);
			gameOverState.AddTransition<ResumeGameEvent>(gameState);
		}

		protected override State AssignState(StateInfo stateInfo)
		{
			var state = base.AssignState(stateInfo);
			if (state != null)
			{
				_container.Inject(state);
			}
			return state;
		}

		private void RegisterTransitionEvent<T>()
		{
			_signalBus?.Subscribe<T>(DoTransition<T>);
		}
	}
}
