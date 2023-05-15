using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Utility.Extensions;

namespace Core.CoroutineActions
{
    [Serializable]
    public class FadeTo: CoroutineAction
    {
        [SerializeField]
        private List<SpriteRenderer> _sprites = new List<SpriteRenderer>();

        [SerializeField]
        private GameObject _owner;

        [SerializeField]
        private float _fade = 0.0f;

        [SerializeField]
        private float _time = 0.0f;

        private bool _isComplete;

        public List<SpriteRenderer> Sprites
        {
            get => _sprites;
            set => _sprites = value;
        }

        public GameObject Owner
        {
            get => _owner;
            set => _owner = value;
        }

        public float Fade
        {
            get => _fade;
            set => _fade = value;
        }

        public float Time
        {
            get => _time;
            set => _time = value;
        }

        protected override void OnStarted()
        {
            List<SpriteRenderer> sprites = new List<SpriteRenderer>(_sprites);
            if (_owner != null)
            {
                sprites.AddRange(_owner.GetComponentsInChildren<SpriteRenderer>());
            }

            if (_time > 0.0f)
            {
                _isComplete = false;
                foreach (var sprite in sprites)
                {
	                
	                
	                sprite?.DOFade(_fade, _time).OnComplete(() => { _isComplete = true; });
                }
            }
            else
            {
                _isComplete = true;
                foreach (var sprite in sprites)
                {
                    sprite?.SetAlpha(_fade);
                }
            }
        }

        protected override bool Update()
        {
            return !_isComplete;
        }
    }
}
