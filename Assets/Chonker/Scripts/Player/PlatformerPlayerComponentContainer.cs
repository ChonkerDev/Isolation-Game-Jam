using UnityEngine;

namespace Chonker.Scripts.Player {
    public class PlatformerPlayerComponentContainer : MonoBehaviour {
        public InputMovementWrapper InputMovementWrapper;
        public PlatformerPlayerCamera PlatformerPlayerCamera;
        public PlatformerCharacterController PlatformerCharacterController;
        public PlatformerPlayerPhysicsConfig PlatformerPlayerPhysicsConfig;
        public PlatformerPlayerState PlatformerPlayerState;
        public PlatformerPlayerForceFieldDetector PlatformerPlayerForceFieldDetector;
    }
}