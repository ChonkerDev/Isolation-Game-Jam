using UnityEngine;

namespace Chonker.Scripts.Player {
    public class PlatformerPlayerComponentContainer : MonoBehaviour {
        [Header("Component References")]
        public InputMovementWrapper InputMovementWrapper;
        public PlatformerPlayerCamera PlatformerPlayerCamera;
        public PlatformerCharacterController PlatformerCharacterController;
        public PlatformerPlayerState PlatformerPlayerState;
        public PlatformerPlayerForceFieldDetector PlatformerPlayerForceFieldDetector;
        public PlatformerPlayerAnimationManager platformerPlayerAnimationManager;
        public PlatformerPlayerAnimationConfig PlatformerPlayerAnimationConfig;
        public PlatformerSurfaceCheck PlatformerSurfaceCheck;
        public PlatformerPlayerDeathBoxDetector PlatformerPlayerDeathBoxDetector;
        public PlatformerPlayerMoveablePlatformCheck PlatformerPlayerMoveablePlatformCheck;
        [Header("Config")]
        public PlatformerPlayerPhysicsConfigSO PhysicsConfigSO;
    }
}