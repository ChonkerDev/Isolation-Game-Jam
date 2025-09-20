using System;
using System.Collections;
using System.Collections.Generic;
using Chonker.Scripts.Game_Management;
using Chonker.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CollectablePlayerUpgrade : MonoBehaviour {

    [SerializeField] private float amplitude = .2f;
    [SerializeField] private float frequency = 1;
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private TextMeshPro _textMeshPro;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioSource _audioSource;
    private Light2D _light2D;
    private float startY;

    private void Start()
    {
        startY = transform.position.y;
        _textMeshPro.gameObject.SetActive(false);
        _light2D = GetComponent<Light2D>();
    }

    private void Update()
    {
        float yPos = startY + Mathf.Sin(Time.time * frequency) * amplitude;

        Vector3 position = transform.position;
        position.y = yPos;
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        _collider2D.enabled = false;
        LevelManager.PlayerInstance.PlatformerPlayerState.UnlockOmniDash();
        LevelManager.PlayerInstance.platformerPlayerAnimationManager.setPlayerNotUpgradeLayerActive(false);
        _textMeshPro.gameObject.SetActive(true);
        _spriteRenderer.enabled = false;
        _audioSource.Play();
        _light2D.enabled = false;
        StartCoroutine(delayTextMeshDisable());
    }

    private IEnumerator delayTextMeshDisable() {
        yield return new WaitForSeconds(5);
        _textMeshPro.gameObject.SetActive(false);
    }
}
