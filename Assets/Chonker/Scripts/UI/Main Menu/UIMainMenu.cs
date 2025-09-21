using System;
using System.Collections;
using Chonker.Core;
using Chonker.Core.Tween;
using Chonker.Scripts.Game_Management;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;


public class UIMainMenu : NavigationUIMenu {
    public AnimationCurve MainMenuPopInScaleCurve;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _settingsButton;
    
    [SerializeField] private OptionsMenu _optionsMenu;
    [SerializeField] private AudioSource _selectableChangedSoundSource;
    [SerializeField] private AudioSource _selectionConfirmedSoundSource;
    [SerializeField] private AudioSource _whiteFadeSoundSource;
    void Start() {
        Deactivate();
        OnCurrentSelectionChanged += OnCurrentSelectionChangedEvent;
        _newGameButton.onClick.AddListener(() => {
            ClearCurrentInteractable();
            _selectionConfirmedSoundSource.Play();
            _whiteFadeSoundSource.Play();
            PersistantDataManager.instance.SetLevelCollectedAllFlowers(SceneManagerWrapper.CurrentSceneId, false);
            PersistantDataManager.instance.SetLevelCollectedAllFlowers(SceneManagerWrapper.CurrentSceneId, false);

            ScreenFader.instance.FadeOut(Color.white, 2, () => SceneManagerWrapper.LoadScene(SceneManagerWrapper.SceneId.Level1),
                EaseType.EaseOutQuad);
        });
        /*_continueButton.onClick.AddListener(() => {
            ClearCurrentInteractable();
            _selectionConfirmedSoundSource.Play();
            _whiteFadeSoundSource.Play();
            ScreenFader.instance.FadeOut(Color.white, 2,
                () => SceneManagerWrapper.LoadScene(PersistantDataManager.instance.GetCampaignProgress()),
                EaseType.EaseOutQuad);
        });*/
        _settingsButton.onClick.AddListener(() => {
            _selectionConfirmedSoundSource.Play();
            _optionsMenu.Activate();
        });

        /*if (PersistantDataManager.instance.GetCampaignProgress() == SceneManagerWrapper.SceneId.Level1) {
            _continueButton.interactable = false;
            Navigation newGameNav = _newGameButton.navigation;
            newGameNav.selectOnDown = _settingsButton;
            _newGameButton.navigation = newGameNav;
            Navigation settingsNav = _settingsButton.navigation;
            settingsNav.selectOnUp = _newGameButton;
            _settingsButton.navigation = settingsNav;
        }*/
    }

    private void OnCurrentSelectionChangedEvent(GameObject prev, GameObject current) {
        if (current == null) return;
        _selectableChangedSoundSource.Play();
    }
}
