using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para> Written By: Nathan Jowett  </para>
/// Moderated By: Lucian Dusciac
/// <para> Once the player has entered the objects trigger the object will either rotate 180 degrees or be thrown from its place, 
/// forces are set in the inspector.</para> 
/// </summary>


public class CrossBehaviour : InteractableTemplate
{
    
    [Header("Cross Variables")]
    public float rotationSpeed;
    private float rotTime;
    public float throwingForce;
    bool isRotating;
    bool isTriggered;
    [Space]
    [Header("SFX")]
    public SoundEffect CrossSpinSound;
    public SoundEffect CrossDropSound;

   

    private void Start()
    {
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

    /// <summary>
    /// 
    /// </summary>
    void FallingCross()
    {
        gameObject.AddComponent<Rigidbody>();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.back * throwingForce, ForceMode.Impulse);
        gameObject.GetComponent<BoxCollider>().isTrigger = false;
        Destroy(gameObject.GetComponent<BoxCollider>());
        AudioManager.instance.PlaySound(CrossDropSound, gameObject.transform);
        EnterInteractableState();
    }


    void invertedCross()
    {
        if (isRotating)
        {
            rotTime += (Time.deltaTime * rotationSpeed);
            gameObject.transform.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(0, 180, rotTime));
            AudioManager.instance.PlaySound(CrossSpinSound,gameObject.transform);
        }
    }

    void EnterInteractableState()
    {
       gameObject.AddComponent<BoxCollider>();
        
    }

    public override void Interact()
    {
        //add implementation for ammending cross
    }


}
