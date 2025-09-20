using Unity.Mathematics.Geometry;
using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public class GroundMovementState : PlatformerPlayerMovementState {
        private PlayerGroundStateAnimationController groundStateAnimationController;
        private float desiredVelocity;
        [SerializeField] private float footStepFrequencyInSeconds;
        private float footStepTimer;
        private bool isStationary;

        public override void Initialize() {
            base.Initialize();
            groundStateAnimationController = GetComponent<PlayerGroundStateAnimationController>();
        }

        public override void OnUpdate() {
            inputMovementWrapper.jumpInputManager.CheckForJumpInput();
            if (PlatformerPlayerState.AllowedToDash() && inputMovementWrapper.WasDashPressedThisFrame()) {
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Dash);
            }

            if (!isStationary) {
                footStepTimer += Time.deltaTime;
            }
            if (footStepTimer > footStepFrequencyInSeconds) {
                footStepTimer = 0;
                PlayerAudioManager.PlayOneShotFootStep();
            }
        }

        public override void OnFixedUpdate(ref Vector2 currentVelocity) {
            if (AllowedToJump() &&
                inputMovementWrapper.jumpInputManager.ConsumeJumpInput()) {
                ApplyJump(ref currentVelocity, componentContainer.PlatformerPlayerState.CurrentMoveablePlatformPositionDiff / Time.fixedDeltaTime);
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Air);
                return;
            }

            // TODO: there should be a better solution than this
            if (!characterController.Grounded ||
                componentContainer.PlatformerPlayerForceFieldDetector.CurrentForceFieldForce.y > 0) {
                currentVelocity.y = 0;
                parentManager.UpdateState(PlatformerPlayerMovementStateId.Air);
                return;
            }

            float currentMovementInput = componentContainer.InputMovementWrapper.ReadHorizontalMovementInput();
            float targetSpeed = currentMovementInput * PlatformerPlayerPhysicsConfig.MaxMovementSpeed;
            float speedDif = targetSpeed - desiredVelocity;

            bool accelerating = (Mathf.Abs(targetSpeed) > 0.01f);
            float accelRate = accelerating
                ? PlatformerPlayerPhysicsConfig.GroundAcceleration
                : PlatformerPlayerPhysicsConfig.GroundDeceleration;
            accelRate *= PlatformerPlayerState.CurrentSurfaceAccelerationCoefficient;

            float movement = accelRate * speedDif * Time.fixedDeltaTime;

            desiredVelocity += movement * Time.fixedDeltaTime;
            desiredVelocity = Mathf.Clamp(desiredVelocity, -PlatformerPlayerPhysicsConfig.MaxMovementSpeed,
                PlatformerPlayerPhysicsConfig.MaxMovementSpeed);


            if (targetSpeed > 0) {
                setLookDirection(true);
            }

            if (targetSpeed < 0) {
                setLookDirection(false);
            }

            currentVelocity.x = desiredVelocity;
            float groundY = characterController.CurrentGroundHit.point.y;
            float currentY = characterController.MiddleOfBox.y;
            float yDelta = groundY - currentY;
            float yVelocity = yDelta / Time.fixedDeltaTime;
            currentVelocity.y = yVelocity;
            currentVelocity.x +=
                componentContainer.PlatformerPlayerState.CurrentMoveablePlatformPositionDiff.x /
                Time.fixedDeltaTime;
            float requestedVelocityBeforeForceField = desiredVelocity;
            isStationary = requestedVelocityBeforeForceField == 0 || currentMovementInput == 0 ||
                                characterController.RbVelocity.x == 0;
            //Debug.Log($"{requestedVelocityBeforeForceField.x == 0} | {currentMovementInput == 0} | {characterController.RbVelocity.x == 0}");
            groundStateAnimationController.ProcessAnimations(isStationary);
        }

        public override void OnEnter(PlatformerPlayerMovementStateId prevState) {
            PlatformerPlayerState.ResetNumJumps();
            PlatformerPlayerState.ResetNumDashes();
            characterController.setTargetRotation(0);
            groundStateAnimationController.OnGroundStateEnter(prevState);
            desiredVelocity = characterController.RbVelocity.x;
            footStepTimer = 0;
            PlatformerPlayerState.ResetLastTouchedWallSlideSide();
        }

        public override void OnExit(PlatformerPlayerMovementStateId newState) {
        }

        public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.Ground;
    }
}