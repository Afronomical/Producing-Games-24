using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


// Script is subject to change as the cross will have to be an interactable item so that it can be put back on wall and turned right way up
public class CrossEvents : MonoBehaviour
{
    [Header("Cross Item Components")]
    public GameObject Cross;
    //public Rigidbody rb;
    [Space]
    [Header("Cross Variables")]
    public float rotationSpeed;
    private float rotTime;
    public float throwingForce;
    bool isRotating;
    bool isTriggered;

    private void Start()
    {
       
        //Cross.GetComponent<Rigidbody>().SetActive(false);
        //rb.useGravity = false;
        isTriggered = false;
    }

    private void Update()
    {
        invertedCross();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isTriggered == false)
        {
            int eventType = UnityEngine.Random.Range(0, 2);
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
        Cross.AddComponent<Rigidbody>();
        Rigidbody rb = Cross.GetComponent<Rigidbody>();  
        rb.AddRelativeForce(Vector3.back * throwingForce, ForceMode.Impulse);        
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
