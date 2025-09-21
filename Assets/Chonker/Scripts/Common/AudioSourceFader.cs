using System;
using System.Collections;
using UnityEngine;

public class AudioSourceFader : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void FadeOut(float fadeTime) {
        StartCoroutine(FadeOutI(fadeTime));
    }

    public IEnumerator FadeOutI(float fadeTime) {
        float timer = 0;
        while (timer > 0) {
            timer -= Time.deltaTime/fadeTime;
            audioSource.volume = timer;
            yield return null;
        }
        
        audioSource.volume = 0;
    }
    
    
}
