using System;
using Chonker.Scripts.Game_Management;
using UnityEngine;

public class TimedPlatform : LevelResettable {
    [SerializeField] private float _timeInSecondsToDisappear = 1;
private Vector2 originalPosition;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private float timer;
    private LayerMask PlayerLayerMask;
    private Vector2 positionCheck;
    private float verticalOffsetForPlayerDetect = .1f;
    private float shortenWidthCheckAmount = .5f; 
    private void Start() {
        PlayerLayerMask = LayerMask.GetMask("Player");
        positionCheck = transform.position;
        _boxCollider = GetComponent<BoxCollider2D>();
        originalPosition = transform.position;
    }

    private void FixedUpdate() {
        Vector2 position = positionCheck + Vector2.up * .1f;
        Vector2 size = _boxCollider.size;
        size.x = _boxCollider.size.x - shortenWidthCheckAmount;
        Collider2D collider =
            Physics2D.OverlapBox(position, size, 0, PlayerLayerMask);
        Debug.DrawRay(position + Vector2.up * size.y / 2, Vector3.left * size.x / 2, Color.red);
        Debug.DrawRay(position + Vector2.up * size.y / 2, Vector3.right * size.x / 2, Color.red);
        if (collider) {
            timer += Time.deltaTime;
        }
        
        if (timer > _timeInSecondsToDisappear) {
            _spriteRenderer.enabled = false;
            _boxCollider.enabled = false;
        }
    }

    public override void Reset() {
        transform.position = originalPosition;
        timer = 0;
        _spriteRenderer.enabled = true;
        _boxCollider.enabled = true;
    }
}