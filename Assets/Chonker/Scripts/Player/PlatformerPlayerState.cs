using System;
using UnityEngine;

namespace Chonker.Scripts.Player {
    public class PlatformerPlayerState : MonoBehaviour {

        private PlatformerPlayerPhysicsConfig PlatformerPlayerPhysicsConfig;
        public bool DoubleJumpUnlocked;
        public int MaxNumberOfJumps => DoubleJumpUnlocked ? 2 : 1;

        public int NumJumpsAvailable { get; private set; }

        private void Awake() {
            PlatformerPlayerPhysicsConfig = GetComponent<PlatformerPlayerPhysicsConfig>();
        }

        private void Start() {
            NumJumpsAvailable = MaxNumberOfJumps;
        }

        public void ResetNumJumps() {
            NumJumpsAvailable = MaxNumberOfJumps;
        }

        public void DecrementNumJumps() {
            NumJumpsAvailable--;
            NumJumpsAvailable = Mathf.Max(0, NumJumpsAvailable);
        }
    }
}