using System;
using Chonker.Scripts.Player;
using Chonker.Scripts.Player.States;
using UnityEngine;

public class PlatformerPlayerAnimationManager : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _spriteAnchor;
    private PlatformerPlayerComponentContainer componentContainer;
    private string LastAnimatorState;
    private float targetRotation;
    private float rotationSpeed = 100000000;

    [SerializeField] private bool _initialFacingRight = true;

    private PlatformerPlayerAnimationConfig platformerPlayerAnimationConfig =>
        componentContainer.PlatformerPlayerAnimationConfig;

    private FacingDirection _facingDirection;

    public FacingDirection FacingDirection {
        get => _facingDirection;
        set {
            _facingDirection = value;
            _spriteRenderer.flipX = _facingDirection == FacingDirection.Left;
        }
    }

    public void FlipFacingDirection() {
        if (FacingDirection == FacingDirection.Right) {
            FacingDirection =  FacingDirection.Left;
        } else if (FacingDirection == FacingDirection.Left) {
            FacingDirection =  FacingDirection.Right;

        }
    }

    private void Awake() {
        componentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
    }

    private void Start() {
        FacingDirection = FacingDirection.Right;
    }

    private void Update() {
        rotateSprite();
    }

    public void setTargetRotation(float rotation, int rotationSpeed = 100000000) {
        targetRotation = rotation;
        this.rotationSpeed = rotationSpeed;
    }

    private void rotateSprite() {
        float currentLookRotation = transform.localEulerAngles.z;
        float lookRotation =
            Mathf.MoveTowardsAngle(currentLookRotation, targetRotation, rotationSpeed * Time.deltaTime);
        _spriteAnchor.localEulerAngles = new Vector3(0, 0, lookRotation);
    }
    
    public void SetSpriteAnchorScale(float xScale, float yScale) {
        _spriteAnchor.localScale = new Vector3(xScale, yScale, 1f);
    }

    private void CrossFadeAnimator(string stateName) {
        if (LastAnimatorState == stateName) {
            return;
        }

        LastAnimatorState = stateName;
        _animator.CrossFadeInFixedTime(stateName, 0);
    }

    public void CrossFadeToGround(GroundStates GroundState) {
        GroundStates groundStateReplaced = GroundState;
        switch (GroundState) {
            case GroundStates.MoveStart:
                if (platformerPlayerAnimationConfig.SkipMovingStartAnimation) {
                    groundStateReplaced = GroundStates.MoveLoop;
                }

                break;
            case GroundStates.MoveStop:
                if (platformerPlayerAnimationConfig.SkipMovingStopAnimation) {
                    groundStateReplaced = GroundStates.Idle;
                }

                break;
            case GroundStates.Land:
                if (platformerPlayerAnimationConfig.SkipLandingAnimation) {
                    groundStateReplaced = GroundStates.Idle;
                }

                break;
        }

        CrossFadeAnimator("Base Layer.Ground." + groundStateReplaced);
    }

    public void CrossFadeToAir(AirStates airState) {
        string stateName = "Base Layer.Air." + airState;
        CrossFadeAnimator(stateName);
    }

    public void CrossFadeToWallSlide() {
        string stateName = "Base Layer.Wall Slide.Wall Slide";
        CrossFadeAnimator(stateName);
    }

    public void CrossFadeDash(DashState DashState) {
        DashState dashStateReplaced = DashState;
        switch (DashState) {
            case DashState.End:
                if (platformerPlayerAnimationConfig.SkipDashStopAnimation) {
                    return;
                }

                break;
        }

        string stateName = "Base Layer.Dash." + dashStateReplaced;
        CrossFadeAnimator(stateName);
    }

    public void CrossFadeToDead() {
        string stateName = "Base Layer.Dead.Dead";
        CrossFadeAnimator(stateName);
    }

    public bool isCurrentState(GroundStates GroundState) {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Ground." + GroundState);
    }


    public enum GroundStates {
        Idle,
        MoveLoop,
        MoveStart,
        MoveStop,
        Land
    }

    public enum AirStates {
        Fall,
        Mid,
        Rise
    }

    public enum DashState {
        Start,
        HorizontalLoop,
        VerticalLoop,
        End
    }
}