using System;
using DG.Tweening;
using UnityEngine;

namespace Core.CoroutineActions
{
	[Serializable]
	public class MoveByDirection: CoroutineAction
	{
		[SerializeField]
		private Vector3 _direction;

		[SerializeField] 
		private float _speed;

		[SerializeField]
		private GameObject _owner;

		public GameObject Owner
		{
			get => _owner;
			set => _owner = value;
		}

		public Vector3 Direction
		{
			get => _direction;
			set => _direction = value;
		}

		public float Speed
		{
			get => _speed;
			set => _speed = value;
		}

		private bool _isStarted;

		private Vector3 _normalDirection;

		protected override void OnStarted()
		{
			if (_owner == null)
				return;

			_isStarted = true;
			_normalDirection = _direction.normalized;
		}

		protected override void OnTerminated()
		{
			_isStarted = false;

		}

		protected override bool Update()
		{
			if (!_isStarted)
				return false;
			
			_owner.transform.position += _normalDirection * _speed * Time.deltaTime;
			return true;
		}
	}
}
