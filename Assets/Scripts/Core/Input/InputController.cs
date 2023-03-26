using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Core.Input
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

		public bool Rotate;

		public Transform ActionPoint => _actionPoint;

#if ENABLE_INPUT_SYSTEM

		public void OnMove(InputAction.CallbackContext context)
		{
			MoveInput(context.ReadValue<Vector2>());
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			if (CursorInputForLook)
			{
				LookInput(context.ReadValue<Vector2>());
			}
		}

		public void OnJump(InputAction.CallbackContext context)
		{
			if (context.started)
				JumpInput(context.started);
		}

		public void OnSprint(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				SprintInput(context.started);
			}
		}

		public void OnShoot(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				ShootInput(context.started);
			}
		}

		public void OnRotate(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				RotateInput(true);
			}
			else if (context.canceled)
			{
				RotateInput(false);
			}
		}

		public void OnEscape(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				SetCursorLock(false);
			}
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
			SetCursorLock(CursorLocked);
		}

		public void RotateInput(bool isRotate)
		{
			Rotate = isRotate;
			SetCursorLock(CursorLocked);
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorLock(CursorLocked);
		}

		private void SetCursorLock(bool isLock)
		{
			Cursor.lockState = isLock ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}