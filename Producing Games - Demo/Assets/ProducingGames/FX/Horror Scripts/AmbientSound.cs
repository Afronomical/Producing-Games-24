using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // The AudioSource component to play the sound
    [SerializeField] private AudioClip ambientClip; // The ambient sound clip to play
    [SerializeField, Range(0f, 1f)] private float volume = 1f; // Volume of the sound
    [SerializeField, Range(0.1f, 3f)] private float pitch = 1f; // Pitch of the sound
    [SerializeField] private bool loop = true; // Whether the sound should loop
    [SerializeField, Range(0f, 1f)] private float spatialBlend = 1f; // Spatial blend of the sound
    [SerializeField, Range(1f, 1000f)] private float minDistance = 1f; // Minimum distance of the sound
    [SerializeField, Range(1f, 1000f)] private float maxDistance = 100f; // Maximum distance of the sound
    [SerializeField] private AudioRolloffMode rolloffMode = AudioRolloffMode.Linear; // Rolloff mode of the sound
    [SerializeField, Range(0.1f, 5f)] private float fadeInDuration = 1f; // Fade in duration
    [SerializeField, Range(0.1f, 5f)] private float fadeOutDuration = 1f; // Fade out duration

    private bool isPlaying = false; // Flag to check if the sound is currently playing

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPlaying)
        {
            isPlaying = true;
            StartCoroutine(FadeIn());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPlaying)
        {
            isPlaying = false;
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        audioSource.clip = ambientClip;
        audioSource.volume = 0f;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
        audioSource.spatialBlend = spatialBlend;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.rolloffMode = rolloffMode;

        audioSource.Play();

        float timer = 0f;
        while (timer < fadeInDuration)
        {
            // Use a smoother interpolation function like quadratic or cubic
            float t = timer / fadeInDuration;
            audioSource.volume = Mathf.Lerp(0f, volume, t * t); // Quadratic interpolation
            timer += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = volume;
    }

    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        float timer = 0f;
        while (timer < fadeOutDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeOutDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        audioSource.Stop();
    }
}
