using System;
using Chonker.Scripts.Game_Management;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LevelCheckpoint : MonoBehaviour
{
    private bool checkPointTriggered = false;
    private Animator animator;
    private Light2D light2D;
    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        light2D = GetComponentInChildren<Light2D>();
    }

    private void Start() {
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
        animator.SetBool("On", true);
        light2D.enabled = true;
        checkPointTriggered = true;
        LevelManager.instance.CurrentCheckPoint = this;
    }
}
