using UnityEngine;
using System.Collections;

public class DistantScreamsEvent : MonoBehaviour
{
    public AudioClip[] screamSounds;
    public float maxDistanceSquared = 900f; // Maximum distance squared for hearing the distant screams
    public float minInterval = 60f; // Minimum interval between distant screams

    public AudioSource audioSource; // Expose AudioSource field for manual assignment in the editor

    private float lastScreamTime;
    private Transform playerTransform;
    private GameManager gM;
    private bool eventTriggered;

    private void Start()
    {
        lastScreamTime = Time.time - minInterval; // Initialize lastScreamTime to ensure immediate playback

        // Set the 'playerTransform' reference to the actual player transform
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
            return;
        }
        playerTransform = player.transform;

        if (audioSource == null)
        {
            Debug.LogError("Audio source not assigned. Please assign an audio source in the editor.");
            return;
        }

        gM = GameManager.Instance;

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
            float sqrDistanceToPlayer = (transform.position - playerTransform.position).sqrMagnitude;
            int randChance = Random.Range(0, 101);

            if (sqrDistanceToPlayer <= maxDistanceSquared && Time.time - lastScreamTime >= minInterval && randChance <= gM.eventChance && !eventTriggered)
            {
                lastScreamTime = Time.time;

                // Choose a random scream sound
                AudioClip screamSound = screamSounds[Random.Range(0, screamSounds.Length)];

                // Play the scream sound through the assigned audio source
                audioSource.PlayOneShot(screamSound);

                eventTriggered = true;
            }

            yield return null;
        }
    }
}

