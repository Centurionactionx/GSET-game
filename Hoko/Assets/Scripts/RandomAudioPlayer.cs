using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomAudioPlayer : MonoBehaviour
{
    public List<AudioClip> audioClips;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomClip()
    {
        if (audioClips == null || audioClips.Count == 0)
        {
            Debug.LogWarning("No audio clips assigned to RandomAudioPlayer.");
            return;
        }

        int randomIndex = Random.Range(0, audioClips.Count);
        AudioClip clipToPlay = audioClips[randomIndex];
        audioSource.PlayOneShot(clipToPlay);
    }
}
