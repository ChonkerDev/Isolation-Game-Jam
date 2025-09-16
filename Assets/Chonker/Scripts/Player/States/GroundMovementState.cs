using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public class GroundMovementState : PlatformerPlayerMovementState {

        public override void Initialize() {
            base.Initialize();
        }

        public override void OnUpdate() {
            inputMovementWrapper.jumpInputManager.CheckForJumpInput();
            if (PlatformerPlayerState.AllowedToDash() && inputMovementWrapper.WasDashPressedThisFrame()) {
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Dash);
            }
        }

        public override void OnFixedUpdate(ref Vector2 currentVelocity) {
            if (inputMovementWrapper.jumpInputManager.ConsumeJumpInput() && PlatformerPlayerState.NumJumpsAvailable > 0) {
                ApplyJump(ref currentVelocity);
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Air);
                return;
            }
            
            float currentMovementInput = componentContainer.InputMovementWrapper.ReadHorizontalMovementInput();
            float targetSpeed = currentMovementInput * PlatformerPlayerPhysicsConfig.MaxMovementSpeed;
            float speedDif = targetSpeed - currentVelocity.x;

            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? PlatformerPlayerPhysicsConfig.GroundAcceleration : PlatformerPlayerPhysicsConfig.GroundDeceleration;
            
            float movement = accelRate * speedDif * Time.fixedDeltaTime;

            currentVelocity.x += movement * Time.fixedDeltaTime;
            currentVelocity = Vector2.ClampMagnitude(currentVelocity, PlatformerPlayerPhysicsConfig.MaxMovementSpeed);
            currentVelocity += componentContainer.PlatformerPlayerForceFieldDetector.CurrentForceFieldForce;
            if (!characterController.Grounded) {
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Air);
            }
            if (targetSpeed > 0) {
                PlatformerPlayerState.facingRight = true;
            } 
            if (targetSpeed < 0) {
                PlatformerPlayerState.facingRight = false;
            }

            bool isIdle = Mathf.Abs(currentVelocity.x) < .02f || currentMovementInput == 0;
            PlatformerPlayerAnimationManager.CrossFadeToGround(isIdle);

        }

        public override void OnEnter(PlatformerPlayerMovementStateId prevState) {
            PlatformerPlayerState.ResetNumJumps();
            PlatformerPlayerState.ResetNumDashes();
        }

        public override void OnExit(PlatformerPlayerMovementStateId newState) {
            
        }

        public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.Ground;
    }
}