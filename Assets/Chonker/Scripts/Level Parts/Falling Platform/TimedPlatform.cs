using System;
using Chonker.Scripts.Game_Management;
using Chonker.Scripts.Level_Parts.Falling_Platform;
using UnityEngine;

public class TimedPlatform : LevelResettable {
    [SerializeField] private float _timeInSecondsToDisappear = 1;
    private Vector2 originalPosition;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    [HideInInspector] public float Timer;
    private float verticalOffsetForPlayerDetect = .1f;
    private float shortenWidthCheckAmount = .5f;
    [SerializeField] private float timeToReturn;
    [HideInInspector] public bool isInactive = false;
    private float InactiveTimer;

    private void Awake() {
        
    }

    private void Start() {
        _boxCollider = GetComponent<BoxCollider2D>();
        originalPosition = transform.position;
    }

    private void FixedUpdate() {
        if (Timer > _timeInSecondsToDisappear && !isInactive) {
            _spriteRenderer.enabled = false;
            _boxCollider.enabled = false;
            isInactive = true;
        }

        if (isInactive) {
            InactiveTimer += Time.deltaTime;
            if (InactiveTimer > timeToReturn) {
                Timer = 0;
                InactiveTimer = 0;
                _spriteRenderer.enabled = true;
                _boxCollider.enabled = true;
                isInactive = false;
            }
        }
    }

    public override void Reset() {
        transform.position = originalPosition;
        Timer = 0;
        InactiveTimer = 0;
        isInactive = false;
        _spriteRenderer.enabled = true;
        _boxCollider.enabled = true;
    }
}