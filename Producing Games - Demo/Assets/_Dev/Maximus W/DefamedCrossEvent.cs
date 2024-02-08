using UnityEngine;

public class DefamedCrossEvent : MonoBehaviour
{
    public GameObject[] crosses; // Array of crosses spread across the level
    public AudioClip[] crossAudio; // Audio clips for cross events
    public float chanceOfEvent = 0.2f; // Chance of cross event triggering
    public float sanityPenalty = 10f; // Amount of sanity penalty when the event triggers

    private GameManager gameManager;

    private void Start()
    {
        //gameManager = GameManager.instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager instance not found.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Randomly determine if the event should trigger
            if (Random.value <= chanceOfEvent)
            {
                TriggerCrossEvent();
            }
        }
    }

    private void TriggerCrossEvent()
    {
        // Choose a random cross from the array
        GameObject randomCross = crosses[Random.Range(0, crosses.Length)];

        // Perform the event action (invert or throw the cross)
        if (Random.value < 0.5f) // 50% chance of inverting the cross
        {
            InvertCross(randomCross);
        }
        else // 50% chance of throwing the cross
        {
            ThrowCross(randomCross);
        }

        // Play a random audio clip for the event
        if (crossAudio.Length > 0)
        {
            AudioClip randomClip = crossAudio[Random.Range(0, crossAudio.Length)];
            AudioSource.PlayClipAtPoint(randomClip, randomCross.transform.position);
        }

        // Apply sanity penalty to the player through the GameManager
        if (gameManager != null)
        {
            //gameManager.DecreaseSanity(sanityPenalty);
        }
    }

    private void InvertCross(GameObject cross)
    {
        // Implement logic to invert the cross
        cross.transform.Rotate(Vector3.forward, 180f);
    }

    private void ThrowCross(GameObject cross)
    {
        // Implement logic to throw the cross from its peg
        Rigidbody rb = cross.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
}
