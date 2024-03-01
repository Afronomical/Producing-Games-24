using UnityEngine;
using System.Collections;

public class DistantScreamsEvent : MonoBehaviour
{
    public AudioClip[] screamSounds;
    public float maxDistance = 30f; // Maximum distance for hearing the distant screams
    public float minInterval = 60f; // Minimum interval between distant screams

    public AudioSource audioSource; // Expose AudioSource field for manual assignment in the editor

    private float lastScreamTime;
    private GameObject player;

    private void Start()
    {
        lastScreamTime = Time.time - minInterval; // Initialize lastScreamTime to ensure immediate playback

        // Set the 'player' reference to the actual player GameObject
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("Audio source not assigned. Please assign an audio source in the editor.");
            return;
        }

        TriggerDistantScreams();
    }

    public void TriggerDistantScreams()
    {
        StartCoroutine(DistantScreamsCoroutine());
    }

    private IEnumerator DistantScreamsCoroutine()
    {
        while (true)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= maxDistance && Time.time - lastScreamTime >= minInterval)
            {
                lastScreamTime = Time.time;

                // Choose a random scream sound
                AudioClip screamSound = screamSounds[Random.Range(0, screamSounds.Length)];

                // Play the scream sound through the assigned audio source
                audioSource.PlayOneShot(screamSound);
            }

            yield return null;
        }
    }
}
