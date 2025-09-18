using System;
using System.Linq;
using Chonker.Scripts.Player;
using UnityEngine;

namespace Chonker.Scripts.Game_Management {
    public class LevelManager : MonoBehaviour {
        private LevelResettable[] resettableLevelParts;
        public static LevelManager instance { get; private set; }
        public static PlatformerPlayerComponentContainer PlayerInstance;

        public LevelCheckpoint CurrentCheckPoint;

        [SerializeField] private bool DebugReset;

        private void Awake() {
            instance = this;
        }

        private void Start() {
            resettableLevelParts = FindObjectsByType<LevelResettable>(FindObjectsSortMode.None).ToArray();
        }

        public void ResetLevel() {
            foreach (var instanceResettableLevelPart in instance.resettableLevelParts) {
                instanceResettableLevelPart.Reset();
            }
            Vector2 resetPosition = CurrentCheckPoint ? CurrentCheckPoint.transform.position : transform.position;
            PlayerInstance.ResetCharacter(resetPosition);
        }

        private void OnValidate() {
            if (DebugReset) {
                DebugReset = false;
                ResetLevel();
            }
        }

        private void OnDestroy() {
            instance = null;
            PlayerInstance = null;
        }
    }
}