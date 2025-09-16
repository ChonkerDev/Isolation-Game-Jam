using System;
using System.Collections;
using Chonker.Scripts.Player;
using Chonker.Scripts.Player.States;
using UnityEngine;

public class PlatformerCharacterController : MonoBehaviour {
    private Rigidbody2D rigidbody2D;
    [SerializeField, HideInInspector] private BoxCollider2D boxCollider2D;
    private PlatformerPlayerComponentContainer platformerPlayerComponentContainer;
    private Vector2 currentVelocity;
    public RaycastHit2D CurrentGroundHit { get; private set; }
    public bool Grounded => CurrentGroundHit.transform != null;

    [SerializeField] private float _boxColliderHeight = 1;
    [SerializeField] private float _boxColliderWidth = 1;
    
    [SerializeField] private bool ReportGroundedState;
    private LayerMask ObstacleMask;

    private PlatformerPlayerMovementStateManager platformerPlayerMovementStateManager;
    
    public float CurrentGravity { get; private set; }
    public Vector2 RbVelocity => rigidbody2D.linearVelocity;
    private Coroutine gravityCoroutine;

    public PlatformerPlayerMovementStateId CurrentMovementStateId => platformerPlayerMovementStateManager.CurrentState;
    
    private void Awake() {
        rigidbody2D = GetComponentInParent<Rigidbody2D>();
        platformerPlayerComponentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
        platformerPlayerMovementStateManager = GetComponentInChildren<PlatformerPlayerMovementStateManager>();
    }

    private void Start() {
        ObstacleMask = LayerMask.GetMask("Obstacle");
        CurrentGravity = platformerPlayerComponentContainer.PlatformerPlayerPhysicsConfig.GravityRate;
    }

    private void Update() {
        platformerPlayerMovementStateManager.GetCurrentState().OnUpdate();

    }

    private void FixedUpdate() {
        Vector2 currentVelocity = rigidbody2D.linearVelocity;
        float probeBuffer = .1f;
        float distanceCheck = -rigidbody2D.linearVelocity.y * Time.fixedDeltaTime + probeBuffer;
        CurrentGroundHit = probeGround(distanceCheck);
        platformerPlayerMovementStateManager.GetCurrentState().OnFixedUpdate(ref currentVelocity);
        rigidbody2D.linearVelocity = currentVelocity;
        ProbeCeiling(1);
    }

    public RaycastHit2D probeGround(float distance) {
        Vector2 position = transform.position;
        position += boxCollider2D.offset;
        return Physics2D.BoxCast(position, boxCollider2D.size, 0, Vector2.down, distance,
            ObstacleMask);
    }

    public bool ProbeForWall(Vector2 direction, float distance) {
        Vector2 position = transform.position;
        position += boxCollider2D.offset;
        Debug.DrawRay(position, direction * distance, Color.red);
        RaycastHit2D hit = Physics2D.BoxCast(position, boxCollider2D.size, 0,
            direction, distance,
            ObstacleMask);
        return hit.transform;
    }

    public void ProbeCeiling(float distanceFromTopOfBox) {
        float yPosition = rigidbody2D.position.y + boxCollider2D.offset.y + boxCollider2D.size.y / 2;
        float baseXPosition = rigidbody2D.position.x;
        float distance = boxCollider2D.size.y / 2 + distanceFromTopOfBox;
        float boxHalfWidth = boxCollider2D.size.x / 2;
        int numRaycasts = 4;
        RaycastHit2D hit2D;
        for (int i = 0; i < numRaycasts; i++) {
            float alpha = i / ((float)numRaycasts - 1);
            float xPosition = Mathf.Lerp(baseXPosition - boxHalfWidth, baseXPosition + boxHalfWidth, alpha);
            Vector2 position = new Vector2(xPosition, yPosition);
            Debug.DrawRay(position, Vector2.up * distance, Color.red);
            hit2D = Physics2D.Raycast(position, Vector2.up, distance, ObstacleMask);
            if (hit2D.transform) {
                break;
            }
        }
        
        
    }
    
    public RaycastHit2D ProbeForWallHit(Vector2 direction, float distance) {
        Vector2 position = transform.position;
        position += boxCollider2D.offset;
        Debug.DrawRay(position, direction * distance, Color.red);
        RaycastHit2D hit = Physics2D.BoxCast(position, boxCollider2D.size, 0,
            direction, distance,
            ObstacleMask);
        return hit;
    }

    public void ApplyHighJumpGravityForDuration() {
        if (gravityCoroutine != null) {
            StopCoroutine(gravityCoroutine);
        } 
        gravityCoroutine = StartCoroutine(applyGravityForHighJumpI());
    }

    private IEnumerator applyGravityForHighJumpI() {
        float timer = 1;
        while (timer > 0) {
            if (platformerPlayerComponentContainer.InputMovementWrapper.jumpInputManager.IsJumpHeld) {
                CurrentGravity = platformerPlayerComponentContainer.PlatformerPlayerPhysicsConfig.HighJumpGravityRate;
            }
            else {
                CurrentGravity = platformerPlayerComponentContainer.PlatformerPlayerPhysicsConfig.GravityRate;
            }
            float time = platformerPlayerComponentContainer.PlatformerPlayerPhysicsConfig.JumpPower / CurrentGravity;
            timer -= Time.deltaTime / time;
            yield return null;
        }
        CurrentGravity = platformerPlayerComponentContainer.PlatformerPlayerPhysicsConfig.GravityRate;
        gravityCoroutine = null;
    }

    private void OnValidate() {
        if (!boxCollider2D) {
            boxCollider2D = GetComponentInParent<BoxCollider2D>();
        }

        boxCollider2D.size = new Vector2(_boxColliderWidth, _boxColliderHeight);
        boxCollider2D.offset = new Vector2(0, _boxColliderHeight / 2);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.forestGreen;
        Vector2 position = boxCollider2D.transform.position;
        Gizmos.DrawWireCube(position + boxCollider2D.offset, boxCollider2D.size);
    }
}