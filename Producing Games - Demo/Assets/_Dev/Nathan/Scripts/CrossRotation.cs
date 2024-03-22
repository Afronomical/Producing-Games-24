using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossRotation : InteractableTemplate
{
    [Header("Cross Variables")]
    public float rotationSpeed;
    private float rotTime;
    private float replaceTime;
    

    [Space]
    [Header("SFX")]
    public SoundEffect CrossSpinSound;
    

    [HideInInspector] public bool eventTriggered;
    [HideInInspector] public bool isCrossReplaced;
    private GameManager gM;

    [HideInInspector] public bool isInverting;
    [HideInInspector] public bool isInverted;
    private float startXEuAng;
    private float startYEuAng;
    private float startZEuAng;     
    
    private void Start()
    {
        isInverting = false;
        isInverted = false;
        gM = GameManager.Instance;
        startXEuAng = gameObject.transform.localEulerAngles.x;
        startYEuAng = gameObject.transform.localEulerAngles.y;
        startZEuAng = gameObject.transform.localEulerAngles.z;

    }

    private void Update()
    {
        invertedCross();
        RevertCross();
    }

    void invertedCross()
    {
        if (isInverting)
        {
            rotTime += (Time.deltaTime * rotationSpeed);
            gameObject.transform.localEulerAngles = new Vector3(startXEuAng, startYEuAng, Mathf.Lerp(0, 180, rotTime));
            AudioManager.instance.PlaySound(CrossSpinSound, gameObject.transform);
        }
    }

    void RevertCross()
    {
        if (isInverted == true)
        {
            replaceTime += (Time.deltaTime * rotationSpeed);
            gameObject.transform.localEulerAngles = new Vector3(startXEuAng, startYEuAng, Mathf.Lerp(gameObject.transform.localEulerAngles.z, startZEuAng, replaceTime));
            AudioManager.instance.PlaySound(CrossSpinSound, gameObject.transform);
        }
    }

    public override void Interact()
    {
        AudioManager.instance.PlaySound(CrossSpinSound, gameObject.transform);
        isInverting = false;
        isInverted = true;        
    }
}
