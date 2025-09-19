using UnityEngine;
using UnityEngine.Audio;

namespace Chonker.Scripts.Game_Management {
    public class PersistantDataManager : MonoBehaviour {
        public static PersistantDataManager instance;
        private const string MASTER_AUDIO = "MASTER_AUDIO";
        private const string MUSIC_AUDIO = "MUSIC_AUDIO";
        private const string SFX_AUDIO = "SFX_AUDIO";
        private const string CAMPAIGN_PROGRESS = "CAMPAIGN_PROGRESS";
        private const string COLLECTED_FLOWERS_SUFFIX = "_COLLECTED_FLOWERS";

        [SerializeField] private AudioMixer _audioMixer;
        public AudioMixer AudioMixer => _audioMixer;

        private void Awake() {
            if (!instance) {
                instance = this;
                bool defaultsNotSet = false;
                if (!PlayerPrefs.HasKey(MASTER_AUDIO)) {
                    PlayerPrefs.SetFloat(MASTER_AUDIO, .5f);
                    defaultsNotSet = true;
                }

                if (!PlayerPrefs.HasKey(MUSIC_AUDIO)) {
                    PlayerPrefs.SetFloat(MUSIC_AUDIO, .5f);
                    defaultsNotSet = true;
                }

                if (!PlayerPrefs.HasKey(SFX_AUDIO)) {
                    PlayerPrefs.SetFloat(SFX_AUDIO, .5f);
                    defaultsNotSet = true;
                }

                if (!PlayerPrefs.HasKey(CAMPAIGN_PROGRESS)) {
                    SetCampaignProgress(SceneManagerWrapper.SceneId.Level1);
                    defaultsNotSet = true;
                }

                if (defaultsNotSet) {
                    PersistData();
                }
            }
        }

        public void PersistData() {
            PlayerPrefs.Save();
        }

        public void StoreMasterVol(float val) {
            PlayerPrefs.SetFloat(MASTER_AUDIO, val);
        }

        public void StoreMusicVol(float val) {
            PlayerPrefs.SetFloat(MUSIC_AUDIO, val);
        }

        public void StoreSFXVol(float val) {
            PlayerPrefs.SetFloat(SFX_AUDIO, val);
        }

        public float GetMasterVol() {
            return PlayerPrefs.GetFloat(MASTER_AUDIO);
        }

        public float GetMusicVol() {
            return PlayerPrefs.GetFloat(MUSIC_AUDIO);
        }

        public float GetSFXVol() {
            return PlayerPrefs.GetFloat(SFX_AUDIO);
        }

        public void SetCampaignProgress(SceneManagerWrapper.SceneId sceneId) {
            PlayerPrefs.SetInt(CAMPAIGN_PROGRESS, (int)sceneId);
            PersistData();
        }

        public void SetLevelCollectedAllFlowers(SceneManagerWrapper.SceneId sceneId, bool collectedAll) {
            PlayerPrefs.SetInt(sceneId + COLLECTED_FLOWERS_SUFFIX, collectedAll ? 1 : 0);
        }

        public bool GetCollectedAllFlowers() {
            bool level1Collected =
                PlayerPrefs.GetInt(SceneManagerWrapper.SceneId.Level1 + COLLECTED_FLOWERS_SUFFIX) == 1;
            bool level2Collected =
                PlayerPrefs.GetInt(SceneManagerWrapper.SceneId.Level2 + COLLECTED_FLOWERS_SUFFIX) == 1;
            Debug.Log("Levels Collected: " + level1Collected + " |  " + level2Collected);
            return level1Collected && level2Collected;
        }

        public SceneManagerWrapper.SceneId GetCampaignProgress() {
            return (SceneManagerWrapper.SceneId)PlayerPrefs.GetInt(CAMPAIGN_PROGRESS);
        }
    }
}