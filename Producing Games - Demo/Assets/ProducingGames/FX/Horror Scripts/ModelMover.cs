using UnityEngine;

public class ModelMover : MonoBehaviour
{
    public GameObject targetObject; // Target GameObject whose position will be the target position
    public float movementSpeed = 1.0f; // Speed of movement

    private Vector3 initialPosition; // Initial position of the object
    private bool isMoving = false; // Flag to check if the object is currently moving
    private bool soundPlayed = false; // Flag to check if the sound has been played

    public SoundEffect movementNoise;
    private GameObject player;

    void Start()
    {
        initialPosition = transform.position;
        player = GameManager.Instance.player;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!soundPlayed && Random.Range(1, 100) <= GameManager.Instance.eventChance) // Check if the sound has already been played
            {
                StartMoving();
                AudioManager.instance.PlaySound(movementNoise, player.transform);
                soundPlayed = true; // Set the flag to true to indicate that the sound has been played
            }
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    public void StartMoving()
    {
        if (targetObject != null)
        {
            isMoving = true;
        }
        else
        {
            Debug.LogWarning("Target object is not set.");
        }
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    private void MoveTowardsTarget()
    {
        if (targetObject != null)
        {
            Vector3 targetPosition = targetObject.transform.position;
            targetPosition.y = initialPosition.y; // Adjusting the target position's height
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition, step);
        }
    }

}
