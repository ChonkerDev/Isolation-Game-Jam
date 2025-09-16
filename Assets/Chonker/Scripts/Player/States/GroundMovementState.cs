using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public class GroundMovementState : PlatformerPlayerMovementState {
        private PlayerGroundStateAnimationController groundStateAnimationController;

        public override void Initialize() {
            base.Initialize();
            groundStateAnimationController = GetComponent<PlayerGroundStateAnimationController>();
        }

        public override void OnUpdate() {
            inputMovementWrapper.jumpInputManager.CheckForJumpInput();
            if (PlatformerPlayerState.AllowedToDash() && inputMovementWrapper.WasDashPressedThisFrame()) {
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Dash);
            }
        }

        public override void OnFixedUpdate(ref Vector2 currentVelocity) {
            if (inputMovementWrapper.jumpInputManager.ConsumeJumpInput() &&
                PlatformerPlayerState.NumJumpsAvailable > 0) {
                ApplyJump(ref currentVelocity);
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Air);
                return;
            }

            float currentMovementInput = componentContainer.InputMovementWrapper.ReadHorizontalMovementInput();
            float targetSpeed = currentMovementInput * PlatformerPlayerPhysicsConfig.MaxMovementSpeed;
            float speedDif = targetSpeed - currentVelocity.x;

            bool accelerating = (Mathf.Abs(targetSpeed) > 0.01f);
            float accelRate = accelerating
                ? PlatformerPlayerPhysicsConfig.GroundAcceleration
                : PlatformerPlayerPhysicsConfig.GroundDeceleration;

            float movement = accelRate * speedDif * Time.fixedDeltaTime;

            currentVelocity.x += movement * Time.fixedDeltaTime;
            currentVelocity = Vector2.ClampMagnitude(currentVelocity, PlatformerPlayerPhysicsConfig.MaxMovementSpeed);
            if (!characterController.Grounded) {
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Air);
            }

            if (targetSpeed > 0) {
                setLookDirection(true);
            }

            if (targetSpeed < 0) {
                setLookDirection(false);
            }

            Vector2 requestedVelocityBeforeForceField = currentVelocity;
            currentVelocity += componentContainer.PlatformerPlayerForceFieldDetector.CurrentForceFieldForce;
            bool isStationary = requestedVelocityBeforeForceField.x == 0 || currentMovementInput == 0 ||
                                characterController.RbVelocity.x == 0;
            //Debug.Log($"{requestedVelocityBeforeForceField.x == 0} | {currentMovementInput == 0} | {characterController.RbVelocity.x == 0}");
            groundStateAnimationController.ProcessAnimations(isStationary);
        }

        public override void OnEnter(PlatformerPlayerMovementStateId prevState) {
            PlatformerPlayerState.ResetNumJumps();
            PlatformerPlayerState.ResetNumDashes();
            PlatformerPlayerAnimationManager.setTargetRotation(0);
            groundStateAnimationController.OnGroundStateEnter(prevState);
        }

        public override void OnExit(PlatformerPlayerMovementStateId newState) {
        }

        public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.Ground;
    }
}