using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class InputController : MonoBehaviour
	{
		[SerializeField]
		private Transform _actionPoint;
		
		[Header("Character Input Values")]
		public Vector2 Move;
		
		public Vector2 Look;
		
		public bool Jump;
		
		public bool Sprint;

		[Header("Movement Settings")]
		public bool AnalogMovement;

		[Header("Mouse Cursor Settings")]
		public bool CursorLocked = true;
		
		public bool CursorInputForLook = true;

		public bool Shoot;

		public Transform ActionPoint => _actionPoint;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(CursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnShoot(InputValue value)
		{
			ShootInput(value.isPressed);
		}
#endif

		public void MoveInput(Vector2 newMoveDirection)
		{
			Move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			Look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			Jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			Sprint = newSprintState;
		}

		public void ShootInput(bool isShoot)
		{
			Shoot = isShoot;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(CursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}