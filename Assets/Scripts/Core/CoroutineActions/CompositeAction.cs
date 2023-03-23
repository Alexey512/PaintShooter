using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.CoroutineActions
{
    [Serializable]
    public abstract class CompositeAction: CoroutineAction
    {
        [SerializeReference, SubclassSelector] 
        private List<CoroutineAction> _actions = new List<CoroutineAction>();

        public List<CoroutineAction> Actions
        {
            get => _actions;
            set => _actions = value;
        }

        public CompositeAction()
        {
        }

        public CompositeAction(params CoroutineAction[] actions)
        {
            _actions.Clear();
            _actions.AddRange(actions);
        }
    }
}
