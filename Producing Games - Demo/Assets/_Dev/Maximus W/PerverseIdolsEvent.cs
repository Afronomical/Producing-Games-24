using UnityEngine;
using System.Collections;

public class PerverseIdolsEvent : MonoBehaviour
{
    public GameObject perverseIdolsObject;
    public float duration = 20f; // Duration of the perverse idols event
    public AudioClip idolMusic; // Audio clip for the idol music
    public float musicVolume = 0.5f; // Volume level for the music
    public float playerDistanceThreshold = 5f; // Distance threshold for playing music

    private AudioSource audioSource;
    private bool isPlayerNearby = false;
    private Transform playerTransform;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = idolMusic;
        audioSource.volume = musicVolume;
        audioSource.loop = true; // Loop the music

        // Find the player's transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Trigger the event coroutine
        TriggerPerverseIdols();
    }

    private void Update()
    {
        // Check if the player is nearby and within the distance threshold, then play music accordingly
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (isPlayerNearby && distanceToPlayer <= playerDistanceThreshold)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    private void TriggerPerverseIdols()
    {
        StartCoroutine(PerverseIdolsCoroutine());
    }

    private IEnumerator PerverseIdolsCoroutine()
    {
        perverseIdolsObject.SetActive(true);
        // Add any additional logic or animation for the perverse idols event

        yield return new WaitForSeconds(duration);

        perverseIdolsObject.SetActive(false);
        // Additional logic to reset or clean up after the perverse idols event
    }
}
