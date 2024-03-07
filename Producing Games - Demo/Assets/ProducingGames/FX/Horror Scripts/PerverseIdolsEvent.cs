using UnityEngine;

public class PerverseIdolEvent : MonoBehaviour
{
    public AudioClip[] musicTracks;
    public float maxDistanceSquared = 900f; // Maximum distance squared for hearing the music
    public float minInterval = 60f; // Minimum interval between music tracks

    public AudioSource musicSource; // Expose AudioSource field for manual assignment in the editor
    public GameObject idolObject; // Reference to the perverse idol GameObject

    private bool isPlayerInside = false;
    private GameManager gM;
    private bool eventTriggered;
    private Transform playerCameraTransform;

    private void Start()
    {
        gM = GameManager.Instance;
        playerCameraTransform = Camera.main.transform;
    }

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
        if (isPlayerInside && !IsPlayerLookingAtIdol() && Random.Range(0, 101) <= gM.eventChance && !eventTriggered)
        {
            HideIdol();
            StopMusic(); // Stop the music when the player is inside the trigger but not looking at the idol
            eventTriggered = true;
        }
    }

    private bool IsPlayerLookingAtIdol()
    {
        if (idolObject == null)
            return false;

        Vector3 directionToIdol = idolObject.transform.position - playerCameraTransform.position;
        Vector3 forwardDirection = playerCameraTransform.forward;

        // Optimized angle calculation
        float dotProduct = Vector3.Dot(directionToIdol.normalized, forwardDirection);
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

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
            musicSource.Stop();
    }
}

