using System;
using Chonker.Scripts.Game_Management;
using UnityEngine;
using UnityEngine.Events;

public class BreakableWall : LevelResettable {
    [SerializeField] private BoxCollider2D obstacleCollider2D;
    [SerializeField] private BoxCollider2D breakCheckCollider2D;
    [SerializeField] private SpriteRenderer WallNotBroken;
    [SerializeField] private float _height;
    [SerializeField] private UnityEvent _onWallbreak;
    [SerializeField] private AudioSource _breakAudioSource;

    private void Start() {
        _breakAudioSource.Play();
        _breakAudioSource.Pause();
    }

    public void BreakWall() {
        WallNotBroken.enabled = false;
        obstacleCollider2D.enabled = false;
        breakCheckCollider2D.enabled = false;
        _onWallbreak.Invoke();
        _breakAudioSource.UnPause();
    }

    public override void Reset() {
        WallNotBroken.enabled = true;
        obstacleCollider2D.enabled = true;
        breakCheckCollider2D.enabled = true;
    }

    void OnValidate() {
        base.OnValidate();
        WallNotBroken.size = new Vector2(1, _height);
        obstacleCollider2D.size = new Vector2(.58f, _height);
        breakCheckCollider2D.size = new Vector2(breakCheckCollider2D.size.x, _height);

    }
    
}
