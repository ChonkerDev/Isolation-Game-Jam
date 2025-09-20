using System;
using System.Collections;
using Chonker.Scripts.Game_Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class LevelCheckpoint : MonoBehaviour
{
    private bool checkPointTriggered = false;
    private Animator animator;
    private Light2D light2D;
    [SerializeField] private UnityEvent _onCheckpointTriggered;
    [SerializeField] private TextMeshPro text;
    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        light2D = GetComponentInChildren<Light2D>();
    }

    private void Start() {
        text.gameObject.SetActive(false);
        animator.SetBool("On", false);
        light2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (checkPointTriggered) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            triggerCheckPoint();
        }
    }

    private void triggerCheckPoint() {
        text.gameObject.SetActive(true);
        _onCheckpointTriggered.Invoke();
        animator.SetBool("On", true);
        light2D.enabled = true;
        checkPointTriggered = true;
        LevelManager.instance.CurrentCheckPoint = this;
        StartCoroutine(DelayTextOff());
    }

    private IEnumerator DelayTextOff() {
        yield return new WaitForSecondsRealtime(3f);
        text.gameObject.SetActive(false);
    }
}
