using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectDoor : InteractableTemplate
{
    public Camera mainCam;
    public Vector3 camRotation;// = new Vector3(0,0,0);
    public Vector3 camPosition;// = new Vector3(-1.54999995f, 0.310000002f, 7.25f);
    public Quaternion oldCamRotation;
    public Vector3 oldCamPosition;

    public override void Interact()
    {
        Debug.Log("*** Interacting with door ***");
        mainCam.transform.parent = null;
        
        oldCamPosition = mainCam.transform.position;
        oldCamRotation = mainCam.transform.rotation;

        mainCam.transform.position = Vector3.Lerp(mainCam.transform.localPosition, camPosition, 3 * Time.deltaTime);
        mainCam.transform.rotation = Quaternion.Lerp(mainCam.transform.localRotation, Quaternion.Euler(camRotation), 3 * Time.deltaTime);
    }
}
