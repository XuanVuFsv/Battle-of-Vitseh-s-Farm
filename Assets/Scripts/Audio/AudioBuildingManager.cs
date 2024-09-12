using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBuildingManager : Singleton<AudioBuildingManager>
{
    public AudioClip activeClip;
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void StopPlayAudio()
    {
        audioSource.Stop();
    }
}