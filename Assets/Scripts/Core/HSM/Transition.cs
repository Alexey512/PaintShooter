using System;

namespace HSM
{
    public class TransitionInfo
    {
        public Type Event { get; }

        public StateInfo From { get; }
        
        public StateInfo To { get; }

        public TransitionInfo(Type transitionEvent, StateInfo from, StateInfo to)
        {
            Event = transitionEvent;
            From = from;
            To = to;
        }

        public override bool Equals(object obj)
        {
            var other = obj as TransitionInfo;
            if (other == null)
                return false;
            return Event == other.Event && Equals(To, other.To) && Equals(From, other.From);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Event != null ? Event.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (To != null ? To.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (From != null ? From.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}