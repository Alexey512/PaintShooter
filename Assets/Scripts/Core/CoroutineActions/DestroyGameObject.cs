using System;
using UnityEngine;

namespace Core.CoroutineActions
{
	[Serializable]
	public class DestroyGameObject: CoroutineAction
	{
		[SerializeField]
		private GameObject _owner;

		public GameObject Owner
		{
			get => _owner;
			set => _owner = value;
		}

		protected override void OnStarted()
		{
			if (_owner)
			{
				GameObject.Destroy(_owner);
				_owner = null;
			}
		}
	}
}
