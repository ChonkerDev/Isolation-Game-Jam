using System.Collections;
using Chonker.Runtime.Core.StateMachine;
using Chonker.Scripts.Game_Management;
using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public class DeadMovementState : PlatformerPlayerMovementState {
        public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.Dead;

        public override void OnEnter(PlatformerPlayerMovementStateId prevState) {
            PlatformerPlayerAnimationManager.CrossFadeToDead();
            PlayerAudioManager.PlayDeath();
            StartCoroutine(DelayReset());
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

        private IEnumerator DelayReset() {
            yield return new WaitForSeconds(0.5f);
            LevelManager.instance.ResetLevel();
        }
    }
}