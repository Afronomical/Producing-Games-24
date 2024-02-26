using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class InspectableObject : InteractableTemplate
{
    protected Camera mainCam;
    protected CameraLook camLookScript;
    public Vector3 camRotation;// = new Vector3(0,0,0);
    public Transform camMoveTransform;

    public Vector3 camPosition;// = new Vector3(-1.54999995f, 0.310000002f, 7.25f);
    protected Quaternion oldCamRotation;
    protected Vector3 oldCamPosition;

    protected bool looking = false;
    protected bool playerCanMove = true;
    protected bool stopLooking = false;

    [Header("Camera Values")]
    public GameObject monitor;
    public Material[] cameraScreens;

    private Material currentMaterial;
    private int index = 0;

    Vector2 defaultScreenSize;

    protected virtual void Start()
    {
        mainCam = Camera.main;//.transform.parent.gameObject;
        camLookScript = Camera.main.GetComponent<CameraLook>();
        camPosition = camMoveTransform.position;
        camRotation = camMoveTransform.rotation.eulerAngles;

        defaultScreenSize = cameraScreens[0].mainTextureScale;
    }

    protected virtual void Update()
    {


        currentMaterial = cameraScreens[index];
        monitor.GetComponent<MeshRenderer>().material = currentMaterial;

        if (Input.GetKeyDown(KeyCode.X) && looking)
        {
            cameraScreens[index].mainTextureScale = cameraScreens[index].mainTextureScale * new Vector2(2, 2);
        }
        if (Input.GetKeyDown(KeyCode.Z) && looking)
        {
            cameraScreens[index].mainTextureScale = cameraScreens[index].mainTextureScale / new Vector2(2, 2)/* - cameraScreens[index].mainTextureScale / 2*/;
        }


        if (Input.GetKeyDown(KeyCode.RightArrow) && looking)
        {
            if (index == cameraScreens.Length - 1)
            {
                index = 0;
                return;
            }
            index++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && looking)
        {
            if (index == 0)
            {
                index = cameraScreens.Length - 1;
                return;
            }
            index--;
        }

        if (Input.GetKeyDown(KeyCode.C) && looking)
        {
            looking = false;
            playerCanMove = true;
            stopLooking = true;

            //re-enable interaction ability
            this.gameObject.GetComponent<BoxCollider>().enabled = true;

            foreach(var m in cameraScreens)
            {
                m.mainTextureScale = defaultScreenSize;
            }
        }
        //move camera to glass panel if door is interacted with
        if (looking)
        {
            mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, camPosition, 2.5f * Time.deltaTime);
            mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.localRotation, Quaternion.Euler(camRotation), 2.5f * Time.deltaTime);
            camLookScript.enabled = false;
        }
        //move camera back to initial position if looking is done
        if (stopLooking)
        {
            mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, oldCamPosition, 2.5f * Time.deltaTime);
            mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.rotation, oldCamRotation, 2.5f * Time.deltaTime);

            mainCam.transform.parent = GameObject.Find("Player").transform;

            if (mainCam.transform.position == oldCamPosition)
            {
                //re-enable player and door interaction when leaving the door
                stopLooking = false;
                GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
                Camera.main.GetComponent<CameraLook>().canHeadBob = true;
                camLookScript.enabled = true;
            }
        }


        //we would need the state manager at this point to be able to freeze player movement and interaction
        if (!playerCanMove)
        {
            GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;

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

        Camera.main.GetComponent<CameraLook>().canHeadBob = false;
    }


}
