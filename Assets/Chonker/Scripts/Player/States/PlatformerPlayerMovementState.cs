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

        protected PlatformerPlayerState PlatformerPlayerState => componentContainer.PlatformerPlayerState;

        protected LayerMask ObstacleLayerMask;

        public override void Initialize() {
            parentManager = GetComponentInParent<PlatformerPlayerMovementStateManager>();
            componentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
            ObstacleLayerMask = LayerMask.GetMask("Obstacle");
        }

        protected void ApplyJump(ref Vector2 velocity, bool overrideVerticalVelocity = false,
            bool overrideHorizontalVelocity = false) {
            characterController.ApplyHighJumpGravityForDuration();
            if (overrideVerticalVelocity && velocity.y > 0) {
                velocity.y = PlatformerPlayerPhysicsConfig.JumpPower;
            }
            else {
                velocity.y += PlatformerPlayerPhysicsConfig.JumpPower;
            }

            if (overrideHorizontalVelocity) {
                velocity.x = 0;
            }
        }

        protected void setLookDirection(bool facingRight) {
            PlatformerPlayerAnimationManager.FacingDirection = facingRight ? FacingDirection.Right : FacingDirection.Left;
        }

        protected bool CheckForWallSlide() {
            if (!PlatformerPlayerState.WallSlideAbilityUnlocked()) return false;
            if(!componentContainer.PlatformerPlayerForceFieldDetector.IsForceFieldPresent()) return false;
            bool doesVelocityMatchFacingDirection =
                (Mathf.Sign((int) PlatformerPlayerAnimationManager.FacingDirection) == Mathf.Sign(characterController.RbVelocity.x));
            bool isDistanceFromGroundValid =
                !characterController.probeGround(PlatformerPlayerPhysicsConfig.MaxDistanceFromGroundToPreventWallSlide)
                    .transform;
            if (!doesVelocityMatchFacingDirection ||
                inputMovementWrapper.ReadHorizontalMovementInput() == 0 || !isDistanceFromGroundValid) return false;
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
            float distance = Mathf.Abs(characterController.RbVelocity.x) * Time.fixedDeltaTime + characterController.BoxSize.x + additionalDistance;
            Debug.DrawRay(characterController.transform.position, direction * distance, Color.red);
            return Physics2D.Raycast(characterController.transform.position,
                direction, distance,ObstacleLayerMask
            );
        }

        protected bool AllowedToJump() {
            return !componentContainer.PlatformerPlayerForceFieldDetector.IsForceFieldPresent();
        }
        
        protected bool AllowedToDash() {
            return PlatformerPlayerState.AllowedToDash();
        }
        

        public abstract void OnUpdate();
        public abstract void OnFixedUpdate(ref Vector2 currentVelocity);
    }
}