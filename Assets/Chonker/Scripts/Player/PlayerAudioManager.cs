using System;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip FootStepClip;
    [SerializeField] private AudioClip DashClip;
    [SerializeField] private AudioSource WallSlideAudioSource;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShotFootStep() {
        audioSource.PlayOneShot(FootStepClip);
    }

    public void PlayDash() {
        audioSource.PlayOneShot(DashClip);
    }

    public void PlayWallSlideLoop() {
        WallSlideAudioSource.Play();
    }

    public void StopLoop() {
        audioSource.Stop();
    }
}
