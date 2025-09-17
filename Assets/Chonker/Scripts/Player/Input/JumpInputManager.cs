using System.Collections;
using UnityEngine;

namespace Chonker.Scripts.Player {
    public class JumpInputManager : InputManager {

        private bool jumpPressedThisFrame;
        private Coroutine ClearJumpInputDelayedCoroutine;
        public bool IsJumpHeld => inputActions.PlayerControl.Jump.IsPressed();

        public void CheckForJumpInput() {
            if (inputActions.PlayerControl.Jump.WasPressedThisFrame() && !jumpPressedThisFrame) {
                ClearJumpInputDelayedCoroutine = StartCoroutine(ClearJumpInputDelayedI());
            }
        }

        public bool ConsumeJumpInput() {
            if (jumpPressedThisFrame) {
                jumpPressedThisFrame = false;
                ClearJumpInput();
                return true;
            }
            return false;
        }

        private IEnumerator ClearJumpInputDelayedI() {
            jumpPressedThisFrame = true;
            yield return new WaitForSeconds(platformerPlayerComponentContainer.PhysicsConfigSO.JumpInputBufferTimeInSeconds);
            jumpPressedThisFrame = false;
            ClearJumpInputDelayedCoroutine = null;
        }

        public void ClearJumpInput() {
            jumpPressedThisFrame = false;
            if (ClearJumpInputDelayedCoroutine == null) return;
            StopCoroutine(ClearJumpInputDelayedCoroutine);
            ClearJumpInputDelayedCoroutine = null;
        }
    }
}