using System;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip FootStepClip;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShotFootStep() {
        audioSource.PlayOneShot(FootStepClip);
    }
}
