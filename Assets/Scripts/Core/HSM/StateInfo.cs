using System;
using System.Collections.Generic;

namespace HSM
{
    public class StateInfo
    {
        public Type StateType { get; }

        public List<TransitionInfo> Transitions { get; } = new List<TransitionInfo>();

        public StateInfo(Type type)
        {
            StateType = type;
        }

        public StateInfo AddTransition<TTransitionEvent>(StateInfo state)
        {
            var transition = new TransitionInfo(typeof(TTransitionEvent), this, state);
            Transitions.Add(transition);
            return this;
        }
    }
}
