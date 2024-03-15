using System.Collections;
using System.Collections.Generic;
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
            AudioManager.instance.PlaySound(ambientSoundEffect, transform);
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

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeOutDuration);
        AudioManager.instance.StopSound(ambientSoundEffect);
    }
}