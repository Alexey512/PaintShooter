using UnityEngine;

namespace StarterAssets
{
    public class UICanvasInputController : MonoBehaviour
    {

        [Header("Output")]
        public InputController InputController;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            InputController.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            InputController.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            InputController.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            InputController.SprintInput(virtualSprintState);
        }
        
    }

}
