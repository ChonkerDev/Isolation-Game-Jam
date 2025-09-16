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

        protected PlatformerPlayerAnimationManager PlatformerPlayerAnimationManager => componentContainer.platformerPlayerAnimationManager;

        protected RaycastHit2D CurrentGroundHit => characterController.CurrentGroundHit;

        protected PlatformerPlayerPhysicsConfig PlatformerPlayerPhysicsConfig =>
            componentContainer.PlatformerPlayerPhysicsConfig;

        protected PlatformerPlayerState PlatformerPlayerState => componentContainer.PlatformerPlayerState;


        public override void Initialize() {
            parentManager = GetComponentInParent<PlatformerPlayerMovementStateManager>();
            componentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
        }

        protected void ApplyJump(ref Vector2 velocity, bool overrideVerticalVelocity = false,
            bool overrideHorizontalVelocity = false) {
            characterController.ApplyHighJumpGravityForDuration();
            if (overrideVerticalVelocity && velocity.y > 0) {
                velocity.y = PlatformerPlayerPhysicsConfig.JumpPower;
            }
            else {
                velocity.y = +PlatformerPlayerPhysicsConfig.JumpPower;
            }

            if (overrideHorizontalVelocity) {
                velocity.x = 0;
            }
        }



        public abstract void OnUpdate();
        public abstract void OnFixedUpdate(ref Vector2 currentVelocity);
    }
}