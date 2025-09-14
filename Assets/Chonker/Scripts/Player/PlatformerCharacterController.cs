using System;
using Chonker.Scripts.Player;
using UnityEngine;

public class PlatformerCharacterController : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    private PlatformerPlayerComponentContainer  platformerPlayerComponentContainer;
    private bool jumpRequested;
    private Vector2 currentVelocity;

    [SerializeField] private float _boxColliderHeight = 1;
    [SerializeField] private float _boxColliderWidth = 1;
    [SerializeField] private float jumpForce = 10;
    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        platformerPlayerComponentContainer.GetComponentInChildren<PlatformerPlayerComponentContainer>();
    }

    private void Update() {
        if (platformerPlayerComponentContainer.InputMovementWrapper.WasJumpPressedThisFrame()) {
            jumpRequested = true;
        }
    }

    private void FixedUpdate() {
        if (jumpRequested) {
            jumpRequested = false;
            currentVelocity += Vector2.up * (jumpForce * Time.fixedDeltaTime);
        }

        Vector2 currentMovement = platformerPlayerComponentContainer.InputMovementWrapper.ReadMovementInput();
        rigidbody2D.linearVelocity = currentMovement;
    }

    private void OnValidate() {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(_boxColliderWidth, _boxColliderHeight);
    }
}
