using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectDoor : InteractableTemplate
{
    private Camera mainCam;
    private CameraLook camLookScript;
    public Vector3 camRotation;// = new Vector3(0,0,0);
    public Vector3 camPosition;// = new Vector3(-1.54999995f, 0.310000002f, 7.25f);
    private Quaternion oldCamRotation;
    private Vector3 oldCamPosition;

    bool looking = false;
    bool playerCanMove = true;
    bool stopLooking = false;
    private void Start()
    {
        mainCam = Camera.main;
        camLookScript = Camera.main.GetComponent<CameraLook>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) && looking)
        {
            looking = false;
            playerCanMove = true;
            stopLooking = true;

            //GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
            this.gameObject.GetComponent<BoxCollider>().enabled = true;
            //GetComponent<CameraLook>().enabled = true;
            
        }

        if (looking)
        {
            mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, camPosition, 2.5f * Time.deltaTime);
            mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.localRotation, Quaternion.Euler(camRotation), 2.5f * Time.deltaTime);
            //mainCam.transform.rotation = Quaternion.RotateTowards(mainCam.transform.localRotation, Quaternion.Euler(camRotation), 360);
            camLookScript.enabled = false; 
        }
        
        if(stopLooking)
        {
            mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, oldCamPosition, 2.5f * Time.deltaTime);
            mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, oldCamRotation, 2.5f * Time.deltaTime);

            mainCam.transform.parent = GameObject.Find("Player").transform;
            
            if(mainCam.transform.position == oldCamPosition)
            {
                stopLooking = false;
                GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
                mainCam.GetComponent<CameraLook>().canHeadBob = true;
                camLookScript.enabled = true;
            }
        }


        //we would need the state manager at this point to be able to freeze player movement and interaction
        if(!playerCanMove)
        {
            GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            //GetComponent<CameraLook>().enabled = false;
        }

    }
    public override void Interact()
    {
        Debug.Log("*** Interacting with door ***");

        mainCam.transform.parent = null;
        
        oldCamPosition = mainCam.transform.position;
        oldCamRotation = mainCam.transform.rotation;

        looking = true;
        playerCanMove = false;

        mainCam.GetComponent<CameraLook>().canHeadBob = false;
    }
}
