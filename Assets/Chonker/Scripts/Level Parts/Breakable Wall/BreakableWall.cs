using Chonker.Scripts.Game_Management;
using UnityEngine;

public class BreakableWall : LevelResettable {
    [SerializeField] private Collider2D obstacleCollider2D;
    [SerializeField] private Collider2D breakCheckCollider2D;
    [SerializeField] private SpriteRenderer WallNotBroken;
    public void BreakWall() {
        WallNotBroken.enabled = false;
        obstacleCollider2D.enabled = false;
        breakCheckCollider2D.enabled = false;
    }

    public override void Reset() {
        WallNotBroken.enabled = true;
        obstacleCollider2D.enabled = true;
        breakCheckCollider2D.enabled = true;
    }
}
