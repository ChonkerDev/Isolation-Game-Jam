using System;
using Chonker.Scripts.Game_Management;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class CollectableFlower : LevelResettable {

    [SerializeField] private bool _transformBobEnabled = false;
    [SerializeField] private float amplitude = .2f;
    [SerializeField] private float frequency = 1;
    [SerializeField] private BoxCollider2D _obstacleCollider2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private UnityEvent _onCollected;
    private AudioSource _audioSource;
    private Light2D _light2D;
    private float startY;
    private bool collected;

    private void Start() {
        _audioSource = GetComponentInChildren<AudioSource>();
        _light2D = GetComponentInChildren<Light2D>();
        startY = transform.position.y;

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        collectFlower();
    }
    
    private void Update() {
        if (!_transformBobEnabled) return;
        float yPos = startY + Mathf.Sin(Time.time * frequency) * amplitude;

        Vector3 position = transform.position;
        position.y = yPos;
        transform.position = position;
    }

    private void collectFlower() {
        if (!collected) {
            LevelManager.instance.IncrementNumCollectedFlowers();
        }
        _onCollected.Invoke();
        _obstacleCollider2D.enabled = false;
        _spriteRenderer.enabled = false;

         int numFlowrsCollected = LevelManager.instance.NumCollectedFlowers;
        int totalNumFlowers = LevelManager.instance.TotalFlowerCount;
        float alpha = (float) numFlowrsCollected / totalNumFlowers;
        _audioSource.pitch = Mathf.Lerp(1, 2, alpha);
        _audioSource.Play();
        _light2D.gameObject.SetActive(false);
        collected = true;
    }

    public override void Reset() {
        if (!collected) {
            return;
        }
        //_light2D.gameObject.SetActive(true);
        _obstacleCollider2D.enabled = true;
        _spriteRenderer.enabled = true;
        Color color = Color.black;
        color.a = .6f;
        _spriteRenderer.color = color;

    }
}
