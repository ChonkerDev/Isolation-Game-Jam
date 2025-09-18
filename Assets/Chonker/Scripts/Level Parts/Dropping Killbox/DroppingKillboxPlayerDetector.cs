using System;
using UnityEngine;

public class DroppingKillboxPlayerDetector : MonoBehaviour {
    private DroppingKillbox DroppingKillbox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        DroppingKillbox = GetComponentInParent<DroppingKillbox>();
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            DroppingKillbox.triggerFall();
            gameObject.SetActive(false);
        }
    }
}
