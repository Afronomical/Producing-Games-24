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
    private Transform playerRef;
    private float originCamNearClippingPlane;

    [Header("Object Animation/Object Door Material (Leave empty if not required!)")]
    public Animator playAnimation;
    public Material doorMaterialRef;

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
    HidingScare hidingScare;

    private void Start()
    {
        cam = Camera.main;
        playerRef = GameManager.Instance.player.transform;
        hidingScare = Object.FindFirstObjectByType<HidingScare>();

        originCamNearClippingPlane = cam.nearClipPlane;
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
        cam.transform.rotation = playerRef.rotation;
        cam.nearClipPlane = 0.01f;
        cam.GetComponent<CameraLook>().enabled = false;
        PlayerControlsAccess(false);
        CupboardAnim(true);
        playerRef.position = Vector3.MoveTowards(playerRef.position, points[pointIndex].position, enterTransitionSpeed * Time.deltaTime);
                
         if (Quaternion.Angle(playerRef.rotation, points[pointIndex].rotation) > 0.1)
            playerRef.rotation = Quaternion.Lerp(playerRef.rotation, points[pointIndex].rotation, enterTransitionSpeed * Time.deltaTime);

        //Checks when the camera can transition
        if (Vector3.Distance(playerRef.position, points[pointIndex].position) <= 0.2 && Mathf.Approximately(Quaternion.Angle(playerRef.rotation, points[pointIndex].rotation), 0))
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
        CupboardAnim(false);
        cam.GetComponent<CameraLook>().enabled = true;
        if ((Input.GetKeyDown(KeyCode.C)))
        {
            CupboardAnim(true);
            playerHidingStates = PlayerHidingStates.goOut;
        }
            
    }

    //Logic handles the player exiting the hiding spot
    public void GoOut()
    {
        playerRef.position = Vector3.MoveTowards(playerRef.position, points[pointIndex].position, exitTransitionSpeed * Time.deltaTime);

        //Checks when the camera can transition
        if (Vector3.Distance(playerRef.position, points[pointIndex].position) <= 0.2)
        {
            pointIndex++;
            if (pointIndex == points.Count)
            {
                playerRef.rotation = points[pointIndex - 1].rotation;
                playerHidingStates = PlayerHidingStates.outside;
                pointIndex = 0;
            }
                
        }
    }

    //This will enable the player's controls again
    public void Outside()
    {
        CupboardAnim(false);
        PlayerControlsAccess(true);
        playerHidingStates = PlayerHidingStates.none;
        cam.nearClipPlane = originCamNearClippingPlane;
    }

    //Logic handles the player entering the hiding spot
    public void PlayerControlsAccess(bool canControl)
    {
        playerRef.GetComponent<PlayerMovement>().enabled = canControl;
        playerRef.GetComponent<DropItem>().enabled = canControl;
        playerRef.GetComponent<CharacterController>().enabled = canControl;
        playerRef.GetComponent<MeshRenderer>().enabled = canControl;
        gameObject.GetComponent<BoxCollider>().enabled = canControl;
        
    }

    //This is where the animation will be called, allows if there is multiple steps with the animation (Currently just open/close doors for the cupboard)
    public void CupboardAnim(bool isEntering)
    {
        if(playAnimation != null)
        playAnimation.SetBool("CupboardOpen", isEntering);
    }

    //When the Player interacts with the hiding spot, start entering
    public override void Interact()
    {
        if (playerHidingStates == PlayerHidingStates.none)
            playerHidingStates = PlayerHidingStates.goIn;
        
    }

}