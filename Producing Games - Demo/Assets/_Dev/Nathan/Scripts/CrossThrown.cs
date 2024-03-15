using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossThrown : InteractableTemplate
{
    [Header("Cross Variables")]
    
    public float throwingForce;
    [Space]
    [Header("SFX")]
    public SoundEffect CrossThrownSound;

    
    private float startXEuAng;
    private float startYEuAng;
    private float startZEuAng;
    private float startXpos;
    private float startYpos;
    private float startZpos;
    private GameManager gM;
    CrossThrowTrigger trigger;
    
    

    void Start()
    {
        gM = GameManager.Instance;       
        CrossStartPos();       
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
    public void resetCrossPos()
    {        
        gameObject.transform.position = new Vector3(startXpos, startYpos, startZpos);
        gameObject.transform.localEulerAngles = new Vector3(startXEuAng, startYEuAng, startZEuAng);
    }   
    
    public void FallingCross()
    {
        gameObject.AddComponent<Rigidbody>();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.back * throwingForce, ForceMode.Impulse);
        AudioManager.instance.PlaySound(CrossThrownSound, gameObject.transform);
    }

    public override void Interact()
    {
        Destroy(gameObject.GetComponent<Rigidbody>());
        resetCrossPos();  
    }
}
