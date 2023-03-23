using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Utility.Extensions;
using Object = UnityEngine.Object;

namespace Core.CoroutineActions
{
    [Serializable]
    public class ChangeSprite: CoroutineAction
    {
        [SerializeField]
        private SpriteRenderer _target;

        [SerializeField]
        private Sprite _image;

        [SerializeField]
        private float _time = 0.0f;

        private bool _inProgress;

        public SpriteRenderer Target
        {
            get => _target;
            set => _target = value;
        }

        public Sprite Image
        {
            get => _image;
            set => _image = value;
        }

        public float Time
        {
            get => _time;
            set => _time = value;
        }

        protected override void OnStarted()
        {
            if (_target == null)
                return;

            _inProgress = true;

            Coroutiner.Start(StartChange());
        }

        private IEnumerator StartChange()
        {
            var tmpSprite = Object.Instantiate(_target.gameObject, _target.transform.parent).GetComponent<SpriteRenderer>();
            tmpSprite.sortingOrder = _target.sortingOrder + 1;
            tmpSprite.SetAlpha(0.0f);
            tmpSprite.sprite = _image;
            yield return tmpSprite.DOFade(1.0f, _time).WaitForCompletion();
            _target.sprite = _image;
            Object.Destroy(tmpSprite.gameObject);
            _inProgress = false;
        }

        protected override bool Update()
        {
            return _inProgress;
        }
    }
}
