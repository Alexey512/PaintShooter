using Core.CoroutineActions;
using UnityEngine;

namespace Core.Actions
{
    [ExecuteInEditMode]
    public class MonoAction: MonoBehaviour
    {
        [SerializeReference]
        private CoroutineAction _action;

        [SerializeField]
        private bool _executeOnStart;

        private void Start()
        {
	        if (_executeOnStart)
	        {
                Execute();
	        }
        }

        public void Execute()
        {
            if (_action != null && !_action.IsExecuting)
            {
                _action.Execute();
            }
        }
    }
}
