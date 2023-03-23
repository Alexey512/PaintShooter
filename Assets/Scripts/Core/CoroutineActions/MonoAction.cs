using Core.CoroutineActions;
using UnityEngine;

namespace Core.Actions
{
    [ExecuteInEditMode]
    public class MonoAction: MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        private CoroutineAction _action;

        public void Execute()
        {
            if (_action != null && !_action.IsExecuting)
            {
                _action.Execute();
            }
        }
    }
}
