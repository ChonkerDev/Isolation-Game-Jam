using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public class AirMovementState : PlatformerPlayerMovementState {
        private float coyoteTimeTimer;
        private bool coyoteTimeActive;

        public override void Initialize() {
            base.Initialize();
        }

        public override void OnUpdate() {
            inputMovementWrapper.jumpInputManager.CheckForJumpInput();
        }

        public override void OnFixedUpdate(ref Vector2 currentVelocity) {
            coyoteTimeTimer += Time.fixedDeltaTime;
            currentVelocity.y -= characterController.CurrentGravity * Time.fixedDeltaTime;
            Debug.Log(PlatformerPlayerState.NumJumpsAvailable);
            if (PlatformerPlayerState.NumJumpsAvailable > 0 && inputMovementWrapper.jumpInputManager.ConsumeJumpInput()) {
                bool coyoteTimeValid = coyoteTimeTimer < PlatformerPlayerPhysicsConfig.CoyoteTime;
                if (coyoteTimeValid) {
                    ApplyJump(ref currentVelocity, true);
                }
                else {
                    ApplyJump(ref currentVelocity);
                    PlatformerPlayerState.DecrementNumJumps();
                }

            }

            float currentMovementInput = componentContainer.InputMovementWrapper.ReadHorizontalMovementInput();
            float targetSpeed = currentMovementInput * PlatformerPlayerPhysicsConfig.MaxMovementSpeed;
            float speedDif = targetSpeed - currentVelocity.x;

            float accelRate = PlatformerPlayerPhysicsConfig.AirInputAcceleration;

            float movement = accelRate * speedDif * Time.fixedDeltaTime;
            if (!characterController.ProbeForWall(new Vector2(currentMovementInput, 0), Mathf.Abs(movement) * Time.fixedDeltaTime)) {
                currentVelocity.x += movement * Time.fixedDeltaTime;
            }
            if (characterController.Grounded) {
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Ground);
            }
        }

        public override void OnEnter() {
            coyoteTimeTimer = 0;
            inputMovementWrapper.jumpInputManager.ClearJumpInput();
        }

        public override void OnExit() {
        }

        public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.Air;
    }
}