using System.Collections;
using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public class AirMovementState : PlatformerPlayerMovementState {
        private bool applyCoyoteTimeJump = false;
        private float midAnimationVelThreshold => PlatformerPlayerPhysicsConfig.JumpPower * -.2f;
        private float risingAnimationVelThreshold => PlatformerPlayerPhysicsConfig.JumpPower * .2f;
        public override void Initialize() {
            base.Initialize();
        }

        public override void OnUpdate() {
            inputMovementWrapper.jumpInputManager.CheckForJumpInput();
            if (PlatformerPlayerState.AllowedToDash() && inputMovementWrapper.WasDashPressedThisFrame()) {
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Dash);
            }
            if (characterController.RbVelocity.y < midAnimationVelThreshold) {
                PlatformerPlayerAnimationManager.CrossFadeToAir(PlatformerPlayerAnimationManager.AirStates.Fall);
            } else if (characterController.RbVelocity.y < risingAnimationVelThreshold) {
                PlatformerPlayerAnimationManager.CrossFadeToAir(PlatformerPlayerAnimationManager.AirStates.Mid);
            }
            else {
                PlatformerPlayerAnimationManager.CrossFadeToAir(PlatformerPlayerAnimationManager.AirStates.Rise);
            }
        }

        public override void OnFixedUpdate(ref Vector2 currentVelocity) {
            currentVelocity.y -= characterController.CurrentGravity * Time.fixedDeltaTime;
            if (applyCoyoteTimeJump) {
                ApplyJump(ref currentVelocity);
                applyCoyoteTimeJump = false;
            } else if (PlatformerPlayerState.NumJumpsAvailable > 0 &&
                       inputMovementWrapper.jumpInputManager.ConsumeJumpInput()) {
                ApplyJump(ref currentVelocity);
                PlatformerPlayerState.DecrementNumJumps();
            }

            float currentMovementInput = componentContainer.InputMovementWrapper.ReadHorizontalMovementInput();
            float targetSpeed = currentMovementInput * PlatformerPlayerPhysicsConfig.MaxMovementSpeed;
            float speedDif = targetSpeed - currentVelocity.x;

            float accelRate = PlatformerPlayerPhysicsConfig.AirInputAcceleration;

            float movement = accelRate * speedDif * Time.fixedDeltaTime;
            currentVelocity.x += movement * Time.fixedDeltaTime;

            if (characterController.Grounded) {
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Ground);
            }

            currentVelocity += componentContainer.PlatformerPlayerForceFieldDetector.CurrentForceFieldForce;

            if (currentVelocity.x > 0) {
                PlatformerPlayerState.facingRight = true;
            }

            if (currentVelocity.x < 0) {
                PlatformerPlayerState.facingRight = false;
            }
        }

        public override void OnEnter(PlatformerPlayerMovementStateId prevState) {
            applyCoyoteTimeJump = false;
            if (prevState == PlatformerPlayerMovementStateId.Ground) {
                PlatformerPlayerState.DecrementNumJumps();
                StartCoroutine(CheckForCoyoteTimeJump());
            }
        }

        public override void OnExit(PlatformerPlayerMovementStateId newState) {
            StopAllCoroutines();
        }

        private IEnumerator CheckForCoyoteTimeJump() {
            float coyoteTimeTimer = PlatformerPlayerPhysicsConfig.CoyoteTime;
            while (coyoteTimeTimer > 0) {
                if (inputMovementWrapper.jumpInputManager.ConsumeJumpInput()) {
                    applyCoyoteTimeJump = true;
                    break;
                }
                coyoteTimeTimer -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

        }

        public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.Air;
    }
}