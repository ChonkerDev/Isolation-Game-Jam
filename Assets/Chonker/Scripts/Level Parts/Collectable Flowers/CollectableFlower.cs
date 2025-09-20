using System;
using Chonker.Scripts.Game_Management;
using UnityEngine;
using UnityEngine.Events;

public class CollectableFlower : MonoBehaviour {

    [SerializeField] private BoxCollider2D _obstacleCollider2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private UnityEvent _onCollected;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        collectFlower();
    }

    private void collectFlower() {
        LevelManager.instance.NumCollectedFlowers++;
        _onCollected.Invoke();
        _obstacleCollider2D.enabled = false;
        _spriteRenderer.enabled = false;
    }
}
