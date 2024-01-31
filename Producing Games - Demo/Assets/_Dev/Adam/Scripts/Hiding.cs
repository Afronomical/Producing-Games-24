using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding : MonoBehaviour
{
    public GameObject HidingSpot;
    public GameObject InsideSpot;
    public Camera cam;
    bool goInHidingSpot;
    bool GoIn;



    private void Start()
    {
        
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
            goInHidingSpot = true; 
        
        

        if(goInHidingSpot)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, HidingSpot.transform.position, 2.5f * Time.deltaTime);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, HidingSpot.transform.rotation, 5);
            if (cam.transform.position == HidingSpot.transform.position)
                GoIn = true;

        }

        if (cam.transform.position == HidingSpot.transform.position)
        {
            Debug.Log("Plz work");
            goInHidingSpot=false;
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, InsideSpot.transform.position, 2.5f * Time.deltaTime);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, InsideSpot.transform.rotation, 2.5f * Time.deltaTime);
        }
    }
}
