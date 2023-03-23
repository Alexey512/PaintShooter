using System;

namespace Core.CoroutineActions
{
    public interface ICoroutineAction
    {
        bool IsExecuting { get; }
        
        bool IsPaused { get; }

        CoroutineAction Execute();
        
        void Pause();
        
        void Resume();
        
        void Terminate();

        event Action<CoroutineAction> Started;
        
        event Action<CoroutineAction> Paused;
        
        event Action<CoroutineAction> Done;
    }
}
