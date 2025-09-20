using System.Collections;
using UnityEngine;

namespace Chonker.Scripts.Player.States {
    public class DashMovementState : PlatformerPlayerMovementState {
        private Vector2 direction;
        private Vector2 currentVelocity;
        private LayerMask breakableWallLayerMask;
        private bool preparingVelocity;

        public override void OnEnter(PlatformerPlayerMovementStateId previousState) {
            currentVelocity = Vector2.zero;
            PlatformerPlayerState.DecrementNumberOfDashes();
            StartCoroutine(DelaySetDirection());
            breakableWallLayerMask = LayerMask.GetMask("Breakable Wall");
            componentContainer.PlayerAudioManager.PlayDash();
        }

        public override void OnExit(PlatformerPlayerMovementStateId newState) {
            StopAllCoroutines();
        }

        public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.Dash;

        public override void OnUpdate() {
        }

        public override void OnFixedUpdate(ref Vector2 currentVelocity) {
            if (preparingVelocity) {
                currentVelocity = Vector2.zero;
                return;
            }
            Vector2 targetVelocity = this.currentVelocity;
            RaycastHit2D wallhit = characterController.ProbeForWallHit(targetVelocity.normalized,
                targetVelocity.magnitude * Time.fixedDeltaTime);
            if (wallhit.transform) {
                currentVelocity = Vector3.ProjectOnPlane(targetVelocity, wallhit.normal);
            }
            else {
                currentVelocity = targetVelocity;
            }
            
            CheckForWallBreak();

            float changeViewDirectionThreshold = 0;
            if (currentVelocity.x > changeViewDirectionThreshold) {
                setLookDirection(true);
            }
            else if (currentVelocity.x < -changeViewDirectionThreshold) {
                setLookDirection(false);
            }
        }

        private IEnumerator DelaySetDirection() {
            preparingVelocity = true;
            float timer = PlatformerPlayerPhysicsConfig.DirectionInputBufferInSeconds;
            while (timer > 0) {
                timer -= Time.deltaTime;
                characterController.setTargetRotation(0);
                if (Mathf.Abs(characterController.RbVelocity.sqrMagnitude) > .1f) {
                    if (characterController.RbVelocity.x > 0) {
                        setLookDirection(true);
                    }
                    else if (characterController.RbVelocity.x < 0) {
                        setLookDirection(false);
                    }
                }

                direction = new Vector2(inputMovementWrapper.ReadHorizontalMovementInput(),
                    inputMovementWrapper.ReadVerticalMovementInput());

                yield return null;
            }

            preparingVelocity = false;
            if (direction.magnitude < .1f) {
                direction = Vector2.right * (int)PlatformerPlayerAnimationManager.FacingDirection;
            }
            else if (!PlatformerPlayerState.AllowedToOmniDirectionalDash()) {
                direction = Vector2.right * (int)PlatformerPlayerAnimationManager.FacingDirection;
            }

            direction.Normalize();
            float lookAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            characterController.setTargetRotation(lookAngle);
            StartCoroutine(ProcessAcceleration());
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
            float xAt45Degrees = .7f;
            if (Mathf.Abs(currentVelocity.normalized.x) < xAt45Degrees) {
                PlatformerPlayerAnimationManager.CrossFadeDash(PlatformerPlayerAnimationManager.DashState.HorizontalLoop);
            }
            else {
                PlatformerPlayerAnimationManager.CrossFadeDash(PlatformerPlayerAnimationManager.DashState.VerticalLoop);
            }
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

        private void CheckForWallBreak() {
            RaycastHit2D raycastHit2D = characterController.ProbeForBreakableWall(currentVelocity.normalized, currentVelocity.magnitude * Time.fixedDeltaTime);
            if (raycastHit2D.transform) {
                if (raycastHit2D.collider.gameObject.TryGetComponent(out BreakableWall bw)) {
                    bw.BreakWall();
                }
            }
        }
        
    }
}