using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
    bool isInverting;
    bool isReInverting;
    [Space]
    [Header("SFX")]
    public SoundEffect CrossSpinSound;
    public SoundEffect CrossDropSound;

    private GameManager gM;
    [HideInInspector] public bool eventTriggered;
    private float startXEuAng;
    private float startYEuAng;  
    private float startXpos;
    private float startYpos;
    private float startZpos;
    private int eventType;
   
    private void Start()
    {
        gM = GameManager.Instance;

        startXEuAng = gameObject.transform.localEulerAngles.x;
        startYEuAng = gameObject.transform.localEulerAngles.y;

        CrossStartPos();
    }
    

    private void Update()
    {       
        invertedCross();
        ReInvertCross();
    }

    private void OnTriggerEnter(Collider other)
    { 
        int randChance = Random.Range(0, 101);
        
      if (other.CompareTag("Player") && randChance <= gM.eventChance && !eventTriggered)
        {
            eventType = UnityEngine.Random.Range(0, 2);
            if (eventType == 0)
            {
                isInverting = true;
                eventTriggered = true;
            }
            else if (eventType == 1)
            {
                FallingCross();
                eventTriggered = true;
            }
            else //(eventType == 2) 
            {
                Debug.Log("No Cross Event");
                eventTriggered = true;
            }
        }

    }

    
    void EnterInteractableState()
    {
       gameObject.AddComponent<BoxCollider>();
        
    }

    void FallingCross()
    {
        gameObject.AddComponent<Rigidbody>();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.back * throwingForce, ForceMode.Impulse);
        gameObject.GetComponent<BoxCollider>().isTrigger = false;
        Destroy(gameObject.GetComponent<BoxCollider>()); 
        EnterInteractableState();
        AudioManager.instance.PlaySound(CrossDropSound, gameObject.transform);
       
    }
    void CrossStartPos()
    {
        startXpos = gameObject.transform.position.x;
        startYpos = gameObject.transform.position.y;
        startZpos = gameObject.transform.position.z;
    }

    void invertedCross()
    {
        if (isInverting)
        {
            rotTime += (Time.deltaTime * rotationSpeed);
            gameObject.transform.localEulerAngles = new Vector3(startXEuAng,startYEuAng, Mathf.Lerp(0, 180, rotTime));
            AudioManager.instance.PlaySound(CrossSpinSound,gameObject.transform);
            Destroy(gameObject.GetComponent<BoxCollider>());
            EnterInteractableState();
        }
    }

    private void ReInvertCross()
    {
        if (isReInverting)
        {
            rotTime += (Time.deltaTime * rotationSpeed);
            gameObject.transform.localEulerAngles = new Vector3(startXEuAng, startYEuAng, Mathf.Lerp(0, -180, rotTime));
            AudioManager.instance.PlaySound(CrossSpinSound, gameObject.transform);
        }
    }

    void ReplaceCross()
    {
        gameObject.transform.position = new Vector3(startXpos,startYpos,startZpos);
    }
        
    public override void Interact()
    {
        
        if (eventType == 0)
        {
            isReInverting = true;
           
        }
        else if (eventType == 1)
        {
           ReplaceCross();
           
        }
        else //(eventType == 2) 
        {
            Debug.Log("No Cross Event");
           
        }

    }


}
