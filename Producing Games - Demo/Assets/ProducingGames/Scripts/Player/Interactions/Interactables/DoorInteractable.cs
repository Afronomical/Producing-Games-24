using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.UI;

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
    public enum DoorStates
    {
        None,

        Open,
        Shut,
        Locked
    }

    [Header("References:")]
    [SerializeField] private Transform doorHingeTransform;

    [Header("Customizables:")]

    [Tooltip("How much the door rotates by in degrees")]
    [SerializeField] private float doorRotationOffset = 45.0f;

    [Tooltip("The delay between interactions in seconds")]
    [SerializeField] private float interactionInterval = 2.0f;

    [Tooltip("How long it takes for the door to close/open")]
    [SerializeField] private float doorDuration = 3.0f;

    [SerializeField] private DoorStates currentState = DoorStates.Open;
    [SerializeField] private bool isPatientDoor = false;

    [Space(10)]

    [Tooltip("No touch")]
    public List<GameObject> entitiesInsideRoom = new();

    private bool isDoorRotating = false;
    private float currentTime;

    private Quaternion initialDoorRotation;
    private Quaternion targetRotation;
    private Transform playerPosition;
    private Vector3 forwardDirection;
    private GameObject demon;
    private DemonCharacter demonCharacter;

    public DoorStates PreviousState { get; private set; }

    [Header("Temporary Variables for Testing:")]
    [SerializeField] private Material closedMaterial;
    [SerializeField] private Material openMaterial;
    private MeshRenderer doorMeshRenderer;

    private void Awake()
    {
        doorMeshRenderer = GetComponent<MeshRenderer>();

        initialDoorRotation = doorHingeTransform.rotation;
        forwardDirection = transform.forward;

        ChangeDoorMaterial();

        switch (currentState)
        {
            case DoorStates.Open:
                Vector3 startingRot = doorHingeTransform.rotation.eulerAngles;

                int rand = Random.Range(0, 2);

                if (rand == 0)
                    startingRot.y += doorRotationOffset;
                else
                    startingRot.y -= doorRotationOffset;

                doorHingeTransform.rotation = Quaternion.Euler(startingRot);
                break;
            case DoorStates.None:
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        playerPosition = FindFirstObjectByType<PlayerMovement>().transform;
    }

    private void Update()
    {
        if (demon == null)
        {
            demon = GameManager.Instance.demon;
            demonCharacter = demon.GetComponent<DemonCharacter>();
        }

        // INFO: Reset door to previous state when demon is no longer in rage mode
        if (!demonCharacter.IsInRageMode() && currentState == DoorStates.Locked)
        {
            ChangeDoorState(PreviousState);
        }

        // INFO: If the door is currently rotating:
        if (isDoorRotating)
        {
            currentTime += Time.deltaTime;

            // INFO: Rotates the door to the target rotation for doorDuration time
            doorHingeTransform.rotation = Quaternion.Lerp(doorHingeTransform.rotation, targetRotation, currentTime / doorDuration);

            if (currentTime > doorDuration)
            {
                currentTime = 0.0f;
                isDoorRotating = false;
            }
        }

        // INFO: Will automatically close and lock when the demon goes into rage mode
        // if it's a patient door and there and the player or demon aren't in the room
        if (isPatientDoor && !isDoorRotating)
        {
            if (demonCharacter.IsInRageMode() && entitiesInsideRoom.Count == 0)
            {
                //Invoke(nameof(AutomaticLockDoor), doorDuration);
                ChangeDoorState(DoorStates.Locked);
            }
            else if (demonCharacter.IsInRageMode() && entitiesInsideRoom.Count > 0)
            {
                //Invoke(nameof(AutomaticOpenDoor), doorDuration);
                ChangeDoorState(DoorStates.Open);
            }

        }
    }

    public override void Interact()
    {
        // INFO: Given that the player tries to interact with the door when the door is
        // still rotating, the function won't do anything
        if (isDoorRotating)
            return;

        if (currentState == DoorStates.Open)
            ChangeDoorState(DoorStates.Shut);
        else if (currentState == DoorStates.Shut)
            ChangeDoorState(DoorStates.Open);

        /*
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
        */
    }

    public void ChangeDoorState(DoorStates newState)
    {
        if (currentState != newState)
        {
            if (currentState != DoorStates.Locked)
                PreviousState = currentState;

            currentState = newState;

            switch (currentState)
            {
                case DoorStates.Open:
                    Open(playerPosition.position);
                    break;
                case DoorStates.Shut:
                    Shut();
                    break;
                case DoorStates.Locked:
                    Locked();
                    break;
                case DoorStates.None:
                    break;
                default:
                    break;
            }
            isDoorRotating = true;
        }

        /*
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
        */
    }

    /// <summary>
    /// The function that gets called when the door goes into the open state
    /// </summary>
    public void Open(Vector3 characterPosition)
    {
        ChangeDoorMaterial();

        float dot = Vector3.Dot(forwardDirection, (characterPosition - transform.position).normalized);

        Vector3 currentRotation = doorHingeTransform.rotation.eulerAngles;

        // INFO: Opens the door outwards based on which way the player is facing it
        if (dot >= 0.0f)
            currentRotation.y += doorRotationOffset;
        else
            currentRotation.y -= doorRotationOffset;

        targetRotation = Quaternion.Euler(currentRotation);
    }

    /// <summary>
    /// The function that gets called when the door goes into the shut state
    /// </summary>
    private void Shut()
    {
        ChangeDoorMaterial();
        targetRotation = initialDoorRotation;
    }

    /// <summary>
    /// The function that gets called when the door goes into the locked state
    /// </summary>
    private void Locked()
    {
        ChangeDoorMaterial();
        targetRotation = initialDoorRotation;
    }

    /// <summary>
    /// Temp function for locked/unlocked door visualisation
    /// </summary>
    private void ChangeDoorMaterial()
    {
        if (currentState == DoorStates.Locked)
            doorMeshRenderer.material = closedMaterial;
        else
            doorMeshRenderer.material = openMaterial;

        /*
        if (isLocked)
            doorMeshRenderer.material = closedMaterial;
        else
            doorMeshRenderer.material = openMaterial;
        */
    }
}
