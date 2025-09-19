using System;
using UnityEngine;

namespace Chonker.Scripts.Level_Parts.Falling_Platform {
    public class TimedPlatformTrigger : MonoBehaviour {
        private TimedPlatform timedPlatform;

        private int playerLayer;
        private void Awake() {
            timedPlatform = GetComponentInParent<TimedPlatform>();
        }

        private void Start() {
            playerLayer =  LayerMask.NameToLayer("Player");
        }

        private void OnTriggerStay2D(Collider2D other) {
            if (!timedPlatform.isInactive && other.gameObject.layer == playerLayer) {
                timedPlatform.Timer += Time.deltaTime;
            }
        }
    }
}