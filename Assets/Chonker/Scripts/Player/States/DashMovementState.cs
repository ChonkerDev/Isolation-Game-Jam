using System.Collections;
using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public class DashMovementState : PlatformerPlayerMovementState {
        private Vector2 direction;
        private int dashStage = 0;
        private Vector2 currentVelocity;

        public override void OnEnter(PlatformerPlayerMovementStateId previousState) {
            direction = new Vector2(inputMovementWrapper.ReadHorizontalMovementInput(),
                inputMovementWrapper.ReadVerticalMovementInput());
            if (direction.magnitude < .1f) {
                direction = PlatformerPlayerState.facingRight ? Vector2.right : Vector2.left;
            }

            dashStage = 0;
            StartCoroutine(ProcessAcceleration());
        }

        public override void OnExit(PlatformerPlayerMovementStateId  newState) {
            StopAllCoroutines();
        }

        public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.Dash;

        public override void OnUpdate() {
        }

        public override void OnFixedUpdate(ref Vector2 currentVelocity) {
            Vector2 targetVelocity = this.currentVelocity;
            RaycastHit2D wallhit = characterController.ProbeForWallHit(new Vector2(targetVelocity.x, 0),
                targetVelocity.magnitude * Time.fixedDeltaTime);
            currentVelocity = Vector3.ProjectOnPlane(targetVelocity, wallhit.normal);
        }

        private IEnumerator ProcessAcceleration() {
            float accelerationTime = PlatformerPlayerPhysicsConfig.DashAccelerationTime;
            float dashSpeed = PlatformerPlayerPhysicsConfig.DashTopSpeed;
            float accTimer = 0;
            while (accTimer < 1) {
                accTimer += Time.deltaTime / accelerationTime;
                currentVelocity = Vector2.Lerp(Vector2.zero, direction * dashSpeed, accTimer);
                yield return new WaitForFixedUpdate();
            }
            currentVelocity = direction * dashSpeed;
            yield return new WaitForSeconds(PlatformerPlayerPhysicsConfig.DashConstantSpeedTime);
            parentManager.UpdateState(PlatformerPlayerMovementStateId.Air);
            /*float decelerationTime = PlatformerPlayerPhysicsConfig.DashDecelerationTime;
            float decelerationTimer = 0;
            while (decelerationTimer < 1) {
                decelerationTimer += Time.deltaTime/ decelerationTime;

                yield return new WaitForFixedUpdate();
            }*/
        }
    }
}