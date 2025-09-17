using System;
using Chonker.Scripts.Player;
using UnityEngine;

public class PlatformerSurfaceCheck : MonoBehaviour {
    private PlatformerPlayerComponentContainer platformerPlayerComponentContainer;
    private PlatformerPlayerState platformerPlayerState => platformerPlayerComponentContainer.PlatformerPlayerState;
    private PlatformerPlayerPhysicsConfig platformerPlayerPhysicsConfig => platformerPlayerComponentContainer.PlatformerPlayerPhysicsConfig;
    public const string MOVEABLE_PLATFORM_SURFACE_TAG = "Moveable Platform";
    public const string SLIPPER_SURFACE_TAG = "Slippery Platform";
    private LayerMask obstacleLayerMask;

    private MoveablePlatform lastFrameFoundMoveablePlatform;
    private int obstacleLayerIndex;

    private void Awake() {
        platformerPlayerComponentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
    }

    private void Start() {
        obstacleLayerIndex = LayerMask.NameToLayer("Obstacle");
        obstacleLayerMask = LayerMask.GetMask("Obstacle");
    }

    private void FixedUpdate() {
        Vector2 position = transform.position;
        float distance = platformerPlayerComponentContainer.PlatformerCharacterController.BoxSize.y / 2 + .1f;
        Debug.DrawRay(position, Vector2.down * distance, Color.lightBlue );
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, distance, obstacleLayerMask); //TODO: need to figure out best way to handle alternate surfaces without breaking the tilemape copmposite collider
        if (hit.transform) {
            GameObject foundObject = hit.transform.gameObject;
            if (foundObject.CompareTag(MOVEABLE_PLATFORM_SURFACE_TAG)) {
                if (lastFrameFoundMoveablePlatform?.gameObject == foundObject) {
                    platformerPlayerState.CurrentMoveablePlatformPositionDiff = lastFrameFoundMoveablePlatform.CurrentPositionDifference;
                    return;
                }
                MoveablePlatform foundMoveablePlatform = foundObject.GetComponent<MoveablePlatform>();
                platformerPlayerState.CurrentMoveablePlatformPositionDiff = foundMoveablePlatform.CurrentPositionDifference;

                lastFrameFoundMoveablePlatform = foundMoveablePlatform;
                return;
            }
            else {
                lastFrameFoundMoveablePlatform = null;
                platformerPlayerState.CurrentMoveablePlatformPositionDiff = Vector2.zero;
            }
            
            if (foundObject.CompareTag(SLIPPER_SURFACE_TAG)) {
                if (!hit.collider.isTrigger) {
                    Debug.LogError("Slppery surfaces, or any other surface with a non regular friction should be ");
                }
                platformerPlayerState.CurrentSurfaceAccelerationCoefficient =
                    platformerPlayerPhysicsConfig.slipperySurfaceCoefficient;
                return;
            }
            else {
                platformerPlayerState.CurrentSurfaceAccelerationCoefficient = 1;
            }
        }
    }
}