using System;
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

    private void Awake() {
        rigidbody2D = GetComponentInParent<Rigidbody2D>();
        platformerPlayerComponentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
        platformerPlayerMovementStateManager = GetComponent<PlatformerPlayerMovementStateManager>();
    }

    private void Start() {
        ObstacleMask = LayerMask.GetMask("Obstacle");
    }

    private void Update() {
        platformerPlayerMovementStateManager.GetCurrentState().OnUpdate();

    }

    private void FixedUpdate() {
        Vector2 currentVelocity = rigidbody2D.linearVelocity;
        probeGround();
        platformerPlayerMovementStateManager.GetCurrentState().OnFixedUpdate(ref currentVelocity);
        rigidbody2D.linearVelocity = currentVelocity;
    }

    private void probeGround() {
        Vector2 position = transform.position;
        position += boxCollider2D.offset;
        float probeBuffer = .1f;
        float distanceCheck = rigidbody2D.linearVelocity.y * Time.fixedDeltaTime + probeBuffer;
        CurrentGroundHit = Physics2D.BoxCast(position, boxCollider2D.size, 0, Vector2.down, distanceCheck,
            ObstacleMask);
        ReportGroundedState = CurrentGroundHit;
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