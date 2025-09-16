using System.Collections;
using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public class DashMovementState : PlatformerPlayerMovementState {
        private Vector2 direction;
        private Vector2 currentVelocity;

        public override void OnEnter(PlatformerPlayerMovementStateId previousState) {
            currentVelocity = Vector2.zero;
            PlatformerPlayerState.DecrementNumberOfDashes();
            StartCoroutine(DelaySetDirection());
            StartCoroutine(ProcessAcceleration());
        }

        public override void OnExit(PlatformerPlayerMovementStateId newState) {
            StopAllCoroutines();
        }

        public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.Dash;

        public override void OnUpdate() {
        }

        public override void OnFixedUpdate(ref Vector2 currentVelocity) {
            Vector2 targetVelocity = this.currentVelocity;
            RaycastHit2D wallhit = characterController.ProbeForWallHit(new Vector2(targetVelocity.x, 0),
                targetVelocity.magnitude * Time.fixedDeltaTime);
            if (wallhit.transform) {
                currentVelocity = Vector3.ProjectOnPlane(targetVelocity, wallhit.normal);
            }
            else {
                currentVelocity = targetVelocity;
            }

            float changeViewDirectionThreshold = 0;
            if (currentVelocity.x > changeViewDirectionThreshold) {
                PlatformerPlayerState.facingRight = true;
            }
            else if (currentVelocity.x < -changeViewDirectionThreshold) {
                PlatformerPlayerState.facingRight = false;
            }
        }

        private IEnumerator DelaySetDirection() {
            float timer = PlatformerPlayerPhysicsConfig.DirectionInputBufferInSeconds;
            while (timer > 0) {
                timer -= Time.deltaTime;
                direction = new Vector2(inputMovementWrapper.ReadHorizontalMovementInput(),
                    inputMovementWrapper.ReadVerticalMovementInput());
                if (direction.magnitude > .1f) {
                    direction = direction.normalized;
                    PlatformerPlayerState.facingRight = direction.x > 0;
                    break;
                }

                yield return null;
            }

            if (direction.magnitude < .1f) {
                direction = PlatformerPlayerState.facingRight ? Vector2.right : Vector2.left;
            }
            else if (!PlatformerPlayerState.AllowedToOmniDirectionalDash()) {
                direction = PlatformerPlayerState.facingRight ? Vector2.right : Vector2.left;
            }
        }

        private IEnumerator ProcessAcceleration() {
            float accelerationTime = PlatformerPlayerPhysicsConfig.DashAccelerationTime;
            float dashSpeed = PlatformerPlayerPhysicsConfig.DashTopSpeed;
            float accTimer = 0;
            PlatformerPlayerAnimationManager.CrossFadeDash(PlatformerPlayerAnimationManager.DashState.Start);
            while (accTimer < 1) {
                accTimer += Time.deltaTime / accelerationTime;
                currentVelocity = Vector2.Lerp(Vector2.zero, direction * dashSpeed, accTimer);
                yield return new WaitForFixedUpdate();
            }

            currentVelocity = direction * dashSpeed;
            PlatformerPlayerAnimationManager.CrossFadeDash(PlatformerPlayerAnimationManager.DashState.Loop);
            yield return new WaitForSeconds(PlatformerPlayerPhysicsConfig.DashConstantSpeedTime);
            float decelerationTime = PlatformerPlayerPhysicsConfig.DashDecelerationTime;
            float decelerationTimer = 0;
            PlatformerPlayerAnimationManager.CrossFadeDash(PlatformerPlayerAnimationManager.DashState.End);
            while (decelerationTimer < 1 && decelerationTime > 0) {
                decelerationTimer += Time.deltaTime / decelerationTime;
                currentVelocity = Vector2.Lerp(direction * dashSpeed, Vector2.zero, decelerationTimer);
                yield return new WaitForFixedUpdate();
            }

            parentManager.UpdateState(PlatformerPlayerMovementStateId.Air);
        }
    }
}