using Chonker.Scripts.Player;
using Chonker.Scripts.Player.States;
using UnityEngine;

public class WallSlideMovementState : PlatformerPlayerMovementState {
    public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.WallSlide;
    private int numTimesTouchedSameSide;
    public override void OnEnter(PlatformerPlayerMovementStateId prevState) {
        PlatformerPlayerState.SetLastWallSlideSide(PlatformerPlayerAnimationManager.FacingDirection);
        RaycastHit2D hit = ProbeForWall(1);
        if (!hit.transform) {
            Debug.LogError("If entering the wall slide state, there should Always be a wall in the facing direction");
        }
        float offset = -characterController.BoxSize.x * (int) PlatformerPlayerAnimationManager.FacingDirection;
        float targetX = hit.point.x + offset / 2;
        characterController.TeleportX(targetX);
        PlatformerPlayerAnimationManager.FlipFacingDirection();
        PlatformerPlayerAnimationManager.CrossFadeToWallSlide();
        
        PlatformerPlayerState.ResetNumDashes();
    }


    public override void OnExit(PlatformerPlayerMovementStateId newState) {
        inputMovementWrapper.jumpInputManager.ClearJumpInput();
        PlatformerPlayerAnimationManager.SetSpriteAnchorScale(1, 1);

    }

    public override void OnUpdate() {
        inputMovementWrapper.jumpInputManager.CheckForJumpInput();
    }

    public override void OnFixedUpdate(ref Vector2 currentVelocity) {
        float gravityRate = -PlatformerPlayerPhysicsConfig.WallSlideGravityRate;
        float horizontalInput = inputMovementWrapper.ReadHorizontalMovementInput();
        bool wallGripRequested = horizontalInput < 0 && PlatformerPlayerAnimationManager.FacingDirection == FacingDirection.Right ||
                                 horizontalInput > 0 && PlatformerPlayerAnimationManager.FacingDirection ==  FacingDirection.Left;
        if (wallGripRequested && PlatformerPlayerState.WallGripAbilityUnlocked()) {
            gravityRate *= PlatformerPlayerPhysicsConfig.WallGripGravityMultiplier;
        }

        currentVelocity.y = gravityRate * Time.fixedDeltaTime;
        currentVelocity.x = 0;
        if (characterController.probeGround(-currentVelocity.y * Time.fixedDeltaTime, .95f).transform) {
            parentManager.UpdateState(PlatformerPlayerMovementStateId.Ground);
            return;
        }
        RaycastHit2D hit = ProbeForWall(1, true);

        if (!hit.transform) {
            parentManager.UpdateState(PlatformerPlayerMovementStateId.Air);
            return;
        }

        if (inputMovementWrapper.jumpInputManager.ConsumeJumpInput()) {
            currentVelocity.y = PlatformerPlayerPhysicsConfig.WallSlideVerticalJumpPower;
            currentVelocity.x = PlatformerPlayerPhysicsConfig.WallSlideHorizontalJumpPower;
            currentVelocity.x *= (int) PlatformerPlayerAnimationManager.FacingDirection ;
            parentManager.UpdateState(PlatformerPlayerMovementStateId.Air);
        }
    }
}