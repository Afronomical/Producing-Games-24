using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDeactivator : MonoBehaviour
{
     Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.enabled = false;
    }

   
}
