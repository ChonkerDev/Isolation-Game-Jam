using System.Collections;
using Chonker.Scripts.Game_Management;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class OptionsMenu : NavigationUIMenu {
    [SerializeField] private AudioSource _audioSourceSliderSFX;
    [SerializeField] private Slider MasterVolumeSlider;
    [SerializeField] private Slider MusicVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private Button exitButton;


    protected override void OnAwake() {
    }

    private void Start() {
        MasterVolumeSlider.value = PersistantDataManager.instance.GetMasterVol();
        MusicVolumeSlider.value = PersistantDataManager.instance.GetMusicVol();
        SFXVolumeSlider.value = PersistantDataManager.instance.GetSFXVol();

        setAudioMixerVolume("MasterVol", PersistantDataManager.instance.GetMasterVol());
        setAudioMixerVolume("MusicVol", PersistantDataManager.instance.GetMusicVol());
        setAudioMixerVolume("SFXVol", PersistantDataManager.instance.GetSFXVol());

        MasterVolumeSlider.onValueChanged.AddListener((f) => {
            PersistantDataManager.instance.StoreMasterVol(f);
            setAudioMixerVolume("MasterVol", f);
            PersistantDataManager.instance.PersistData();
        });
        MusicVolumeSlider.onValueChanged.AddListener((f) => {
            PersistantDataManager.instance.StoreMusicVol(f);
            setAudioMixerVolume("MusicVol", f);
            PersistantDataManager.instance.PersistData();
        });
        SFXVolumeSlider.onValueChanged.AddListener((f) => {
            PersistantDataManager.instance.StoreSFXVol(f);
            setAudioMixerVolume("SFXVol", f);
            PersistantDataManager.instance.PersistData();
            _audioSourceSliderSFX.Play();
        });

        exitButton.onClick.AddListener(Deactivate);

        Deactivate();
    }

    private bool exitMenu = false;

    void OnEnable() {
        base.OnEnable();
        EventSystem.current.GetComponent<InputSystemUIInputModule>().cancel.action.performed+= context => {
            exitMenu = true;
        } ;
    }

    private void setAudioMixerVolume(string name, float val) {
        float scaledValue = Mathf.Max(.0001f, val);
        scaledValue = Mathf.Log10(scaledValue) * 20;
        PersistantDataManager.instance.AudioMixer.SetFloat(name, scaledValue);
    }

    protected override void processCurrentMenu() {
        
        if (exitMenu) {
            exitMenu = false;
            StartCoroutine(DelayExit());
        }
    }

    private IEnumerator DelayExit() {
        yield return null; // eat a frame so input doesn't carry over to next menu
        Deactivate();
    }
}