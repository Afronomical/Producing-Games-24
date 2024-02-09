using System;
using UnityEngine;
using UnityEngine.Rendering;


public class ReligiousCrossEvents : MonoBehaviour
{
    public GameObject Cross;
    public Rigidbody rb;
    public float rotationSpeed;
    private float rotTime;
    public float throwingForce;
    bool isRotating;
    bool isTriggered;

    private void Start()
    {
        rb.useGravity = false;
        isTriggered = false;
    }

    private void Update()
    {
        invertedCross();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isTriggered == false)
        {
            int eventType = UnityEngine.Random.Range(0,2);
            if (eventType == 0) 
            {
                isRotating = true;
                isTriggered = true;
            }
            else if (eventType == 1) 
            {
                FallingCross();
                isTriggered = true;
            }
            else //(eventType == 2) 
            {
                Debug.Log("No Cross Event");
                isTriggered = true;
            }
        }
        
    }


    void FallingCross()
    {
        rb.useGravity = true;
        rb.AddForce(Vector3.back * throwingForce, ForceMode.Impulse);
        //rb.isKinematic = true;
    }

    void invertedCross()
    {
        if (isRotating)
        {
            rotTime += (Time.deltaTime * rotationSpeed);
            Cross.transform.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(0, 180, rotTime));
        }
    }

}
