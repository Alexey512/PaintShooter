using System;
using UnityEngine;

namespace Core.CoroutineActions
{
    [Serializable]
    public class Wait: CoroutineAction
    {
        [SerializeField]
        private float _time = 0.0f;

        public float Time
        {
            get => _time;
            set => _time = value;
        }

        private float _leftTime = 0.0f;

        protected override void OnStarted()
        {
            _leftTime = _time;
        }

        protected override bool Update()
        {
            _leftTime -= UnityEngine.Time.deltaTime;
            return _leftTime > 0.0f;
        }
    }
}
