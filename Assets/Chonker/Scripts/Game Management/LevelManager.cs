using System;
using System.Collections;
using System.Linq;
using Chonker.Core;
using Chonker.Scripts.Player;
using TMPro;
using UnityEngine;

namespace Chonker.Scripts.Game_Management {
    public class LevelManager : MonoBehaviour {
        private LevelResettable[] resettableLevelParts;
        public static LevelManager instance { get; private set; }
        public static PlatformerPlayerComponentContainer PlayerInstance;

        public LevelCheckpoint CurrentCheckPoint;

        [SerializeField] private bool DebugReset;
        [SerializeField] private bool SkipPlayPositionSetOnStart;

        private CollectableFlower[] flowers;

        public int NumCollectedFlowers;
        public int TotalFlowerCount => flowers.Length;

        [SerializeField] private TextMeshProUGUI _collectedAllFlowersText;
        [SerializeField] private AudioSource _collectedAllFlowersAudioSource;
        [SerializeField] private AudioSource _finishedLevelAudioSource;

        private void Awake() {
            instance = this;
        }

        private IEnumerator Start() {
            resettableLevelParts = FindObjectsByType<LevelResettable>(FindObjectsSortMode.None).ToArray();
            flowers = FindObjectsByType<CollectableFlower>(FindObjectsSortMode.None);
            PersistantDataManager.instance.SetLevelCollectedAllFlowers(SceneManagerWrapper.CurrentSceneId, false);
            _collectedAllFlowersText.gameObject.SetActive(false);
            PersistantDataManager.instance.SetCampaignProgress(SceneManagerWrapper.CurrentSceneId);
            PlayerInstance.PlatformerPlayerState.LockOmniDash();
            yield return null;
            if (!SkipPlayPositionSetOnStart) {
                ResetLevel();
            }
            ScreenFader.instance.FadeOut(Color.white, 0);
            yield return new WaitForSeconds(1f);
            ScreenFader.instance.FadeIn(Color.white, 2);
        }

        public void ResetLevel() {
            foreach (var instanceResettableLevelPart in instance.resettableLevelParts) {
                instanceResettableLevelPart.Reset();
            }

            Vector2 resetPosition = CurrentCheckPoint ? CurrentCheckPoint.transform.position : transform.position;
            PlayerInstance.ResetCharacter(resetPosition);
        }

        public void TransitionToNextLevel() {
            _finishedLevelAudioSource.Play();
            if (instance.NumCollectedFlowers >= instance.TotalFlowerCount) {
                _collectedAllFlowersText.gameObject.SetActive(true);
                PersistantDataManager.instance.SetLevelCollectedAllFlowers(SceneManagerWrapper.CurrentSceneId, true);
                _collectedAllFlowersAudioSource.Play();
            }
            float transitionDuration = 4;

            ScreenFader.instance.FadeOut(Color.white, transitionDuration, () => {
                if (SceneManagerWrapper.CurrentSceneId == SceneManagerWrapper.SceneId.Level1) {
                    SceneManagerWrapper.LoadScene(SceneManagerWrapper.SceneId.Level2);
                }
                else if (SceneManagerWrapper.CurrentSceneId == SceneManagerWrapper.SceneId.Level2) {
                    SceneManagerWrapper.LoadScene(SceneManagerWrapper.SceneId.Level3);
                }
            }, EaseType.EaseOutQuad);
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