using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public class GroundMovementState : PlatformerPlayerMovementState {
        private bool jumpRequested;

        public override void Initialize() {
            base.Initialize();
        }

        public override void OnUpdate() {
            if (inputMovementWrapper.WasJumpPressedThisFrame() && PlatformerPlayerState.NumJumpsAvailable > 0) {
                jumpRequested = true;
            }
        }

        public override void OnFixedUpdate(ref Vector2 currentVelocity) {
            if (jumpRequested && PlatformerPlayerState.NumJumpsAvailable > 0) {
                jumpRequested = false;
                currentVelocity += Vector2.up * PlatformerPlayerPhysicsConfig.JumpForce;
                PlatformerPlayerState.DecrementNumJumps();
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Air);
                return;
            }
            
            Vector2 currentMovement = componentContainer.InputMovementWrapper.ReadMovementInput();
            float targetSpeed = currentMovement.x * PlatformerPlayerPhysicsConfig.MaxMovementSpeed;
            float speedDif = targetSpeed - currentVelocity.x;

            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? PlatformerPlayerPhysicsConfig.GroundAcceleration : PlatformerPlayerPhysicsConfig.GroundDeceleration;
            
            float movement = accelRate * speedDif * Time.fixedDeltaTime;

            currentVelocity.x += movement * Time.fixedDeltaTime;
            currentVelocity = Vector2.ClampMagnitude(currentVelocity, PlatformerPlayerPhysicsConfig.MaxMovementSpeed);
            if (!characterController.Grounded) {
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Air);
            }
        }

        public override void OnEnter() {
            PlatformerPlayerState.ResetNumJumps();
        }

        public override void OnExit() {
            
        }

        public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.Ground;
    }
}