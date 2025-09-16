using Chonker.Runtime.Core.StateMachine;
using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public class DeadMovementState : PlatformerPlayerMovementState {
        public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.Dead;

        public override void OnEnter(PlatformerPlayerMovementStateId prevState) {
            PlatformerPlayerAnimationManager.CrossFadeToDead();
        }

        public override void OnExit(PlatformerPlayerMovementStateId newState) {
        }

        public override void OnUpdate() {
        }

        public override void OnFixedUpdate(ref Vector2 currentVelocity) {
            currentVelocity.y -= PlatformerPlayerPhysicsConfig.GravityRate * Time.fixedDeltaTime;

            float targetSpeed = 0;
            float speedDif = targetSpeed - currentVelocity.x;

            float accelRate = PlatformerPlayerPhysicsConfig.GroundDeceleration;

            float movement = accelRate * speedDif * Time.fixedDeltaTime;

            currentVelocity.x += movement * Time.fixedDeltaTime;
        }
    }
}