using System;
using System.Collections;
using System.Collections.Generic;
using Core.CoroutineActions;
using UnityEngine;

namespace HSM
{
    public abstract class State: IDisposable
    {
        public List<TransitionInfo> Transitions { get; } = new List<TransitionInfo>();
        public Type CurrentTransitionEventId { get; set; }
        
        public StateMachine Parent { get; set; }
        
        protected bool IsInThis;

        private Coroutine _executeCoroutine;
        
        ~State()
        {
	        Dispose();
        }

        public virtual void Dispose()
        {
	        if (IsInThis)
	        {
		        OnExit();
	        }
        }

        public void Enter()
        {
            IsInThis = true;

            OnEnter();

            _executeCoroutine = Coroutiner.Start(OnExecute());
        }

        protected virtual void OnEnter()
        {
        }

        protected virtual IEnumerator OnExecute()
        {
            yield break;
        }

        public void Exit()
        {
            if (_executeCoroutine != null)
            {
                Coroutiner.Stop(_executeCoroutine);
            }
            
            IsInThis = false;

            OnExit();
        }

        protected virtual void OnExit()
        {
        }
        
        protected internal virtual bool TryTransition<TEvent>()
        {
            return false;
        }
        
        protected virtual void DoTransition<TEvent>()
        {
            Parent?.DoTransition<TEvent>();
        }
    }
}