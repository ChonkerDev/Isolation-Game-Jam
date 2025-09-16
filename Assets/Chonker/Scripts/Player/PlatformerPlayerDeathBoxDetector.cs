using System;
using Chonker.Scripts.Player;
using UnityEngine;

public class PlatformerPlayerDeathBoxDetector : MonoBehaviour {
    private PlatformerPlayerComponentContainer platformerPlayerComponentContainer;
    private int KillBoxLayerIndex;
    private Collider2D collider2D;
    private void Awake() {
        platformerPlayerComponentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
        collider2D = GetComponent<Collider2D>();
    }

    private void Start() {
        KillBoxLayerIndex = LayerMask.NameToLayer("Kill Box");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer != KillBoxLayerIndex) return;
        collider2D.enabled = false;
        platformerPlayerComponentContainer.PlatformerCharacterController.KillPlayer();
    }
}
