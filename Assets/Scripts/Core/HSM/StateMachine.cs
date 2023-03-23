using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HSM
{
    public abstract class StateMachine : State, IDisposable
    {
        public State CurrentState { get; private set; }

        public StateInfo CurrentStateInfo { get; private set; }

        private readonly List<StateInfo> _statesInfo = new List<StateInfo>();

        private readonly Dictionary<Type, List<TransitionInfo>> _transitionsInfo = new Dictionary<Type, List<TransitionInfo>>();

        private void Initialize()
        {
            OnInitialize();

            _transitionsInfo.Clear();

            foreach (var stateInfo in _statesInfo)
            {
                foreach (var transition in stateInfo.Transitions)
                {
                    AddTransition(transition);
                }
            }

            CurrentState = AssignState(_statesInfo.FirstOrDefault());
        }

        protected virtual void OnInitialize()
        {
        }

        public StateInfo AddState<T>() where T: State
        {
            var stateInfo = new StateInfo(typeof(T));
            _statesInfo.Add(stateInfo);

            foreach (var transition in stateInfo.Transitions)
            {
                AddTransition(transition);
            }
            
            return stateInfo;
        }

        protected override void OnEnter()
        {
            Initialize();

            CurrentState?.Enter();
        }

        protected override void OnExit()
        {
            CurrentState?.Exit();
            CurrentState = null;
        }

        protected virtual State AssignState(StateInfo stateInfo)
        {
            if (stateInfo == null)
                return null;

            CurrentStateInfo = stateInfo;
            var state = Activator.CreateInstance(stateInfo.StateType) as State;
            if (state != null)
            {
	            state.Parent = this;
            }
            return state;
        }

        protected override void DoTransition<TEvent>()
        {
            if (Parent != null)
            {
                base.DoTransition<TEvent>();
            }
            else
            {
                TryTransition<TEvent>();
            }
        }

        protected internal override bool TryTransition<TEvent>()
        {
            if (CurrentState.TryTransition<TEvent>())
                return false;

            var canTransition = ContainsTransition<TEvent>(CurrentStateInfo, out var resultTransition);

            if (canTransition)
            {
                Transition(resultTransition);
            }

            return canTransition;
        }

        private void AddTransition(TransitionInfo transition)
        {
            if (!_transitionsInfo.TryGetValue(transition.Event, out var transitions))
            {
                transitions = new List<TransitionInfo>(5);
                _transitionsInfo.Add(transition.Event, transitions);
            }
            else
            {
                foreach (var otherTransition in transitions)
                {
                    if (otherTransition.Equals(transition))
                    {
                        Debug.Log($"The event {transition.Event} contain transition from {transition.From} to {transition.To}");
                        return;
                    }
                }
            }

            transitions.Add(transition);
        }

        private bool ContainsTransition<TEvent>(StateInfo from, out TransitionInfo resultTransition)
        {
            resultTransition = default;

            if (!_transitionsInfo.TryGetValue(typeof(TEvent), out var transitions))
                return false;

            foreach (var transition in transitions)
            {
                if (transition.From == from || transition.From == null)
                {
                    resultTransition = transition;
                    return true;
                }
            }

            return false;
        }

        private void Transition(TransitionInfo transition)
        {
            CurrentTransitionEventId = transition.Event;

            if (CurrentState != null)
            {
                CurrentState.CurrentTransitionEventId = transition.Event;
                CurrentState.Exit();
            }

            CurrentState = AssignState(transition.To);

            if (CurrentState != null)
            {
                CurrentState.Enter();
            }
        }

        public override void Dispose()
        {
	        base.Dispose();

	        if (IsInThis)
	        {
		        OnExit();
	        }
        }
    }
}