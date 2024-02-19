using UnityEngine;

public class HidingScare : MonoBehaviour
{
    public float jumpScareChance = 5f; // Adjust this value to change the chance of jump scare (in percentage)
    public GameObject jumpScareObject; // Reference to the object responsible for the jump scare (e.g., a scary image, sound, etc.)

    private bool playerIsHiding = false;

    void Update()
    {
        if (playerIsHiding)
        {
            // Check if a jump scare should occur
            if (Random.Range(0f, 100f) < jumpScareChance)
            {
                // Trigger jump scare
                TriggerJumpScare();
            }
        }
    }

    void TriggerJumpScare()
    {
        // Activate the jump scare object
        jumpScareObject.SetActive(true);
    }

    // Method to set whether the player is hiding
    public void SetPlayerIsHiding(bool isHiding)
    {
        playerIsHiding = isHiding;
    }
}
