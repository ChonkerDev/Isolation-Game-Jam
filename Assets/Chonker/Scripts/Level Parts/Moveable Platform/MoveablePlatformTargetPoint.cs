using System;
using UnityEngine;

public class MoveablePlatformTargetPoint : MonoBehaviour {
    public float timeToMoveToNextPointInSeconds;
    public float DelayMoveToNextPointInSeconds;

    [HideInInspector] public Vector2 CachedGlobalPosition;

    private void Awake() {
        CachedGlobalPosition = transform.position;
    }
}
