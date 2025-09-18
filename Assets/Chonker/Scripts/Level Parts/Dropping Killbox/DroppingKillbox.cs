using System.Collections;
using Chonker.Scripts.Game_Management;
using UnityEngine;

public class DroppingKillbox : LevelResettable {
    [SerializeField] private float _fallRate;
    private float currentVelocity;
    private Vector2 originalPosition;
    private DroppingKillboxPlayerDetector playerDetector;
    private float boxHalfHeight;
private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        originalPosition = transform.position;
        playerDetector = GetComponentInChildren<DroppingKillboxPlayerDetector>();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        boxHalfHeight = collider.size.y;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void triggerFall() {
        StartCoroutine(ProcessFall());
    }

    private IEnumerator ProcessFall() {
        float fallTime = 10;
        float fallTimer = fallTime;
        LayerMask groundLayerMask = LayerMask.GetMask("Obstacle");
        while (fallTimer > 0) {
            if (Physics2D.Raycast(transform.position, Vector2.down, boxHalfHeight, groundLayerMask)) {
                explode();
                break;
            }
            fallTimer -= Time.deltaTime;
            currentVelocity -= _fallRate * Time.deltaTime;
            transform.position -= Vector3.down * currentVelocity;
            yield return null;
        }
    }

    private void explode() {
        spriteRenderer.enabled = false;
    }

    public override void Reset() {
        StopAllCoroutines();
        transform.position = originalPosition;
        playerDetector.gameObject.SetActive(true);
        spriteRenderer.enabled = true;
    }
}