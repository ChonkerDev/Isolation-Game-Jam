using System;
using Chonker.Scripts.Player;
using Chonker.Scripts.Player.States;
using UnityEngine;

public class PlatformerPlayerAnimationManager : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private float lookRotationSpeed = 1080;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _spriteAnchor;
    private PlatformerPlayerComponentContainer componentContainer;
    private string LastAnimatorState;

    private void Awake() {
        componentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
    }

    private void Update() {
        rotateSprite();
    }

    private void rotateSprite() {
        PlatformerPlayerMovementStateId stateId =
            componentContainer.PlatformerCharacterController.CurrentMovementStateId;
        float targetLookRotation = 0;
        switch (stateId) {
            case PlatformerPlayerMovementStateId.Air:
            case PlatformerPlayerMovementStateId.Ground:
                float lookAngleRotation = 0;
                break;
            case PlatformerPlayerMovementStateId.Dash:
                if (componentContainer.PlatformerCharacterController.RbVelocity.x > 0) {
                    targetLookRotation = -Vector2.SignedAngle(
                        componentContainer.PlatformerCharacterController.RbVelocity,
                        Vector2.right);
                }
                else {
                    targetLookRotation = -Vector2.SignedAngle(
                        componentContainer.PlatformerCharacterController.RbVelocity,
                        Vector2.left);
                }

                break;
        }

        float currentLookRotation = transform.localEulerAngles.z;
        Debug.Log(targetLookRotation);
        float lookRotation =
            Mathf.MoveTowardsAngle(currentLookRotation, targetLookRotation, lookRotationSpeed * Time.deltaTime);
        _spriteAnchor.localEulerAngles = new Vector3(0, 0, lookRotation);
        _spriteRenderer.flipX = !componentContainer.PlatformerPlayerState.facingRight;
    }

    private void CrossFadeAnimator(string stateName) {
        if (LastAnimatorState == stateName) {
            return;
        }

        LastAnimatorState = stateName;
        _animator.CrossFadeInFixedTime(stateName, 0);
    }

    public void CrossFadeToGround(bool idle) {
        if (idle) {
            CrossFadeAnimator("Base Layer.Ground.Idle");
        }
        else {
            CrossFadeAnimator("Base Layer.Ground.Move");
        }
    }

    public void CrossFadeToAir(AirStates airState) {
        string stateName = "Base Layer.Air." + airState.ToString();
        CrossFadeAnimator(stateName);
    }

    public void CrossFadeDash(DashState DashState) {
        string stateName = "Base Layer.Dash." + DashState.ToString();
        CrossFadeAnimator(stateName);
    }


    public enum AirStates {
        Fall,
        Mid,
        Rise
    }

    public enum DashState {
        Start,
        Loop,
        End
    }
}