using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerForceFieldDetector : MonoBehaviour {
    private int forceFieldLayer;
    public Vector2 CurrentForceFieldForce;
    [SerializeField] private BoxCollider2D _collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        forceFieldLayer = LayerMask.NameToLayer("Force Field");
    }

    public bool IsForceFieldPresent() {
        return CurrentForceFieldForce.sqrMagnitude > .1f;
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

    public void UpdateBoxCollider(Vector2 size) {
        if (!_collider) {
            _collider = GetComponent<BoxCollider2D>();
        }
        
        _collider.size = size;
    }
}