using UnityEngine;

public class JumpScare : MonoBehaviour
{
    public GameObject redCubePrefab;
    public int numberOfCubes = 10;
    public float minForce = 5f;
    public float maxForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Trigger the jump scare effect
            TriggerJumpScare();
        }
    }

    private void TriggerJumpScare()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            // Instantiate a red cube at a random position within the trigger area
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * 2f;
            GameObject redCube = Instantiate(redCubePrefab, randomPosition, Quaternion.identity);

            // Apply a random force to the red cube
            Rigidbody rb = redCube.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 randomForce = Random.onUnitSphere * Random.Range(minForce, maxForce);
                rb.AddForce(randomForce, ForceMode.Impulse);
            }
        }
    }
}

