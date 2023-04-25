using System;
using UnityEngine;

namespace Game.Core.Units
{
	public class CollisionTrigger: MonoBehaviour, ICollisionTrigger
	{
		public event Action<Collision> CollisionEnter;

		public event Action<Collision> CollisionExit;
		
		public event Action<Collision> CollisionStay;
		
		public event Action<Collider> TriggerEnter;
		
		public event Action<Collider> TriggerExit;
		
		public event Action<Collider> TriggerStay;

		private void OnCollisionEnter(Collision collision)
		{
			CollisionEnter?.Invoke(collision);
		}

		private void OnCollisionExit(Collision collision)
		{
			CollisionExit?.Invoke(collision);
		}

		private void OnCollisionStay(Collision collision)
		{
			CollisionStay?.Invoke(collision);
		}

		private void OnTriggerEnter(Collider other)
		{
			TriggerEnter?.Invoke(other);
		}

		private void OnTriggerExit(Collider other)
		{
			TriggerExit?.Invoke(other);
		}

		private void OnTriggerStay(Collider other)
		{
			TriggerStay?.Invoke(other);
		}
	}
}
