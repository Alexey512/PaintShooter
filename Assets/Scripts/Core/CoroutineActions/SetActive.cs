using System;
using UnityEngine;

namespace Core.CoroutineActions
{
    [Serializable]
    public class SetActive: CoroutineAction
    {
        [SerializeField]
        private GameObject _target;

        [SerializeField]
        private bool _isActive;

        public GameObject Target
        {
            get => _target;
            set => _target = value;
        }

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        protected override void OnStarted()
        {
            if (_target != null)
            {
                _target.SetActive(_isActive);
            }
        }
    }
}
