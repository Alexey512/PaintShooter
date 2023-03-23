using System;
using System.Linq;

namespace Core.CoroutineActions
{
    [Serializable]
    public class Sequence : CompositeAction
    {
        private CoroutineAction _currentAction;

        protected override void OnStarted()
        {
            if (_currentAction != null)
                return;

            _currentAction = GetNextAction();
            _currentAction?.Execute();
        }

        private CoroutineAction GetNextAction()
        {
            if (_currentAction == null)
            {
                return Actions.FirstOrDefault();
            }
            
            int actionIndex = Actions.IndexOf(_currentAction) + 1;
            return actionIndex >= 0 && actionIndex < Actions.Count ? Actions[actionIndex] : null;
        }

        protected override bool Update()
        {
            if (_currentAction != null)
            {
                if (_currentAction.IsExecuting)
                {
                    return true;
                }    
            
                _currentAction = GetNextAction();
                if (_currentAction != null)
                {
                    _currentAction.Execute();
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}
