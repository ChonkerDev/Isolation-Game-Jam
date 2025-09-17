using System;
using System.Collections;
using UnityEngine;

public class ForceField : MonoBehaviour {
    private LayerMask playerLayerMask;
    [SerializeField, Range(0, 4)] private float forceMagnitude;
    [SerializeField] private float TimeOnInSeconds;
    [SerializeField, Tooltip("If you only want On, set this to 0")] private float TimeOffInSeconds = 0;
    private Collider2D playerDetectionCollider;
    private float timer;
    public Vector2 Force => transform.right * forceMagnitude;

    IEnumerator Start() {
        playerDetectionCollider = GetComponent<Collider2D>();
        bool processOffAndOn = TimeOffInSeconds > 0;
        while (processOffAndOn) {
            playerDetectionCollider.enabled = true;
            yield return new WaitForSeconds(TimeOnInSeconds);
            playerDetectionCollider.enabled = false;
            yield return new WaitForSeconds(TimeOffInSeconds);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawRay(transform.position, Force);
    }
}