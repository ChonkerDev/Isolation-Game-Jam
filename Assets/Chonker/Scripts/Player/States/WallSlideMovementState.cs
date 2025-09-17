using Chonker.Scripts.Player.States;
using UnityEngine;

public class WallSlideMovementState : PlatformerPlayerMovementState {
    public override PlatformerPlayerMovementStateId StateId => PlatformerPlayerMovementStateId.WallSlide;

    public override void OnEnter(PlatformerPlayerMovementStateId prevState) {
        RaycastHit2D hit = ProbeForWall(1);
        if (!hit.transform) {
            Debug.LogError("If entering the wall slide state, there should Always be a wall in the facing direction");
        }

        float offset = PlatformerPlayerAnimationManager.FacingRight
            ? -characterController.BoxSize.x
            : characterController.BoxSize.x;
        float targetX = hit.point.x + offset / 2;
        characterController.TeleportX(targetX);
        PlatformerPlayerAnimationManager.FacingRight = !PlatformerPlayerAnimationManager.FacingRight;
        PlatformerPlayerAnimationManager.CrossFadeToWallSlide();
        //TODO; remove below once update sprites
        float xScale;
        if (PlatformerPlayerAnimationManager.FacingRight) {
            xScale = -1;
        }
        else {
            xScale = 1;
        }
        PlatformerPlayerAnimationManager.SetSpriteAnchorScale(xScale, 1);
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
        bool wallGripRequested = horizontalInput < 0 && PlatformerPlayerAnimationManager.FacingRight ||
                                 horizontalInput < 0 && !PlatformerPlayerAnimationManager.FacingRight;
        if (wallGripRequested && PlatformerPlayerState.WallGripAbilityUnlocked()) {
            gravityRate *= PlatformerPlayerPhysicsConfig.WallGripGravityMultiplier;
        }

        currentVelocity.y = gravityRate * Time.fixedDeltaTime;
        currentVelocity.x = 0;
        if (characterController.probeGround(-currentVelocity.y * Time.fixedDeltaTime, .95f).transform) {
            parentManager.UpdateState(PlatformerPlayerMovementStateId.Ground);
            return;
        }

        if (inputMovementWrapper.jumpInputManager.ConsumeJumpInput()) {
            currentVelocity.y = PlatformerPlayerPhysicsConfig.WallSlideVerticalJumpPower;
            currentVelocity.x = PlatformerPlayerPhysicsConfig.WallSlideHorizontalJumpPower;
            currentVelocity.x *= PlatformerPlayerAnimationManager.FacingRight ? 1 : -1;
            parentManager.UpdateState(PlatformerPlayerMovementStateId.Air);
        }
    }
}