using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerForceFieldDetector : MonoBehaviour {
    private int forceFieldLayer;
    public Vector2 CurrentForceFieldForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        forceFieldLayer = LayerMask.NameToLayer("Force Field");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (forceFieldLayer == other.gameObject.layer) {
            CurrentForceFieldForce += other.gameObject.GetComponent<ForceField>().Force;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (forceFieldLayer == other.gameObject.layer) {
            CurrentForceFieldForce -= other.gameObject.GetComponent<ForceField>().Force;
        }
    }
}