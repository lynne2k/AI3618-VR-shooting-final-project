using System.Collections;
using UnityEngine;

public class RandomAudioPlayer : MonoBehaviour
{
    public AudioClip[] audioClips; 
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        StartCoroutine(PlayRandomAudio());
    }

    private IEnumerator PlayRandomAudio()
    {
        yield return new WaitForSeconds(15f);
        while (true)
        {
            AudioClip clipToPlay = audioClips[Random.Range(0, audioClips.Length)];

            audioSource.PlayOneShot(clipToPlay);

            yield return new WaitForSeconds(8f);
        }
    }
}
