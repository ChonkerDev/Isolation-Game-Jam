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
            PlatformerPlayerAnimationManager.FacingRight = facingRight;
        }

        protected bool CheckForWallSlide() {
            if (!PlatformerPlayerState.WallSlideAbilityUnlocked()) return false;
            bool doesVelocityMatchFacingDirection =
                PlatformerPlayerAnimationManager.FacingRight == characterController.RbVelocity.x > 0;
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
        
        protected RaycastHit2D ProbeForWall(float additionalDistance = 0) {
            Vector2 direction = PlatformerPlayerAnimationManager.FacingRight ? Vector2.right : Vector2.left;
            float distance = characterController.RbVelocity.x * Time.fixedDeltaTime + characterController.BoxSize.x + additionalDistance;
            Debug.DrawRay(characterController.transform.position, direction * distance, Color.red);
            return Physics2D.Raycast(characterController.transform.position,
                direction, distance,ObstacleLayerMask
            );
        }
        

        public abstract void OnUpdate();
        public abstract void OnFixedUpdate(ref Vector2 currentVelocity);
    }
}