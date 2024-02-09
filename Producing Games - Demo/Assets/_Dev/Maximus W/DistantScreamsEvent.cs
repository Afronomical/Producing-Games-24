using UnityEngine;
using System.Collections;

public class DistantScreamsEvent : MonoBehaviour
{
    public AudioClip[] screamSounds;
    public float maxDistance = 30f; // Maximum distance for hearing the distant screams
    public float minInterval = 60f; // Minimum interval between distant screams

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

                // Play the scream sound from a random direction around the player
                AudioSource.PlayClipAtPoint(screamSound, GetRandomPositionAroundPlayer(), 1.0f);
            }

            yield return null;
        }
    }

    private Vector3 GetRandomPositionAroundPlayer()
    {
        // Implement logic to get a random position around the player
        // This can be achieved by adding a random offset to the player's position
        return player.transform.position;
    }
}