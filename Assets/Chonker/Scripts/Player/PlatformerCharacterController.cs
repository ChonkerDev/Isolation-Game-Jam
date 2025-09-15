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
    private Coroutine gravityCoroutine;

    private void Awake() {
        rigidbody2D = GetComponentInParent<Rigidbody2D>();
        platformerPlayerComponentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
        platformerPlayerMovementStateManager = GetComponent<PlatformerPlayerMovementStateManager>();
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
        Debug.Log(direction);
        Debug.DrawRay(position, direction * distance, Color.red);
        RaycastHit2D hit = Physics2D.BoxCast(position, boxCollider2D.size, 0,
            direction, distance,
            ObstacleMask);
        return hit.transform;
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