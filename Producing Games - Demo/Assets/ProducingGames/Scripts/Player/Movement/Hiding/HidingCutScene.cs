using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

public class HidingCutScene : InteractableTemplate
{
    private Camera cam;
    private int pointIndex;
    private Transform playerTransformRef;
    private Animator playDoorAnimation;
    public PatientCharacter patient;
    public bool playerIsOccupying;
   [Header("Hiding Animation Position Points")]
    public List<Transform> points;

    [Header("Hiding Animation Speeds")]
    public float enterTransitionSpeed = 3;
    public float exitTransitionSpeed = 3;

    private PlayerMovement playerMovement;

    public enum PlayerHidingStates
    {
        none,
        goIn,
        inside,
        goOut,
        outside
    }

    private PlayerHidingStates playerHidingStates;
    HidingScare hidingScare;

    private void Start()
    {
        playDoorAnimation = gameObject.GetComponent<Animator>();
        cam = Camera.main;
        playerTransformRef = GameManager.Instance.player.transform;
        hidingScare = Object.FindFirstObjectByType<HidingScare>();
        


        // INFO: Get Local Reference to Player
        playerMovement = GameManager.Instance.player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        //Checks if there is any "Hiding Animation Position Points"
        if (points == null || points.Count < 1)
            return;

        //Will constantly check for a state change (For example, once goIn is finished, inside will be called)
        switch (playerHidingStates)
        {
            case PlayerHidingStates.none:
                break;

            case PlayerHidingStates.goIn:
                GoIn();
                break;

            case PlayerHidingStates.inside:
                Inside();
                break;

            case PlayerHidingStates.goOut:
                GoOut();
                break;

            case PlayerHidingStates.outside:
                Outside();
                break;
        }
    }


    //Logic handles the player entering the hiding spot
    public void GoIn()
    {
        EnteringAnim(true);

        playerMovement.isHiding = true;
        playerIsOccupying = true;

        playerTransformRef.GetComponent<PickUpItem>().enabled = false;
        cam.transform.rotation = playerTransformRef.rotation;
        cam.GetComponent<CameraLook>().enabled = false;
        PlayerControlsAccess(false);

        playerTransformRef.position = Vector3.MoveTowards(playerTransformRef.position, points[pointIndex].position, enterTransitionSpeed * Time.deltaTime);
         if (Quaternion.Angle(playerTransformRef.rotation, points[pointIndex].rotation) > 0.1)
            playerTransformRef.rotation = Quaternion.Lerp(playerTransformRef.rotation, points[pointIndex].rotation, enterTransitionSpeed * Time.deltaTime);

        //Checks when the camera can transition
        if (Vector3.Distance(playerTransformRef.position, points[pointIndex].position) <= 0.2 && Mathf.Approximately(Quaternion.Angle(playerTransformRef.rotation, points[pointIndex].rotation), 0))
        {
            pointIndex++;
            
            if(pointIndex == points.Count - 1)
            {
                playerHidingStates = PlayerHidingStates.inside;
            }
        }
    }

    //If the player is inside the cupboard, it allows the player to click "c" to exit (moves to the GoOut function)
    public void Inside()
    {
        EnteringAnim(false);
        cam.GetComponent<CameraLook>().enabled = true;
        
        base.actionTooltip.text = "Press C to stop hiding!";
        base.actionTooltip.enabled = true;
        if ((Input.GetKeyDown(KeyCode.C) && !Input.GetMouseButton(0)))
        {
            base.actionTooltip.enabled = false;
            PeekAnim(false);
            playerHidingStates = PlayerHidingStates.goOut;
        }

        //Hold LMB to open the door to peek out, when the LMB is released, it will close the door again
        if (Input.GetMouseButton(0))
            PeekAnim(true);
            
        
        else
            PeekAnim(false);
            
    }

    //Logic handles the player exiting the hiding spot
    public void GoOut()
    {
        EnteringAnim(true);
        playerMovement.isHiding = false;
        playerTransformRef.position = Vector3.MoveTowards(playerTransformRef.position, points[pointIndex].position, exitTransitionSpeed * Time.deltaTime);

        //Checks when the camera can transition
        if (Vector3.Distance(playerTransformRef.position, points[pointIndex].position) <= 0.2)
        {
            pointIndex++;
            if (pointIndex == points.Count)
            {
                playerTransformRef.rotation = points[pointIndex - 1].rotation;
                playerHidingStates = PlayerHidingStates.outside;
                playerTransformRef.GetComponent<PickUpItem>().enabled = true;
                pointIndex = 0;
            }
                
        }
    }

    //This will enable the player's controls again
    public void Outside()
    {
        playerIsOccupying = false;
        EnteringAnim(false);
        PlayerControlsAccess(true);
        playerHidingStates = PlayerHidingStates.none;
    }

    //Disables/Enables the Player's controls,colliders and mesh
    public void PlayerControlsAccess(bool canControl)
    {
        playerTransformRef.GetComponent<PlayerMovement>().enabled = canControl;
        playerTransformRef.GetComponent<DropItem>().enabled = canControl;
        playerTransformRef.GetComponent<CharacterController>().enabled = canControl;
        playerTransformRef.GetComponent<MeshRenderer>().enabled = canControl;
        
        cam.GetComponent<CameraLook>().canHeadBob = canControl;
    }

    //Peeking animation
    public void PeekAnim(bool isEntering)
    {
        if(playDoorAnimation != null)
            playDoorAnimation.SetBool("Peeking", isEntering);
    }
    
    //Entering animation
    public void EnteringAnim(bool isEntering)
    {
        if (playDoorAnimation != null)
            playDoorAnimation.SetBool("Entering", isEntering);
    }

    //When the Player interacts with the hiding spot, start entering
    public override void Interact()
    {
        if(patient != null)
        {
            // INFO: Opens door
            PeekAnim(true);

            // INFO: Kick patient out of hiding spot by accessing the last element held
            // in the points list (Out)
            patient.transform.position = new (points[^1].position.x, patient.transform.position.y, points[^1].position.z);

            // INFO: Sets patient to escorted
            patient.ChangePatientState(PatientCharacter.PatientStates.Escorted);
            patient = null;

            Invoke(nameof(InvokeCloseDoor), 2);
        }
        else
        {
            if (playerHidingStates == PlayerHidingStates.none)
                playerHidingStates = PlayerHidingStates.goIn;
        }

    }

    private void InvokeCloseDoor()
    {
        PeekAnim(false);
    }
}