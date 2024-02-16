using UnityEngine;
using System.Collections;

public class PerverseIdolEvent : MonoBehaviour
{
    public GameObject idolObject; // Reference to the idol object
    public float maxDistance = 30f; // Maximum distance for hearing the distant screams
    public float minInterval = 60f; // Minimum interval between distant screams

    public AudioClip[] screamSounds;
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
            float dotProduct = Vector3.Dot(player.transform.forward, (transform.position - player.transform.position).normalized);

            if (distanceToPlayer <= maxDistance && Time.time - lastScreamTime >= minInterval && dotProduct > 0)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            idolObject.SetActive(false); // Deactivate the idol object when player enters
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            idolObject.SetActive(true); // Reactivate the idol object when player exits
        }
    }
}

