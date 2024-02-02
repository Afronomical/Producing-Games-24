using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingCutScene : InteractableTemplate
{
    private Camera cam;
    private Transform playerRef;
    public List<Transform> points;

    private Animator animDoorRight;
    private Animator animDoorLeft;
    bool goIn;
    bool goOut;
    bool isInside;
    private Vector3 camPos;
    private Quaternion camRot;

    private bool goToPoint1 = false;
    private bool goToPoint0 = true;

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

        if(Input.GetKeyDown(KeyCode.C) && !goIn) 
            goOut = true;
        

        //Go in hiding spot
        if (goIn)
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
            //Go to the entrance of hiding spot
            if (goToPoint0)
            {
                playerRef.position = Vector3.MoveTowards(playerRef.position, points[0].position, 3f * Time.deltaTime);

                if (Quaternion.Angle(playerRef.rotation, points[0].rotation) > 0.1)
                {
                    playerRef.rotation = Quaternion.Lerp(playerRef.rotation, points[0].rotation, 3f * Time.deltaTime);
                }
            }
            
            //Go Inside of hiding spot once the camera is at the entrance point
            if (goToPoint1)
            {
                playerRef.position = Vector3.MoveTowards(playerRef.position, points[1].position, 4f * Time.deltaTime);

                if (Quaternion.Angle(playerRef.rotation, points[0].rotation) > 0.1)
                {
                    playerRef.rotation = Quaternion.Lerp(playerRef.rotation, points[1].rotation, 4f * Time.deltaTime);
                }

                
            }

            //Checks when the camera can transition
            if (playerRef.position == points[0].position && Quaternion.Angle(playerRef.rotation, points[0].rotation) > 0.1)
            {
                goToPoint0 = false;
                goToPoint1 = true;
            }
            float testRot = Quaternion.Angle(playerRef.rotation, points[1].rotation);
            if (playerRef.position == points[1].position && Quaternion.Angle(playerRef.rotation, points[1].rotation) < 0.5)
            {
                animDoorLeft.SetBool("EnterCupboard", false);
                animDoorRight.SetBool("EnterCupboard", false);
                goIn = false;
                goToPoint0 = true;
                goToPoint1 = false;
                isInside = true;

            }
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
        }

    }

    public override void Interact()
    {
        if(!isInside)
            goIn = true;
        
    }
}
