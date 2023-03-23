using System;
using UnityEngine;

namespace Core.CoroutineActions
{
    [Serializable]
    public class AnimationSetTrigger: CoroutineAction
    {
        [SerializeField]
        private Animator _animation;

        [SerializeField]
        private string _name;

        public Animator Animation
        {
            get => _animation;
            set => _animation = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        protected override void OnStarted()
        {
            if (_animation != null && !string.IsNullOrWhiteSpace(_name))
            {
                _animation.SetTrigger(_name);
            }
        }
    }
}
