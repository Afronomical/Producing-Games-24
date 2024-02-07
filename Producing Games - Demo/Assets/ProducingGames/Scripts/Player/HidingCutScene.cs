using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HidingCutScene : InteractableTemplate
{
    private Camera cam;
    private int pointIndex;

    [Header("Player Reference/Object Animation")]
    public Transform playerRef;
    //public Animator playAnimation;

    [Header("Hiding Animation Position Points")]
    public List<Transform> points;

    [Header("Hiding Animation Speeds")]
    public float enterTransitionSpeed = 3;
    public float exitTransitionSpeed = 3;

    public enum PlayerHidingStates
    {
        none,
        goIn,
        inside,
        goOut,
        outside
    }
    private PlayerHidingStates playerHidingStates;

    private void Start()
    {
        cam = Camera.main;
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
        PlayerControlsAccess(false);
        playerRef.position = Vector3.MoveTowards(playerRef.position, points[pointIndex].position, enterTransitionSpeed * Time.deltaTime);
                
         if (Quaternion.Angle(playerRef.rotation, points[pointIndex].rotation) > 0.1)
            playerRef.rotation = Quaternion.Lerp(playerRef.rotation, points[pointIndex].rotation, enterTransitionSpeed * Time.deltaTime);

        //Checks when the camera can transition
        if (Vector3.Distance(playerRef.position, points[pointIndex].position) <= 0.2 && Mathf.Approximately(Quaternion.Angle(playerRef.rotation, points[pointIndex].rotation), 0))
        {
            pointIndex++;
            
            if(pointIndex == points.Count)
            {
                playerHidingStates = PlayerHidingStates.inside;
                pointIndex = 0;
            }
        }
    }

    //If the player is insdie the cupboard, it allows the player to click "c" to exit (moves to the GoOut function)
    public void Inside()
    {
        if ((Input.GetKeyDown(KeyCode.C)))
            playerHidingStates = PlayerHidingStates.goOut;
    }

    //Logic handles the player exiting the hiding spot
    public void GoOut()
    {
        playerRef.position = Vector3.MoveTowards(playerRef.position, points[pointIndex].position, exitTransitionSpeed * Time.deltaTime);

        //Checks when the camera can transition
        if (Vector3.Distance(playerRef.position, points[pointIndex].position) <= 0.2)
        {
            pointIndex++;
            if (pointIndex > 0)
                playerHidingStates = PlayerHidingStates.outside;
        }
    }

    //This will enable the player's controls again
    public void Outside()
    {
        PlayerControlsAccess(true);
        playerHidingStates = PlayerHidingStates.none;
    }

    //Logic handles the player entering the hiding spot
    public void PlayerControlsAccess(bool canControl)
    {
        playerRef.GetComponent<PlayerMovement>().enabled = canControl;
        playerRef.GetComponent<CharacterController>().enabled = canControl;
        playerRef.GetComponent<MeshRenderer>().enabled = canControl;
        gameObject.GetComponent<BoxCollider>().enabled = canControl;
        cam.GetComponent<CameraLook>().enabled = canControl;
    }

    //This is where the animation will be called
    public void CupboardAnim(bool isEntering)
    {

    }

    //When the Player interacts with the hiding spot, start entering
    public override void Interact()
    {
        if (playerHidingStates == PlayerHidingStates.none)
            playerHidingStates = PlayerHidingStates.goIn;
        
    }
}
