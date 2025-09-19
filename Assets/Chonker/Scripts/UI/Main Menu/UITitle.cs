using System.Collections;
using System.Linq;
using Chonker.Core.Tween;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class UITitle : NavigationUIMenu {
    [SerializeField] private UIMainMenu _uIMainMenu;
    [SerializeField] private AudioSource _audioSource;

    IEnumerator Start() {
        Time.timeScale = 1;
        float fadeInTime = 2;
        ScreenFader.instance.FadeIn(Color.white, 2);
        yield return new WaitForSeconds(2);
        while (true) {
            if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) {
                break;
            }

            if (Gamepad.current != null &&
                Gamepad.current.allControls.Any(c => c is ButtonControl { wasPressedThisFrame: true })) {
                break;
            }

            if (Mouse.current != null && Mouse.current.leftButton.isPressed) {
                break;
            }

            yield return null;
        }

        _audioSource.Play();
        StartCoroutine(TweenCoroutines.RunTaper(.5f,
            f => { canvasGroup.alpha = 1 - f; },
            () => {
                _uIMainMenu.Activate();
                Deactivate();
                StartCoroutine(TweenCoroutines.RunAnimationCurveTaper(.3f, _uIMainMenu.MainMenuPopInScaleCurve,
                    curveAlpha => { _uIMainMenu.RectTransform.localScale = Vector3.one * curveAlpha; }));
            }));
    }
}