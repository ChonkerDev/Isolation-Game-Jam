using System;
using UnityEngine;

namespace Chonker.Scripts.Player {
    public class PlatformerPlayerState : MonoBehaviour {

        private PlatformerPlayerPhysicsConfig PlatformerPlayerPhysicsConfig;
        public bool DoubleJumpUnlocked;
        public int MaxNumberOfJumps => DoubleJumpUnlocked ? 2 : 1;
        public int MaxNumberOfDashes => 1;
        [SerializeField] private bool allowOmniDirectionDashing;
        public int NumDashesAvailable;
        public int NumJumpsAvailable { get; private set; }
        public bool facingRight = true;

        private void Awake() {
            PlatformerPlayerPhysicsConfig = GetComponent<PlatformerPlayerPhysicsConfig>();
        }

        private void Start() {
            ResetNumDashes();
            ResetNumJumps();
        }

        public void ResetNumJumps() {
            NumJumpsAvailable = MaxNumberOfJumps;
        }

        public void DecrementNumJumps() {
            NumJumpsAvailable--;
            NumJumpsAvailable = Mathf.Max(0, NumJumpsAvailable);
        }

        public void ResetNumDashes() {
            NumDashesAvailable = MaxNumberOfDashes;
        }

        public void DecrementNumberOfDashes() {
            NumDashesAvailable--;
            NumDashesAvailable = Mathf.Max(0, NumJumpsAvailable);
        }

        public bool AllowedToDash() {
            return NumDashesAvailable > 0;
        }

        public bool AllowedToOmniDirectionalDash() {
            return allowOmniDirectionDashing && PlatformerPlayerPhysicsConfig.AllowOmniDirectionalDash;
        }
    }
}