using System;
using Chonker.Scripts.Player;
using UnityEngine;

public class PlatformerPlayerDeathBoxDetector : MonoBehaviour {
    private PlatformerPlayerComponentContainer platformerPlayerComponentContainer;
    private int KillBoxLayerIndex;
    [SerializeField] private BoxCollider2D _collider2D;
    private void Awake() {
        platformerPlayerComponentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
    }

    private void Start() {
        KillBoxLayerIndex = LayerMask.NameToLayer("Kill Box");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer != KillBoxLayerIndex) return;
        platformerPlayerComponentContainer.PlatformerCharacterController.KillPlayer();
    }
    
    public void UpdateBoxCollider(Vector2 size) {
        if (!_collider2D) {
            _collider2D = GetComponent<BoxCollider2D>();
        }
        
        _collider2D.size = size;
    }
}
