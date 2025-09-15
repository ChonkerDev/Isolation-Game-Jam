using UnityEngine;

namespace Chonker.Scripts.Player {
    public class PlatformerPlayerState : MonoBehaviour {

        public bool DoubleJumpUnlocked;
        public int MaxNumberOfJumps => DoubleJumpUnlocked ? 2 : 1;

        public int NumJumpsAvailable { get; private set; }

        private void Start() {
            NumJumpsAvailable = MaxNumberOfJumps;
        }

        public void ResetNumJumps() {
            NumJumpsAvailable = MaxNumberOfJumps;
        }

        public void DecrementNumJumps() {
            NumJumpsAvailable--;
        }
    }
}