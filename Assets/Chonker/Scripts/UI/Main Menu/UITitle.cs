using System.Collections;
using Chonker.Core.Tween;
using UnityEngine;
using UnityEngine.InputSystem;

public class UITitle : NavigationUIMenu {
    [SerializeField] private UIMainMenu _uIMainMenu;
[SerializeField] private AudioSource _audioSource;
    IEnumerator Start() {
        Time.timeScale = 1;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        while (Keyboard.current == null || !Keyboard.current.anyKey.wasPressedThisFrame) {
            yield return null;
        }

        _audioSource.Play();
        StartCoroutine(TweenCoroutines.RunTaper(.5f,
            f => { canvasGroup.alpha = 1 - f; },
            () => {
                _uIMainMenu.Activate();
                Deactivate();
                StartCoroutine(TweenCoroutines.RunAnimationCurveTaper(.3f, _uIMainMenu.MainMenuPopInScaleCurve,
                    curveAlpha => { _uIMainMenu.RectTransform.localScale = Vector3.one * curveAlpha; } ));
            }));

    }


    
}
