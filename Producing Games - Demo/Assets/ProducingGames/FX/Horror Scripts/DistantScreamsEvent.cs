using UnityEngine;
using System.Collections;

public class DistantScreamsEvent : MonoBehaviour
{
    public SoundEffect[] screamSounds;
    public float minInterval = 60f; // Minimum interval between distant screams

    private GameObject player;

    private float lastScreamTime;
    private GameManager gM;
    [HideInInspector] public bool eventTriggered;

    private void Start()
    {
        lastScreamTime = Time.time - minInterval; // Initialize lastScreamTime to ensure immediate playback

        player = GameManager.Instance.player;
        gM = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Time.time - lastScreamTime >= minInterval && !eventTriggered)
        {
            lastScreamTime = Time.time;

            // Choose a random scream sound
            SoundEffect screamSound = screamSounds[Random.Range(0, screamSounds.Length)];

            // Play the scream sound through the assigned audio source
            AudioManager.instance.PlaySound(screamSound, player.transform);

            eventTriggered = true;
        }
    }
}