using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    public void PlayAudioClip(int clipIndex, bool loop = false)
    {
        if (clipIndex >= 0 && clipIndex < audioClips.Length)
        {
            audioSource.clip = audioClips[clipIndex];
            audioSource.loop = loop;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip index out of range.");
        }
    }
}
