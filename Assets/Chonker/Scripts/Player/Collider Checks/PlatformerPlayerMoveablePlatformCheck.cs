using System;
using Chonker.Scripts.Player;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformerPlayerMoveablePlatformCheck : MonoBehaviour {
    private PlatformerPlayerComponentContainer platformerPlayerComponentContainer;

    private PlatformerCharacterController platformerCharacterController =>
        platformerPlayerComponentContainer.PlatformerCharacterController;

    public const string MOVEABLE_PLATFORM_SURFACE_TAG = "Moveable Platform";
    private LayerMask obstacleLayerMask;

    private MoveablePlatform LeftMoveablePlatform;
    private MoveablePlatform RightMoveablePlatform;

    public Vector2 CurrentMovablePlatformPositionDiff {
        get {
            if (!LeftMoveablePlatform && !RightMoveablePlatform) return Vector2.zero;
            if(LeftMoveablePlatform && !RightMoveablePlatform) return LeftMoveablePlatform.CurrentPositionDifference;
            if(!LeftMoveablePlatform && RightMoveablePlatform) return RightMoveablePlatform.CurrentPositionDifference;
            
            return (LeftMoveablePlatform.CurrentPositionDifference + RightMoveablePlatform.CurrentPositionDifference) / 2;

        }
    }
    float checkWidth => platformerCharacterController.BoxSize.x / 2;
    float checkHeight => platformerCharacterController.BoxSize.y / 6;
    float leftCenterX => platformerCharacterController.MiddleOfBox.x - checkWidth / 2;
    float rightCenterX => platformerCharacterController.MiddleOfBox.x + checkWidth / 2;
    float centerY => platformerCharacterController.BottomOfBoxY;
    private Vector2 checkSize => new Vector2(checkWidth, checkHeight);
    private Vector2 leftCheckCenter => new Vector2(leftCenterX, centerY);
    private Vector2 rightCheckCenter => new Vector2(rightCenterX, centerY);


    private void Awake() {
        platformerPlayerComponentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
    }

    private void Start() {
        obstacleLayerMask = LayerMask.GetMask("Obstacle");
    }

    private void FixedUpdate() {
        CheckSide(ref LeftMoveablePlatform, leftCheckCenter);
        CheckSide(ref RightMoveablePlatform, rightCheckCenter);
        string message = $"{LeftMoveablePlatform} | {RightMoveablePlatform}";
        Debug.Log(message);
    }

    private void CheckSide(ref MoveablePlatform platform, Vector2 center) {
        Collider2D check = Physics2D.OverlapBox(center, checkSize, 0, obstacleLayerMask);
        if (!check || !check.gameObject.CompareTag(MOVEABLE_PLATFORM_SURFACE_TAG)) {
            platform = null;
            return;
        }
        if (platform?.gameObject == check.gameObject) return;
        platform = check.GetComponent<MoveablePlatform>();

    }

    private void OnDrawGizmos() {
        if (!platformerPlayerComponentContainer) return;
        Gizmos.DrawWireCube(leftCheckCenter, checkSize);
        Gizmos.DrawWireCube(rightCheckCenter, checkSize);
    }
}