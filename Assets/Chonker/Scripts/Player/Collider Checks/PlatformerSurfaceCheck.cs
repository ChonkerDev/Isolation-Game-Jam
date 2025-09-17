using System;
using Chonker.Scripts.Player;
using Chonker.Scripts.Player.Collider_Checks;
using UnityEngine;

public class PlatformerSurfaceCheck : MonoBehaviour {
    private PlatformerPlayerComponentContainer platformerPlayerComponentContainer;
    private PlatformerPlayerState platformerPlayerState => platformerPlayerComponentContainer.PlatformerPlayerState;
    private PlatformerPlayerPhysicsConfigSO platformerPlayerPhysicsConfig => platformerPlayerComponentContainer.PhysicsConfigSO;
    public const string SLIPPER_SURFACE_TAG = "Slippery Platform";
    private LayerMask obstacleLayerMask;

    private MoveablePlatform lastFrameFoundMoveablePlatform;
    private int obstacleLayerIndex;
    private RaycastHit2D[] obstacles;
    private readonly RaycastHit2D[] raycastResults = new RaycastHit2D[3];

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
        //Debug.DrawRay(position, Vector2.down * distance, Color.lightBlue );
        int hitCount = Physics2D.RaycastNonAlloc(position, Vector2.down, raycastResults, distance, obstacleLayerMask);

        if (hitCount == 0) {
            platformerPlayerState.CurrentSurfaceType = SurfaceType.None;
            return;
        }

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D hit = raycastResults[i];
            if (hit.transform.CompareTag(SLIPPER_SURFACE_TAG)) {

                platformerPlayerState.CurrentSurfaceType = SurfaceType.Ice;
                return;
            }
        }
        
        platformerPlayerState.CurrentSurfaceType = SurfaceType.None;
    }
}