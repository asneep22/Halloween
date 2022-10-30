using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioPlayer
{
    public static void PlayRandom(Transform target, AudioSource audioSource, List<AudioClip> audioClips)
    {
        audioSource.transform.position = target.position;
        AudioClip audioClip = audioClips[Random.Range(0, audioClips.Count)];
        audioSource.PlayOneShot(audioClip);
    }
}
