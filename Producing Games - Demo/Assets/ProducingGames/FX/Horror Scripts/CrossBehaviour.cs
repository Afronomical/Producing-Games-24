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
    public bool isInverting;
    bool isReInverting;
    [Space]
    [Header("SFX")]
    public SoundEffect CrossSpinSound;
    public SoundEffect CrossDropSound;
    

    private GameManager gM;
    [HideInInspector] public bool eventTriggered;
    private float startXEuAng;
    private float startYEuAng;
    private float startZEuAng;
    private float startXpos;
    private float startYpos;
    private float startZpos;
    private int eventType;
   

    private void Start()
    {
        gM = GameManager.Instance;

        startXEuAng = gameObject.transform.localEulerAngles.x;
        startYEuAng = gameObject.transform.localEulerAngles.y;
        startZEuAng = gameObject.transform.localEulerAngles.z;
                
        CrossStartPos();
    }
    

    private void Update()
    {       
        invertedCross();
        ReInvertCross();
    }

    public void TriggerEvent()
    {
        eventType = UnityEngine.Random.Range(0, 2);
        if (eventType == 0)
        {
            isInverting = true;
            eventTriggered = true;
            EnterInteractableState();
        }
        else if (eventType == 1)
        {
            FallingCross();
            eventTriggered = true;
            EnterInteractableState();
        }
        else //(eventType == 2) 
        {
            Debug.Log("No Cross Event");
            eventTriggered = true;
        }
    }

    
    void EnterInteractableState()
    {
        Debug.Log("Cross Interactable");
    }

    public void FallingCross()
    {
        gameObject.AddComponent<Rigidbody>();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.back * throwingForce, ForceMode.Impulse);
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
        Destroy(gameObject.GetComponent<Rigidbody>());
        gameObject.transform.position = new Vector3(startXpos,startYpos,startZpos);
        gameObject.transform.localEulerAngles = new Vector3(startXEuAng, startYEuAng, startZEuAng);
    }
        
    public override void Interact()
    {
        Debug.Log("....");
        if (gameObject.transform.localEulerAngles.z >= startZEuAng)
        {
            isReInverting = true;           
        }
        else
        {           
           ReplaceCross();           
        }
    }


}
