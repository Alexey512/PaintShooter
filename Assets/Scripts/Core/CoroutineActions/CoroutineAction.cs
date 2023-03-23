using System;
using System.Collections;
using UnityEngine;

namespace Core.CoroutineActions
{
    [Serializable]
    public abstract class CoroutineAction : IEnumerator, ICoroutineAction
    {
        public event Action<CoroutineAction> Started;
        
        public event Action<CoroutineAction> Paused;
        
        public event Action<CoroutineAction> Terminated;
        
        public event Action<CoroutineAction> Done;

        private CoroutineAction _current;

        private object _routine;

        public bool IsExecuting { get; private set; }
        
        public bool IsPaused { get; private set; }

        private bool IsStopped { get; set; }

        public void Pause()
        {
            if (IsExecuting && !IsPaused)
            {
                IsPaused = true;

                OnPaused();
                Paused?.Invoke(this);
            }
        }

        public void Resume()
        {
            IsPaused = false;
            OnResumed();
        }

        public void Terminate()
        {
            if (Stop())
            {
                OnTerminated();
                Terminated?.Invoke(this);
            }
        }

        private bool Stop()
        {
            if (IsExecuting)
            {
                if (_routine is Coroutine)
                {
                    Coroutiner.Stop(_routine as Coroutine);
                }

                (this as IEnumerator).Reset();

                IsExecuting = false;
                IsStopped = true;
                return true;
            }

            return false;
        }

        public CoroutineAction Execute()
        {
            if (_current != null)
            {
                return this;
            }

            //TODO: Refactoring
            (this as IEnumerator).Reset();

            if (!IsExecuting)
            {
                IsExecuting = true;
                
                OnStarted();
                Started?.Invoke(this);
                
                _routine = Coroutiner.Start(this);

                return this;
            }

            Debug.LogWarning($"Instruction { GetType().Name} is already executing.");
            return this;
        }
        
        public void Reset()
        {
            Terminate();

            Started = null;
            Paused = null;
            Terminated = null;
            Done = null;
        }

#region IEnumerator

        object IEnumerator.Current => _current;

        bool IEnumerator.MoveNext()
        {
            if (IsStopped)
            {
                (this as IEnumerator).Reset();
                return false;
            }

            if (!IsExecuting)
            {
                IsExecuting = true;
                
                OnStarted();
                Started?.Invoke(this);
                
                _routine = new object();
            }

            if (_current != null)
                return true;

            if (IsPaused)
                return true;

            if (!Update())
            {
                OnDone();
                Done?.Invoke(this);

                IsStopped = true;
                IsExecuting = false;
                return false;
            }

            return true;
        }

        void IEnumerator.Reset()
        {
            IsPaused = false;
            IsStopped = false;
            IsExecuting = false;

            _routine = null;
        }

#endregion

        protected virtual void OnStarted() { }
        protected virtual void OnPaused() { }
        protected virtual void OnResumed() { }
        protected virtual void OnTerminated() { }
        protected virtual void OnDone() { }

        protected virtual bool Update()
        {
            return false;
        }
    }
}
