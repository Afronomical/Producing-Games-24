using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Written by: Matej Cincibus
/// Moderated by: ...
/// 
/// This class handles how doors are interacted with in the game, if the player is currently holding
/// a key and interacts with a door they will lock/unlock the door, otherwise if the door is open,
/// the player isn't holding a key and interacts with the door, they will open/close it.
/// </summary>

public class DoorInteractable : InteractableTemplate
{
    [Header("References:")]
    [SerializeField] private Transform doorHingeTransform;

    [Header("Customizables:")]
    [Tooltip("How much the door rotates by in degrees")][SerializeField] private float doorRotationOffset = 45.0f;
    [Tooltip("The delay between interactions in seconds")][SerializeField] private float interactionInterval = 2.0f;
    [SerializeField] private float doorRotationSpeed = 1.75f;
    [SerializeField] private bool isLocked = false;

    private bool isOpen = false;
    private bool hasInteractedWithDoor = false;
    private bool isDoorRotating = false;

    private float timeSinceInteraction = 0.0f; // INFO: Prevents multiple interactions at once
    private float t = 0.0f; // INFO: How much the door lerps

    private Quaternion initialDoorRotation;
    private Quaternion targetRotation;
    private Transform playerPosition;
    private Vector3 forwardDirection;

    // INFO: Temporary Variables:
    [SerializeField] private Material closedMaterial;
    [SerializeField] private Material openMaterial;
    private MeshRenderer doorMeshRenderer;

    private void Awake()
    {
        doorMeshRenderer = GetComponent<MeshRenderer>();

        initialDoorRotation = doorHingeTransform.rotation;
        forwardDirection = transform.forward;

        ChangeDoorMaterial();
    }

    private void Start()
    {
        playerPosition = FindFirstObjectByType<PlayerMovement>().transform;
    }

    private void Update()
    {
        // INFO: If the door is currently rotating:
        if (isDoorRotating)
        {
            // INFO: Calculate the speed at which the door will rotate
            t = Mathf.Clamp01(t + doorRotationSpeed * Time.deltaTime);
            doorHingeTransform.rotation = Quaternion.Slerp(doorHingeTransform.rotation, targetRotation, doorRotationSpeed * Time.deltaTime);

            if (doorHingeTransform.rotation == targetRotation)
                isDoorRotating = false;
        }

        // INFO: Prevents multiple interactions
        if (hasInteractedWithDoor)
        {
            timeSinceInteraction += Time.deltaTime;

            if (timeSinceInteraction > interactionInterval)
            {
                timeSinceInteraction = 0.0f;
                hasInteractedWithDoor = false;
            }
        }
    }

    public override void Interact()
    {
        // INFO: Given that the player tries to interact with the door when the interaction is
        // still on cooldown, the function won't do anything
        if (hasInteractedWithDoor)
            return;

        hasInteractedWithDoor = true;

        // INFO: Given that the player is holding an item and that item has a key interactable component on it then:
        if (InventoryHotbar.instance.currentItem != null && InventoryHotbar.instance.currentItem.prefab.TryGetComponent<KeyInteractable>(out _))
        {
            if (isLocked)
            {
                isLocked = false;
                //makes the door an obstacle when locked
                GetComponent<NavMeshObstacle>().enabled = false;
                GetComponent<NavMeshObstacle>().carving = false;
            }
            else
            {
                isLocked = true;
                GetComponent<NavMeshObstacle>().enabled = true;
                GetComponent<NavMeshObstacle>().carving = true;
                // INFO: Closes the door automatically if it's open when the player locks it
                ChangeDoorState();
            }
            ChangeDoorMaterial();
        }
        else if (!isLocked)
            ChangeDoorState();
    }

    private void ChangeDoorState()
    {
        if (isOpen || isLocked)
        {
            isOpen = false;
            targetRotation = initialDoorRotation;
        }
        else if (!isOpen)
        {
            isOpen = true;

            float dot = Vector3.Dot(forwardDirection, (playerPosition.position - transform.position).normalized);

            Vector3 currentRotation = doorHingeTransform.rotation.eulerAngles;

            // INFO: Opens the door outwards based on which way the player is facing it
            if (dot >= 0.0f)
                currentRotation.y += doorRotationOffset;
            else
                currentRotation.y -= doorRotationOffset;

            targetRotation = Quaternion.Euler(currentRotation);
        }
        isDoorRotating = true;
    }

    // INFO: Temp function for locked/unlocked door visualisation
    private void ChangeDoorMaterial()
    {
        if (isLocked)
            doorMeshRenderer.material = closedMaterial;
        else
            doorMeshRenderer.material = openMaterial;
    }
}
