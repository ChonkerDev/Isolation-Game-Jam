using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public class GroundMovementState : PlatformerPlayerMovementState {

        public override void Initialize() {
            base.Initialize();
        }

        public override void OnUpdate() {
            inputMovementWrapper.jumpInputManager.CheckForJumpInput();
        }

        public override void OnFixedUpdate(ref Vector2 currentVelocity) {
            if (inputMovementWrapper.jumpInputManager.ConsumeJumpInput() && PlatformerPlayerState.NumJumpsAvailable > 0) {
                ApplyJump(ref currentVelocity);
                PlatformerPlayerState.DecrementNumJumps();
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
        }

        public override void OnEnter() {
            PlatformerPlayerState.ResetNumJumps();
        }

        public override void OnExit() {
            
        }

        public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.Ground;
    }
}