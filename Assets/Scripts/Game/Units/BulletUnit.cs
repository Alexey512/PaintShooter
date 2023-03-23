using Core.CoroutineActions;
using Core.Utility.Extensions;
using UnityEngine;

namespace Game.Units
{
	public class BulletUnit: MonoBehaviour
	{
		[SerializeReference, SubclassSelector]
		private CoroutineAction _behaviourAction;

		public void Move(Vector3 vector)
		{
			if (_behaviourAction == null)
				return;

			var moveAction = _behaviourAction.FindAction<MoveTo>();
			if (moveAction != null)
			{
				moveAction.TargetPosition = vector;
			}
			else
			{
				var forceAction = _behaviourAction.FindAction<RigidBodyForce>();
				if (forceAction != null)
				{
					forceAction.Force = vector;
				}
			}

			_behaviourAction.Execute();
		}
	}
}
