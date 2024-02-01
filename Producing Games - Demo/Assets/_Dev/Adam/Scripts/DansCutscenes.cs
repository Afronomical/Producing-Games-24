using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DansCutscenes : InteractableTemplate
{
    private Camera cam;
    private GameObject playerRef;
    public List<Transform> points;

    private Animator animDoorRight;
    private Animator animDoorLeft;
    bool goIn;
    bool goOut;
    bool isInside;
    private Vector3 camPos;
    private Quaternion camRot;

    private bool goToPoint2 = false;
    private bool goToPoint1 = true;

    private void Start()
    {
        cam = Camera.main;
        playerRef = GameObject.Find("Player");
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
            playerRef.GetComponent<Rigidbody>().useGravity = false;
            playerRef.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            cam.GetComponent<CameraLook>().enabled = false;
            //=======================================================================

            animDoorLeft.SetBool("EnterCupboard", true);
            animDoorRight.SetBool("EnterCupboard", true);
            //Go to the entrance of hiding spot
            if (goToPoint1)
            {
                playerRef.transform.position = Vector3.MoveTowards(playerRef.transform.position, points[0].position, 10f * Time.deltaTime);

                if (playerRef.transform.rotation != points[0].rotation)
                {
                    playerRef.transform.rotation = Quaternion.Lerp(playerRef.transform.rotation, points[0].rotation, 5f * Time.deltaTime);
                }
            }
            
            //Go Inside of hiding spot once the camera is at the entrance point
            if (goToPoint2)
            {
                playerRef.transform.position = Vector3.MoveTowards(playerRef.transform.position, points[1].position, 4f * Time.deltaTime);

                if (playerRef.transform.rotation != points[1].rotation)
                {
                    /*if (playerRef.transform.position == points[0].position)
                        playerRef.transform.rotation = points[0].rotation;*/

                    playerRef.transform.rotation = Quaternion.Lerp(playerRef.transform.rotation, points[1].rotation, 4f * Time.deltaTime);
                }

                
            }

            //Checks when the camera can transition
            if (playerRef.transform.position == points[0].position && playerRef.transform.rotation == points[0].rotation)
            {
                goToPoint1 = false;
                goToPoint2 = true;
            }

            if (playerRef.transform.position == points[1].position && playerRef.transform.rotation == points[1].rotation)
            {
                animDoorLeft.SetBool("EnterCupboard", false);
                animDoorRight.SetBool("EnterCupboard", false);
                goIn = false;
                goToPoint1 = true;
                goToPoint2 = false;
                isInside = true;

            }
        }

        //Go out of Hiding Spot
        if (goOut)
        {
            animDoorLeft.SetBool("EnterCupboard", true);
            animDoorRight.SetBool("EnterCupboard", true);

            //Moves the Camera to the Entrance of the hiding spot
            playerRef.transform.position = Vector3.MoveTowards(playerRef.transform.position, points[0].position, 2.5f * Time.deltaTime);

            if (playerRef.transform.rotation != points[0].rotation)
            {
                playerRef.transform.rotation = Quaternion.Lerp(playerRef.transform.rotation, points[0].rotation, 3f * Time.deltaTime);
            }
            if (playerRef.transform.position == points[0].position && playerRef.transform.rotation == points[0].rotation)
            {
                animDoorLeft.SetBool("EnterCupboard", false);
                animDoorRight.SetBool("EnterCupboard", false);
                goOut = false;
                 isInside = false;

                //Enables player's movement and body
                //=======================================================================
                playerRef.GetComponent<PlayerMovement>().enabled = true;
                playerRef.GetComponent<Rigidbody>().useGravity = true;
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
