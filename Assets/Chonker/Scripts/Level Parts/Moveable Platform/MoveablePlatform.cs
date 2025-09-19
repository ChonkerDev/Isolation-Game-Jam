using System;
using System.Collections;
using Chonker.Scripts.Game_Management;
using UnityEngine;

public class MoveablePlatform : LevelResettable {
    private Vector2 nextPosition;

    private int currentTargetIndex;
    private MoveablePlatformTargetPoint[] targetPoints;

    public Vector2 CurrentPositionDifference;

    // For interpolation:
    private Vector2 lastFixedPos;
    private Vector2 currentFixedPos;
    
    private Rigidbody2D rigidbody2D;
    public Vector2 Vel;

    private void Awake() {
        targetPoints = GetComponentsInChildren<MoveablePlatformTargetPoint>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        Reset();
    }

    private void FixedUpdate() {
        lastFixedPos = currentFixedPos;
        currentFixedPos = nextPosition;
        CurrentPositionDifference = currentFixedPos - lastFixedPos;
        rigidbody2D.MovePosition(rigidbody2D.position + CurrentPositionDifference);
        Vel = CurrentPositionDifference / Time.fixedDeltaTime;
    }

    private IEnumerator MoveToTargets() {
        int currentTargetIndex = 0;
        while (true) {
            MoveablePlatformTargetPoint currentTarget = targetPoints[currentTargetIndex];
            float timer = 0f;
            float moveTime = currentTarget.timeToMoveToNextPointInSeconds;
            Vector2 startPosition = currentFixedPos;
            Vector2 targetPosition = currentTarget.CachedGlobalPosition;

            while (timer < 1f) {
                timer += Time.fixedDeltaTime / moveTime;
                nextPosition = Vector2.Lerp(startPosition, targetPosition, timer);
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(currentTarget.DelayMoveToNextPointInSeconds);

            currentTargetIndex++;
            if (currentTargetIndex >= targetPoints.Length)
                currentTargetIndex = 0;
        }
    }

    public override void Reset() {
        StopAllCoroutines();
        transform.position = targetPoints[0].transform.position;
        nextPosition = targetPoints[0].transform.position;

        lastFixedPos = nextPosition;
        currentFixedPos = nextPosition;
        StartCoroutine(MoveToTargets());
    }
}
