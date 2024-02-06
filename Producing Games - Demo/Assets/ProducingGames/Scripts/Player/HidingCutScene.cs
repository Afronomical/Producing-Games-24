using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HidingCutScene : InteractableTemplate
{
    private Camera cam;
    private Transform playerRef;
    public List<Transform> points;

    private Animator animDoorRight;
    private Animator animDoorLeft;
    private Vector3 camPos;
    private Quaternion camRot;

    private bool isInside;
    private int pointIndex;

    public enum PlayerHidingStates
    {
        None,
        goIn,
        inside,
        goOut,
        outside
        
    }

    private PlayerHidingStates playerHidingStates;

    private void Start()
    {

        
        cam = Camera.main;
        playerRef = GameObject.Find("Player").transform;
        animDoorRight = GameObject.Find("CupboardDoorRight").GetComponent<Animator>();
        animDoorLeft = GameObject.Find("CupboardDoorLeft").GetComponent<Animator>();
        /*
        Had issues while tidying code, will be fixing shortly...
        camPos = cam.transform.position;
        camRot = cam.transform.rotation;
        */
    }

    private void Update()
    {
        if (points == null || points.Count < 1)
            return;

        /* if(Input.GetKeyDown(KeyCode.C) && !goIn && playerRef.position == points[1].position) 
             goOut = true;*/

        if(Input.GetKeyDown(KeyCode.K)) 
            playerHidingStates = PlayerHidingStates.goIn;
        

        switch (playerHidingStates)
        {
            case PlayerHidingStates.None:
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
        
        //Go in hiding spot
        /*if (PlayerHidingStates.goIn)
        {
            //Disable player's movement and body
            //=======================================================================
            playerRef.GetComponent<PlayerMovement>().enabled = false;
            playerRef.GetComponent<CharacterController>().enabled = false;
            playerRef.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            cam.GetComponent<CameraLook>().enabled = false;
            cam.transform.rotation = playerRef.transform.rotation;
            //=======================================================================

            animDoorLeft.SetBool("EnterCupboard", true);
            animDoorRight.SetBool("EnterCupboard", true);
           
        }

        //Go out of Hiding Spot
        if (goOut)
        {
            animDoorLeft.SetBool("EnterCupboard", true);
            animDoorRight.SetBool("EnterCupboard", true);

            //Moves the Camera to the Entrance of the hiding spot
            playerRef.position = Vector3.MoveTowards(playerRef.position, points[0].position, 2.5f * Time.deltaTime);

            //if (playerRef.rotation != points[0].rotation)
            if (Quaternion.Angle(playerRef.rotation, points[0].rotation) > 0.1)
            {
                playerRef.rotation = Quaternion.Lerp(playerRef.rotation, points[0].rotation, 3f * Time.deltaTime);
            }

            if (playerRef.position == points[0].position && Quaternion.Angle(playerRef.rotation, points[0].rotation) < 0.1)
            {
                animDoorLeft.SetBool("EnterCupboard", false);
                animDoorRight.SetBool("EnterCupboard", false);
                goOut = false;
                isInside = false;

                //Enables player's movement and body
                //=======================================================================
                playerRef.GetComponent<PlayerMovement>().enabled = true;
                playerRef.GetComponent<CharacterController>().enabled = true;
                playerRef.GetComponent<MeshRenderer>().enabled = true;
                gameObject.GetComponent<BoxCollider>().enabled = true;
                cam.GetComponent<CameraLook>().enabled = true;
                //=======================================================================
            }
        }*/

    }


    public void GoIn()
    {
         playerRef.position = Vector3.MoveTowards(playerRef.position, points[pointIndex].position, 3f * Time.deltaTime);
                

         if (Quaternion.Angle(playerRef.rotation, points[pointIndex].rotation) > 0.1)
            playerRef.rotation = Quaternion.Lerp(playerRef.rotation, points[pointIndex].rotation, 3f * Time.deltaTime);

        //Checks when the camera can transition
        if (Vector2.Distance(playerRef.position, points[pointIndex].position) <= 0.2 && Quaternion.Angle(playerRef.rotation, points[pointIndex].rotation) > 0.5)
        {
            animDoorLeft.SetBool("EnterCupboard", false);
            animDoorRight.SetBool("EnterCupboard", false);
            pointIndex++;
            
            if(pointIndex > 1)
                playerHidingStates = PlayerHidingStates.inside;
        }

        //float testRot = Quaternion.Angle(playerRef.rotation, points[1].rotation);
    }


    public void Inside()
    {
        if(!isInside)
            isInside = true;
    }
    public void GoOut()
    {

    }
    public void Outside()
    {

    }

    //This is where the animation will be called
    public void CupboardAnim()
    {

    }

    public override void Interact()
    {
        if (!isInside)
            playerHidingStates = PlayerHidingStates.goIn;
        
    }
}
