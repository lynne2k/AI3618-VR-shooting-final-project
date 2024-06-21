using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAudioPlayer: MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;

    void Start()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (audioClips.Length > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Length);
            AudioClip clipToPlay = audioClips[randomIndex];

            audioSource.clip = clipToPlay;
            audioSource.Play();
        }
    }
}
