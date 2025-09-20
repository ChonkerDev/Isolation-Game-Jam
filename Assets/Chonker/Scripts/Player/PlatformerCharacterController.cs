using System;
using System.Collections;
using Chonker.Scripts.Player;
using Chonker.Scripts.Player.States;
using UnityEngine;

public class PlatformerCharacterController : MonoBehaviour {
    private Rigidbody2D rigidbody2D;
    [SerializeField, HideInInspector] private BoxCollider2D _boxCollider2D;
    [SerializeField] private float _boxColliderWidth = 1;
    [SerializeField] private float _boxColliderHeight = 1;
    [SerializeField] private PlatformerPlayerComponentContainer platformerPlayerComponentContainer;
    private Vector2 currentVelocity;
    public RaycastHit2D CurrentGroundHit { get; private set; }
    public bool Grounded => CurrentGroundHit.transform != null;

    [SerializeField] private bool ReportGroundedState;
    private LayerMask ObstacleMask;
    private LayerMask BreakableWallMask;

    public float CurrentGravity { get; private set; }
    public Vector2 RbVelocity => rigidbody2D.linearVelocity;
    private Coroutine gravityCoroutine;

    public float TopOfBoxY => _boxCollider2D.transform.position.y + _boxCollider2D.size.y / 2 + _boxCollider2D.offset.y;

    public float BottomOfBoxY =>
        _boxCollider2D.transform.position.y - _boxCollider2D.size.y / 2 + _boxCollider2D.offset.y;

    public float MiddleOfBoxY => _boxCollider2D.transform.position.y + _boxCollider2D.offset.y;

    public Vector2 MiddleOfBox =>
        new Vector2(_boxCollider2D.transform.position.x, _boxCollider2D.transform.position.y) + _boxCollider2D.offset;

    public float LeftBoxEdgeX => _boxCollider2D.transform.position.x - _boxCollider2D.size.x / 2;
    public float RightBoxEdgeX => _boxCollider2D.transform.position.x + _boxCollider2D.size.x / 2;
    public Vector2 BoxSize => _boxCollider2D.size;

    public PlatformerPlayerMovementStateId CurrentMovementStateId =>
        platformerPlayerComponentContainer.PlatformerPlayerMovementStateManager.CurrentState;

    private bool teleportThisFrame;
    private Vector2 teleportLocation;


    private void Awake() {
        rigidbody2D = GetComponentInParent<Rigidbody2D>();
        platformerPlayerComponentContainer.PlatformerPlayerMovementStateManager =
            GetComponentInChildren<PlatformerPlayerMovementStateManager>();
    }

    private void Start() {
        ObstacleMask = LayerMask.GetMask("Obstacle");
        BreakableWallMask = LayerMask.GetMask("Breakable Wall");
        CurrentGravity = platformerPlayerComponentContainer.PhysicsConfigSO.GravityRate;
    }

    private void Update() {
        platformerPlayerComponentContainer.PlatformerPlayerMovementStateManager.GetCurrentState().OnUpdate();
    }

    private void FixedUpdate() {
        if (teleportThisFrame) {
            teleportThisFrame = false;
            rigidbody2D.position = teleportLocation;
        }
        Vector2 currentVelocity = rigidbody2D.linearVelocity;
        float probeBuffer = .1f;
        float distanceCheck = -rigidbody2D.linearVelocity.y * Time.fixedDeltaTime + probeBuffer;
        CurrentGroundHit = probeGround(distanceCheck);
        platformerPlayerComponentContainer.PlatformerPlayerMovementStateManager.GetCurrentState()
            .OnFixedUpdate(ref currentVelocity);
        currentVelocity += platformerPlayerComponentContainer.PlatformerPlayerForceFieldDetector.CurrentForceFieldForce;
        currentVelocity = Vector2.ClampMagnitude(currentVelocity,
            platformerPlayerComponentContainer.PhysicsConfigSO.GlobalTerminalVelocity);
        rigidbody2D.linearVelocity = currentVelocity;

        //rigidbody2D.MovePosition(rigidbody2D.position + );
        ProbeCeiling(currentVelocity.y * Time.fixedDeltaTime);
    }

    public RaycastHit2D probeGround(float distance, float probeBoxScale = 1) {
        Vector2 position = transform.position;
        position += _boxCollider2D.offset;
        Vector2 currentPlatformDiff =
            platformerPlayerComponentContainer.PlatformerPlayerState.CurrentMoveablePlatformPositionDiff;
        Vector2 direction = Vector2.down;

        float finalDistance = distance + Mathf.Abs(currentPlatformDiff.y * 2);
        Debug.DrawRay(position, Vector3.down * distance, Color.brown);
        return Physics2D.BoxCast(position, _boxCollider2D.size * probeBoxScale, 0, direction, finalDistance,
            ObstacleMask);
    }

    public bool ProbeForWall(Vector2 direction, float distance) {
        Vector2 position = transform.position;
        position += _boxCollider2D.offset;
        Debug.DrawRay(position, direction * distance, Color.red);
        RaycastHit2D hit = Physics2D.BoxCast(position, _boxCollider2D.size, 0,
            direction, distance,
            ObstacleMask);
        return hit.transform;
    }

    public void ProbeCeiling(float distanceFromTopOfBox) {
        float boxHalfHeight = _boxCollider2D.size.y / 2;
        float margin = platformerPlayerComponentContainer.PhysicsConfigSO.CeilingPassthroughMargin;

        float totalDistance = boxHalfHeight + distanceFromTopOfBox;
        Vector2 leftPosition = new Vector2(LeftBoxEdgeX, MiddleOfBoxY);
        Vector2 rightPosition = new Vector2(RightBoxEdgeX, MiddleOfBoxY);
        Vector2 leftPositionWithMargin = leftPosition;
        leftPositionWithMargin.x += margin;
        Vector2 rightPositionWithMargin = rightPosition;
        rightPositionWithMargin.x -= margin;

        Debug.DrawRay(leftPosition, Vector2.up * totalDistance, Color.red);
        Debug.DrawRay(leftPositionWithMargin, Vector2.up * totalDistance, Color.red);
        Debug.DrawRay(rightPosition, Vector2.up * totalDistance, Color.red);
        Debug.DrawRay(rightPositionWithMargin, Vector2.up * totalDistance, Color.red);

        if (rigidbody2D.linearVelocityY <= 0) return;
        RaycastHit2D leftHit =
            Physics2D.Raycast(leftPosition, Vector2.up, totalDistance, ObstacleMask);
        if (leftHit.transform) {
            RaycastHit2D leftHitWithMargin =
                Physics2D.Raycast(leftPositionWithMargin, Vector2.up, totalDistance, ObstacleMask);
            if (leftHitWithMargin.transform) {
                return;
            }

            Vector2 newPosition = rigidbody2D.position;
            newPosition.x += margin;
            rigidbody2D.MovePosition(newPosition);
        }

        RaycastHit2D rightHit =
            Physics2D.Raycast(rightPosition, Vector2.up, totalDistance, ObstacleMask);
        if (rightHit.transform) {
            RaycastHit2D rightHitWithMargin =
                Physics2D.Raycast(rightPositionWithMargin, Vector2.up, totalDistance, ObstacleMask);
            if (rightHitWithMargin.transform) {
                return;
            }

            Vector2 newPosition = rigidbody2D.position;
            newPosition.x -= margin;
            rigidbody2D.MovePosition(newPosition);
        }
    }

    public void TeleportX(float x) {
        Vector2 position = rigidbody2D.position;
        position.x = x;
        rigidbody2D.MovePosition(position);
    }

    public void TeleportY(float y) {
        Vector2 position = rigidbody2D.position;
        position.y = y;
        rigidbody2D.MovePosition(position);
    }

    public void Teleport(Vector2 position) {
        teleportThisFrame = true;
        teleportLocation = position;
    }

    public void setTargetRotation(float rotation) {
        rigidbody2D.SetRotation(Quaternion.Euler(0, 0, rotation));
    }

    public RaycastHit2D ProbeForWallHit(Vector2 direction, float distance) {
        Vector2 position = transform.position;
        position += _boxCollider2D.offset;
        Debug.DrawRay(position, direction * distance, Color.red);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        RaycastHit2D hit = Physics2D.BoxCast(position, _boxCollider2D.size, angle,
            direction, distance,
            ObstacleMask);
        return hit;
    }

    public RaycastHit2D ProbeForBreakableWall(Vector2 direction, float distance) {
        Vector2 position = transform.position;
        position += _boxCollider2D.offset;
        Debug.DrawRay(position, direction * distance, Color.red);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        RaycastHit2D hit = Physics2D.BoxCast(position, _boxCollider2D.size, angle,
            direction, distance,
            BreakableWallMask);
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
                CurrentGravity = platformerPlayerComponentContainer.PhysicsConfigSO.HighJumpGravityRate;
            }
            else {
                CurrentGravity = platformerPlayerComponentContainer.PhysicsConfigSO.GravityRate;
            }

            float time = platformerPlayerComponentContainer.PhysicsConfigSO.JumpPower / CurrentGravity;
            timer -= Time.deltaTime / time;
            yield return null;
        }

        CurrentGravity = platformerPlayerComponentContainer.PhysicsConfigSO.GravityRate;
        gravityCoroutine = null;
    }

    public void KillPlayer() {
        platformerPlayerComponentContainer.PlatformerPlayerMovementStateManager.UpdateState(
            PlatformerPlayerMovementStateId.Dead);
    }

    private void OnValidate() {
        if (!platformerPlayerComponentContainer) {
            platformerPlayerComponentContainer = transform.parent.GetComponent<PlatformerPlayerComponentContainer>();
        }

        if (!_boxCollider2D) {
            _boxCollider2D = platformerPlayerComponentContainer.GetComponent<BoxCollider2D>();
        }

        _boxCollider2D.size = new Vector2(_boxColliderWidth, _boxColliderHeight);

        platformerPlayerComponentContainer.PlatformerPlayerDeathBoxDetector.UpdateBoxCollider(_boxCollider2D.size);
        platformerPlayerComponentContainer.PlatformerPlayerForceFieldDetector.UpdateBoxCollider(_boxCollider2D.size);
    }
}