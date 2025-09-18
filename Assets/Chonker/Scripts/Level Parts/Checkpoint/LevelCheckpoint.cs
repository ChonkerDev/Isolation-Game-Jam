using System;
using Chonker.Scripts.Game_Management;
using UnityEngine;

public class LevelCheckpoint : MonoBehaviour
{
    private bool checkPointTriggered = false;
private SpriteRenderer spriteRenderer;
    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start() {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (checkPointTriggered) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            triggerCheckPoint();
        }
    }

    private void triggerCheckPoint() {
        spriteRenderer.enabled = false;
        checkPointTriggered = true;
        LevelManager.instance.CurrentCheckPoint = this;
    }
}
