using UnityEngine;
using System.Collections;

public class PerverseIdolEvent : MonoBehaviour
{
    public AudioClip[] musicTracks;
    public float maxDistance = 30f; // Maximum distance for hearing the music
    public float minInterval = 60f; // Minimum interval between music tracks

    public AudioSource musicSource; // Expose AudioSource field for manual assignment in the editor
    public GameObject idolObject; // Reference to the perverse idol GameObject

    private bool isPlayerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            PlayMusic();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            HideIdol();
            StopMusic(); // Stop the music when the player exits the trigger
        }
    }

    private void Update()
    {
        if (isPlayerInside && !IsPlayerLookingAtIdol())
        {
            HideIdol();
            StopMusic(); // Stop the music when the player is inside the trigger but not looking at the idol
        }
    }

    private bool IsPlayerLookingAtIdol()
    {
        if (idolObject == null)
            return false;

        Vector3 directionToIdol = idolObject.transform.position - Camera.main.transform.position;
        float angle = Vector3.Angle(Camera.main.transform.forward, directionToIdol);

        // Check if the angle between the player's forward direction and the direction to the idol is within a certain threshold
        return angle < 55f; // Adjust the threshold as needed
    }

    private void PlayMusic()
    {
        if (musicSource == null || musicTracks.Length == 0)
            return;

        if (!musicSource.isPlaying)
        {
            AudioClip musicTrack = musicTracks[Random.Range(0, musicTracks.Length)];
            musicSource.clip = musicTrack;
            musicSource.Play();
        }
    }

    private void HideIdol()
    {
        if (idolObject != null)
            idolObject.SetActive(false);
    }

    private void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}

