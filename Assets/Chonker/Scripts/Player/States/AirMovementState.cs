using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public class AirMovementState : PlatformerPlayerMovementState {
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
            currentVelocity.y -= PlatformerPlayerPhysicsConfig.GravityRate * Time.fixedDeltaTime;
            if (jumpRequested) {
                currentVelocity.y += PlatformerPlayerPhysicsConfig.JumpForce;
                jumpRequested = false;
                PlatformerPlayerState.ResetNumJumps();
            }

            if (characterController.Grounded && currentVelocity.y <= 0) {
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Ground);
            }

        }

        public override void OnEnter() {
            
        }

        public override void OnExit() {
            jumpRequested = false;
        }

        public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.Air;
    }
}