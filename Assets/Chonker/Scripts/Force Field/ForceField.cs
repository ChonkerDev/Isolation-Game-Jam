using System;
using UnityEngine;

public class ForceField : MonoBehaviour {
    private LayerMask playerLayerMask;
    [SerializeField, Range(0, 4)] private float forceMagnitude;

    public Vector2 Force => transform.right * forceMagnitude;

    private void OnDrawGizmos() {
        Gizmos.DrawRay(transform.position, Force);
    }
}