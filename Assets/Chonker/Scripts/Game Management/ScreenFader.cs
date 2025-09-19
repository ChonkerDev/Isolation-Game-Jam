using System;
using Chonker.Core;
using Chonker.Core.Tween;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour {
    public static ScreenFader instance;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image image;

    private void Awake() {
        if (!instance) {
            instance = this;
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void FadeIn(Color color, float duration, Action onComplete = null, EaseType easeType = EaseType.Linear) {
        image.color = color;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        StartCoroutine(TweenCoroutines.RunTaperRealTime(duration,
            a => {
                canvasGroup.alpha = 1 - a;
            },
            () => {
                onComplete?.Invoke();
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
            }, easeType));
    }

    public void FadeOut(Color color, float duration, Action onComplete = null, EaseType easeType = EaseType.Linear) {
        image.color = color;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 0;
        StartCoroutine(TweenCoroutines.RunTaperRealTime(duration, 
            a => {
            canvasGroup.alpha = a;
        }, () => {
            onComplete?.Invoke();
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = false;
        }, easeType));
    }
}