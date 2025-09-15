using Chonker.Runtime.Core.StateMachine;
using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public abstract class PlatformerPlayerMovementState : StateMachine<PlatformerPlayerMovementStateId> {
        protected PlatformerPlayerMovementStateManager parentManager;
        protected PlatformerPlayerComponentContainer componentContainer;
        protected PlatformerCharacterController characterController => componentContainer.PlatformerCharacterController;
        protected InputMovementWrapper inputMovementWrapper => componentContainer.InputMovementWrapper;

        protected RaycastHit2D CurrentGroundHit => characterController.CurrentGroundHit;
        protected PlatformerPlayerPhysicsConfig PlatformerPlayerPhysicsConfig => componentContainer.PlatformerPlayerPhysicsConfig;
        protected PlatformerPlayerState PlatformerPlayerState => componentContainer.PlatformerPlayerState;

        public override void Initialize() {
            parentManager = GetComponentInParent<PlatformerPlayerMovementStateManager>();
            componentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
        }

        public abstract void OnUpdate();
        public abstract void OnFixedUpdate(ref Vector2 currentVelocity);
    }
}