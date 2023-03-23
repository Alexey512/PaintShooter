using System;
using System.Linq;

namespace Core.CoroutineActions
{
    [Serializable]
    public class Parallel: CompositeAction
    {
        protected override void OnStarted()
        {
            foreach (var action in Actions)
            {
                action.Execute();
            }
        }

        protected override bool Update()
        {
            return Actions.Any(action => action.IsExecuting);
        }
    }
}
