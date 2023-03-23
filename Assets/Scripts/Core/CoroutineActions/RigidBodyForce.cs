using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.CoroutineActions
{
	[Serializable]
	public class RigidBodyForce: CoroutineAction
	{
		[SerializeField]
		private Rigidbody _owner;

		[SerializeField]
		private Vector3 _force;

		[SerializeField]
		private ForceMode _forceMode = ForceMode.Impulse;

		public Rigidbody Owner
		{
			get => _owner;
			set => _owner = value;
		}

		public Vector3 Force
		{
			get => _force;
			set => _force = value;
		}

		protected override void OnStarted()
		{
			if (_owner)
			{
				_owner.AddForce(_force, _forceMode);
				_owner = null;
			}
		}
	}
}
