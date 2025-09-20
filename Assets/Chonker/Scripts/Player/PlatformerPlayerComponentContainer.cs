using System;
using System.Collections;
using Chonker.Scripts.Game_Management;
using Chonker.Scripts.Player.States;
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
        public PlatformerPlayerMovementStateManager PlatformerPlayerMovementStateManager;
        public PlayerAudioManager PlayerAudioManager;

        [Header("Config")]
        public PlatformerPlayerPhysicsConfigSO PhysicsConfigSO;

        private void Awake() {
            LevelManager.PlayerInstance = this;
        }

        public void ResetCharacter(Vector2 position) {
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, 10, LayerMask.GetMask("Obstacle"));
            if (hit.transform) {
                position = hit.point;
                position.y += PlatformerCharacterController.BoxSize.y / 2;
            }
            PlatformerCharacterController.Teleport(position);
            GroundMovementState groundState =
                (GroundMovementState) PlatformerPlayerMovementStateManager.GetState(PlatformerPlayerMovementStateId.Ground);
            PlatformerPlayerMovementStateManager.UpdateState(PlatformerPlayerMovementStateId.Ground);

        }
    }
}