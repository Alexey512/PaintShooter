using System;
using DG.Tweening;
using UnityEngine;

namespace Core.CoroutineActions
{
	[Serializable]
	public class MoveTo: CoroutineAction
	{
		[SerializeField]
		private Vector3 _targetPosition;

		[SerializeField] 
		private float _speed;

		[SerializeField]
		private GameObject _owner;

		public GameObject Owner
		{
			get => _owner;
			set => _owner = value;
		}

		public Vector3 TargetPosition
		{
			get => _targetPosition;
			set => _targetPosition = value;
		}

		public float Speed
		{
			get => _speed;
			set => _speed = value;
		}

		private bool _isComplete;

		protected override void OnStarted()
		{
			if (_owner == null)
			{
				_isComplete = true;
				return;
			}

			_owner.transform.DOMove(_targetPosition, _speed).SetEase(Ease.Linear).SetSpeedBased(true).OnComplete(() =>
			{
				_isComplete = true;
			});
		}

		protected override bool Update()
		{
			return !_isComplete;
		}
	}
}
