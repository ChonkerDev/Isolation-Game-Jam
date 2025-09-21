using System;
using System.Collections;
using UnityEngine;

public class AudioSourceFader : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float _fadeTime;
    private bool faded;
    private void OnTriggerEnter2D(Collider2D other) {
        if (faded) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        faded = true;
        StartCoroutine(FadeOutI(_fadeTime));

    }
    
    private IEnumerator FadeOutI(float fadeTime) {
        float timer = 0;
        while (timer > 0) {
            timer -= Time.deltaTime/fadeTime;
            audioSource.volume = timer;
            yield return null;
        }
        
        audioSource.volume = 0;
    }
    
    
}
