using System;
using System.Collections;
using UnityEngine;

public class MoveablePlatform : MonoBehaviour {
    public Vector2 moveDirection = Vector2.right;
    public float distance = 5f;
    public float speed = 2f;

    private Vector2 startPosition;
    private Vector2 lastPosition;

    private void Start() {
        startPosition = transform.position;
        StartCoroutine(PingPongMove());
    }

    private IEnumerator PingPongMove() {
        while (true) {
            float offset = Mathf.PingPong(Time.time * speed, distance);
            Vector2 newPosition = startPosition + moveDirection.normalized * offset;
            CurrentPositionDifference = newPosition - lastPosition;
            transform.position = newPosition;
            lastPosition = newPosition;
            yield return new WaitForFixedUpdate();
        }
    }

    public Vector2 CurrentPositionDifference;
}