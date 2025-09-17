using System;
using System.Collections;
using UnityEngine;

public class MoveablePlatform : MonoBehaviour {
    private Vector2 nextPosition;

    private int currentTargetIndex;
    private MoveablePlatformTargetPoint[] targetPoints;
    private float timer;
    
    public Vector2 CurrentPositionDifference;


    private void Awake() {
        targetPoints = GetComponentsInChildren<MoveablePlatformTargetPoint>();
    }

    private void Start() {
        transform.position = targetPoints[0].transform.position;
        nextPosition = targetPoints[0].transform.position;
        StartCoroutine(MoveToTargets());
    }

    private void FixedUpdate() {
        Vector2 currentPosition = transform.position;
        CurrentPositionDifference = nextPosition - currentPosition;
        transform.position = nextPosition;

    }

    private IEnumerator MoveToTargets() {
        int currentTargetIndex = 0;
        while (true) {
            MoveablePlatformTargetPoint currentTarget = targetPoints[currentTargetIndex];
            float timer = 0;
            float moveTime = currentTarget.timeToMoveToNextPointInSeconds;
            Vector2 startPosition = transform.position;
            Vector2 targetPosition = currentTarget.CachedGlobalPosition;
            while (timer < 1) {
                timer += Time.fixedDeltaTime / moveTime;
                nextPosition = Vector2.Lerp(startPosition, targetPosition, timer);
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(currentTarget.DelayMoveToNextPointInSeconds);
            currentTargetIndex++;
            if(currentTargetIndex >= targetPoints.Length) currentTargetIndex = 0;
        }
    }
    

    
    
}