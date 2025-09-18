using System;
using System.Collections;
using Chonker.Runtime.Core.StateMachine;
using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public abstract class PlatformerPlayerMovementState : StateMachine<PlatformerPlayerMovementStateId> {
        protected PlatformerPlayerMovementStateManager parentManager;
        protected PlatformerPlayerComponentContainer componentContainer;
        protected PlatformerCharacterController characterController => componentContainer.PlatformerCharacterController;
        protected InputMovementWrapper inputMovementWrapper => componentContainer.InputMovementWrapper;

        protected PlatformerPlayerAnimationManager PlatformerPlayerAnimationManager =>
            componentContainer.platformerPlayerAnimationManager;

        protected RaycastHit2D CurrentGroundHit => characterController.CurrentGroundHit;

        protected PlatformerPlayerPhysicsConfigSO PlatformerPlayerPhysicsConfig =>
            componentContainer.PhysicsConfigSO;

        protected PlatformerPlayerAnimationConfig PlatformerPlayerAnimationConfig =>
            componentContainer.PlatformerPlayerAnimationConfig;

        protected PlayerAudioManager PlayerAudioManager => componentContainer.PlayerAudioManager;

        protected PlatformerPlayerState PlatformerPlayerState => componentContainer.PlatformerPlayerState;

        protected LayerMask ObstacleLayerMask;

        public override void Initialize() {
            parentManager = GetComponentInParent<PlatformerPlayerMovementStateManager>();
            componentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
            ObstacleLayerMask = LayerMask.GetMask("Obstacle");
        }

        protected void ApplyJump(ref Vector2 velocity, Vector2 additiveVelocity = default) {
            characterController.ApplyHighJumpGravityForDuration();
            velocity.y = PlatformerPlayerPhysicsConfig.JumpPower;
            velocity += additiveVelocity;
        }

        protected void setLookDirection(bool facingRight) {
            PlatformerPlayerAnimationManager.FacingDirection =
                facingRight ? FacingDirection.Right : FacingDirection.Left;
        }

        protected bool CheckForWallSlide() {
            if (!AllowedToWallSlide()) return false;
            if (componentContainer.PlatformerPlayerForceFieldDetector.IsForceFieldPresent()) return false;
            float horizontalMovementInput = inputMovementWrapper.ReadHorizontalMovementInput();
            bool doesInputMatchFacingDirection =
                Mathf.Sign((int)PlatformerPlayerAnimationManager.FacingDirection) ==
                 Mathf.Sign(horizontalMovementInput) && horizontalMovementInput != 0;
            bool isDistanceFromGroundValid =
                !characterController.probeGround(PlatformerPlayerPhysicsConfig.MaxDistanceFromGroundToPreventWallSlide)
                    .transform;
            Debug.Log(isDistanceFromGroundValid);
            if (!doesInputMatchFacingDirection || !isDistanceFromGroundValid) return false;
            RaycastHit2D hit = ProbeForWall();
            if (hit.transform) {
                parentManager.UpdateState(PlatformerPlayerMovementStateId.WallSlide);
                return true;
            }

            return false;
        }

        protected RaycastHit2D ProbeForWall(float additionalDistance = 0, bool backwards = false) {
            Vector2 direction = Vector2.right;
            if (PlatformerPlayerAnimationManager.FacingDirection == FacingDirection.Left) {
                direction = Vector2.left;
            }

            if (backwards) {
                direction *= -1;
            }

            float distance = Mathf.Abs(characterController.RbVelocity.x) * Time.fixedDeltaTime +
                             characterController.BoxSize.x + additionalDistance;
            Debug.DrawRay(characterController.transform.position, direction * distance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(characterController.transform.position,
                direction, distance, ObstacleLayerMask
            );
            return hit;
        }

        protected bool AllowedToJump() {
            return !componentContainer.PlatformerPlayerForceFieldDetector.IsForceFieldPresent();
        }

        protected bool AllowedToDash() {
            return PlatformerPlayerState.AllowedToDash();
        }

        protected bool AllowedToWallSlide() {
           return PlatformerPlayerState.AllowedToWallSlide() && PlatformerPlayerState.LastFacingDirectionAfterWallSlide !=  PlatformerPlayerAnimationManager.FacingDirection; 
        }


        public abstract void OnUpdate();
        public abstract void OnFixedUpdate(ref Vector2 currentVelocity);
    }
}