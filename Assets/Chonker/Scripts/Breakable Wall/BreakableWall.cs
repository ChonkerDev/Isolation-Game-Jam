using UnityEngine;

public class BreakableWall : MonoBehaviour {
    [SerializeField] private Collider2D obstacleCollider2D;
    [SerializeField] private Collider2D breakCheckCollider2D;
    [SerializeField] private SpriteRenderer WallNotBroken;
    public void BreakWall() {
        WallNotBroken.enabled = false;
        obstacleCollider2D.enabled = false;
        breakCheckCollider2D.enabled = false;
    }
}
