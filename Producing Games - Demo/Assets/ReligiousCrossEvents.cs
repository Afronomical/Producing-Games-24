using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Rendering;

public class ReligiousCrossEvents : MonoBehaviour
{
    public GameObject Cross;
    
    public float rotSpeed;
    float rotTime;
    public Rigidbody rb;
    float timedelta;
    bool isRotating;
    private void Start()
    {
        //isRotating = false;
        rb.useGravity = false;
    }

    private void Update()
    {
        if (isRotating)
        {
            
            Debug.Log(timedelta.ToString());
            if(timedelta <= 1)
            {
                timedelta += Time.deltaTime;
                Cross.transform.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(0, 180, timedelta));
                //Cross.transform.Rotate(0,0,Mathf.Lerp(0,5.35f,timedelta));
                Debug.Log(Cross.transform.rotation);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isRotating=true;
        //invertedCross();
        //FallingCross();
    }


    void FallingCross()
    {
        //Cross.transform.position += ;
        rb.useGravity = true;
    }

    void invertedCross()
    {              
       Cross.transform.Rotate(Vector3.forward, 180f);
    }

}
