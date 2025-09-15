using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public class AirMovementState : PlatformerPlayerMovementState {
        private bool jumpRequested;
        private float coyoteTimeTimer;
        private bool coyoteTimeActive;

        public override void Initialize() {
            base.Initialize();
        }

        public override void OnUpdate() {
            if (inputMovementWrapper.WasJumpPressedThisFrame()) {
                jumpRequested = true;
            }
        }

        public override void OnFixedUpdate(ref Vector2 currentVelocity) {
            coyoteTimeTimer += Time.fixedDeltaTime;
            currentVelocity.y -= PlatformerPlayerPhysicsConfig.GravityRate * Time.fixedDeltaTime;
            Vector2 position = transform.position;
            Debug.DrawRay( position + currentVelocity * Time.fixedDeltaTime, Vector2.down * PlatformerPlayerPhysicsConfig
                .DistanceToGroundToTreatAirJumpAsGrounded, Color.brown);
            if (jumpRequested) {
                jumpRequested = false;
                RaycastHit2D groundProbe = characterController.probeGround(PlatformerPlayerPhysicsConfig
                    .DistanceToGroundToTreatAirJumpAsGrounded);
                if (currentVelocity.y < 0 && groundProbe.transform) {
                    PlatformerPlayerState.ResetNumJumps();
                    //characterController.transform.position = groundProbe.point;
                    currentVelocity.y = PlatformerPlayerPhysicsConfig.JumpForce;
                }
                else if (coyoteTimeTimer < PlatformerPlayerPhysicsConfig.CoyoteTime) {
                    PlatformerPlayerState.ResetNumJumps();
                    currentVelocity.y = PlatformerPlayerPhysicsConfig.JumpForce;
                }
                else if (PlatformerPlayerState.NumJumpsAvailable > 0) {
                    currentVelocity.y = PlatformerPlayerPhysicsConfig.JumpForce;
                }
                PlatformerPlayerState.DecrementNumJumps();
            }

            if (characterController.Grounded && currentVelocity.y <= 0) {
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Ground);
            }
        }

        public override void OnEnter() {
            coyoteTimeTimer = 0;
            jumpRequested = false;
        }

        public override void OnExit() {
            
        }

        public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.Air;
    }
}