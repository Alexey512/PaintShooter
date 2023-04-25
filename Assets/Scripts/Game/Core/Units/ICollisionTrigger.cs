using System;
using UnityEngine;

namespace Game.Core.Units
{
	public interface ICollisionTrigger
	{
		event Action<Collision> CollisionEnter;

		event Action<Collision> CollisionExit;
		
		event Action<Collision> CollisionStay;
		
		event Action<Collider> TriggerEnter;
		
		event Action<Collider> TriggerExit;
		
		event Action<Collider> TriggerStay;
	}
}
