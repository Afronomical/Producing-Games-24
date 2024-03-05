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
    
    [Space]
    [Header("SFX")]
    public SoundEffect CrossSpinSound;
    public SoundEffect CrossDropSound;

   
    [HideInInspector] public bool eventTriggered; 
    private GameManager gM;
    bool isInverting;
    bool isReInverting;
    private float startXEuAng;
    private float startYEuAng;
    private float startZEuAng;
    private float startXpos;
    private float startYpos;
    private float startZpos;
    private int eventType;
    private bool isReplaced;

    private void Start()
    {
        isInverting = false;
        isReInverting = false;
        isReplaced = false;
        gM = GameManager.Instance; 
        CrossStartPos();           
    }   

    private void Update()
    {
        invertedCross();
    }

    public void TriggerEvent()
    {
        eventType = UnityEngine.Random.Range(0, 2);
        if (eventType == 0)
        {
            isInverting = true;
            eventTriggered = true;           
        }
        else if(eventType == 1)
        {
            FallingCross();
            eventTriggered = true;            
        }
        else //(eventType == 2)
        {
            Debug.Log("No Cross Event Found");
        }
        
    }
    void EnterInteractableState()
    {
        Debug.Log("Cross Interactable");
    }    
    void CrossStartPos()
    {
        startXpos = gameObject.transform.position.x;
        startYpos = gameObject.transform.position.y;
        startZpos = gameObject.transform.position.z;
        startXEuAng = gameObject.transform.localEulerAngles.x;
        startYEuAng = gameObject.transform.localEulerAngles.y;
        startZEuAng = gameObject.transform.localEulerAngles.z;
    }   
    
    public void FallingCross()
    {
        EnterInteractableState();
        gameObject.AddComponent<Rigidbody>();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.back * throwingForce, ForceMode.Impulse);
        AudioManager.instance.PlaySound(CrossDropSound, gameObject.transform);
    }
   
    void invertedCross()
    {
        if (isInverting)
        {
            EnterInteractableState();
            rotTime += (Time.deltaTime * rotationSpeed);
            gameObject.transform.localEulerAngles = new Vector3(startXEuAng,startYEuAng, Mathf.Lerp(0, 180, rotTime));
            AudioManager.instance.PlaySound(CrossSpinSound,gameObject.transform);
            //isInverting = false;
        }
    } 
    public override void Interact()
    {
        Debug.Log("....");
        Destroy(gameObject.GetComponent<Rigidbody>());
        gameObject.transform.position = new Vector3(startXpos, startYpos, startZpos);
        gameObject.transform.localEulerAngles = new Vector3(startXEuAng, startYEuAng, startZEuAng);
        isInverting = false;
        isReplaced = true;
    }
    
    
    /* void ReplaceCross()
    {
        Destroy(gameObject.GetComponent<Rigidbody>());
        gameObject.transform.position = new Vector3(startXpos, startYpos, startZpos);
        gameObject.transform.localEulerAngles = new Vector3(startXEuAng, startYEuAng, startZEuAng);
        isReplaced = true;
    }*/
   /* private void ReInvertCross()
    {
        if (isReInverting)
        {
            rotTime += (Time.deltaTime * rotationSpeed);
            gameObject.transform.localEulerAngles = new Vector3(startXEuAng, startYEuAng, Mathf.Lerp(0, -180, rotTime));
            AudioManager.instance.PlaySound(CrossSpinSound, gameObject.transform);
            isReInverting = false;
        }
    }*/

    


}
