using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CoroutineActions;
using Game.Core.Units;
using UnityEngine;

namespace Game.Core.Actions
{
	[Serializable]
	public class WaitForCollide: CoroutineAction
	{
		[SerializeField]
		private GameObject _owner;

		public GameObject Owner
		{
			get => _owner;
			set => _owner = value;
		}
		
		[SerializeField]
		private ICollisionTrigger _trigger;

		[SerializeField]
		private BoxCollider _boxCollider;

		[SerializeField]
		private LayerMask _layer;

		protected override bool Update()
		{
			if (_owner == null || _boxCollider == null)
				return false;
			
			Vector3 colliderCenter = _owner.transform.position + _boxCollider.center;
			Vector3 colliderSize = _boxCollider.size;

			var colliders = Physics.OverlapBox(colliderCenter, colliderSize, Quaternion.identity, _layer.value);
			return colliders.Length == 0;
		}
	}
}
