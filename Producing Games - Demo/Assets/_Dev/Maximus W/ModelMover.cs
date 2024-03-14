using UnityEngine;

public class ModelMover : MonoBehaviour
{
    public Vector3 movementDirection = Vector3.forward; // Direction of movement
    public float movementDistance = 5.0f; // Distance of movement
    public float movementSpeed = 1.0f; // Speed of movement

    private Vector3 initialPosition; // Initial position of the object
    private Vector3 targetPosition; // Target position of the movement
    private bool isMoving = false; // Flag to check if the object is currently moving

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
            StartMoving();
            AudioManager.instance.PlaySound(movementNoise, player.transform);
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
        targetPosition = initialPosition + movementDirection.normalized * movementDistance;
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    private void MoveTowardsTarget()
    {
        float step = movementSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetPosition, step);
        
    }
}


