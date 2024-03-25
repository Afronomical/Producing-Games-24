using System.Collections;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    [SerializeField] private SoundEffect ambientSoundEffect; // The SoundEffect for the ambient sound
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
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / fadeInDuration;
            float volumeMultiplier = Mathf.Lerp(0f, 1f, normalizedTime);
            AudioManager.instance.musicVolume = volumeMultiplier;
            AudioManager.instance.soundEffectVolume = volumeMultiplier;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / fadeOutDuration;
            float volumeMultiplier = Mathf.Lerp(1f, 0f, normalizedTime);
            AudioManager.instance.musicVolume = volumeMultiplier;
            AudioManager.instance.soundEffectVolume = volumeMultiplier;
            yield return null;
        }
    }
}