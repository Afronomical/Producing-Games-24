using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;


/// <summary>
/// Code for the shadow figure jumpscare. Event hase a chance to play after trigger is hit, plays a timer until the next event can happen
/// shadow will attach to the back of the player and stay behind them during movement. Once shadow is on the edge of your screen the cam will look at the shadow and play specific fx
/// </summary>
public class JumpShadow : MonoBehaviour
{
    [Header("Event Time Vars")]
    public bool playTest;
    public float successResetSecTime = 100;
    public float failResetSecTime = 40;
    [Header("Refs")]
    public GameObject shadowBaseObj;
    public GameObject shadowBodyObj;
    public GameObject postProcVolRef;
    private PulsePostProc pulseProcScript;
    [Header("Shadow Settings")]
    public float shadowRotSpeed = 1f;
    [Range(-30f, 30f)] public float fovLeniency = 0;
    public float headTurnSpeed = 1;
    public float jumpScareFOV = 50;
    public float jumpScareFOVSpeed = 1;
    public SoundEffect jumpScareSound;
    private bool canPlay = true;
    private GameObject playerObj;
    private CharacterController characterController;
    private Camera playerCam;
    private bool playEvent;
    private float origFOV;
    private float fovTime;
    private bool jumpScareOnce = true;
    private bool reverseFOV;



    void Start()
    {
        playerObj = GameManager.Instance.player;
        characterController = playerObj.GetComponent<CharacterController>();
        playerCam = Camera.main; //playerObj.GetComponent<Camera>();
        origFOV = playerCam.fieldOfView;
        pulseProcScript = postProcVolRef.GetComponent<PulsePostProc>();
    }

    
    void Update()
    {
        if (playEvent)
        {
            Event();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (((Random.Range(1, 100) <= GameManager.Instance.eventChance) || playTest) && canPlay) //play event if chance hits, else event starts after delay
            {
                playEvent = true;
                shadowBaseObj.SetActive(true);
                shadowBaseObj.transform.rotation = playerObj.transform.rotation;
                canPlay = false;
                reverseFOV = false;
                StartCoroutine(EventResetTimer(successResetSecTime));
            }
            else
            {
                canPlay = false; //event fail
                StartCoroutine(EventResetTimer(failResetSecTime));
            }
        }
        
    }
    private IEnumerator EventResetTimer(float delay) //timer for event reset
    {
        yield return new WaitForSeconds(delay);
        canPlay = true;
    }
    
    private void Event()  //main event 
    {
        if (characterController.velocity.magnitude > 0)  //slowly lerps the shadow behind the player if they are moving
        {
            
            
            shadowBaseObj.transform.position = playerObj.transform.position;

            shadowBaseObj.transform.rotation = Quaternion.Lerp(shadowBaseObj.transform.rotation, playerObj.transform.rotation, Time.deltaTime * shadowRotSpeed);

            
        }
        Vector3 camRelY = shadowBodyObj.transform.position;
        camRelY.y = playerCam.transform.position.y;
        Vector3 camRelLoc = (camRelY - playerCam.transform.position).normalized;
        
        if (Vector3.Angle(playerCam.transform.forward, camRelLoc) < (origFOV + fovLeniency)) //detects if shadow is within visible fov, then rotates player to look and plays jumpscare events
        {
            Vector3 direction = camRelY - playerObj.transform.position;
            direction.y = 0;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            playerObj.transform.rotation = Quaternion.Lerp(playerObj.transform.rotation, toRotation, headTurnSpeed * Time.deltaTime);
            fovTime += Time.deltaTime * jumpScareFOVSpeed;
            
            if (reverseFOV)
            {
                playerCam.fieldOfView = Mathf.Lerp(jumpScareFOV, origFOV, fovTime > 1 ? 1 : fovTime);
            }
            else
            {
                
                playerCam.fieldOfView = Mathf.Lerp(origFOV, jumpScareFOV, fovTime > 1 ? 1 : fovTime);
            }
            
            
            if (jumpScareOnce)
            {
                jumpScareOnce = false;
                AudioManager.instance.PlaySound(jumpScareSound, playerObj.transform);
                StartCoroutine(AfterEvent());
            }
        }
    }

    private IEnumerator AfterEvent() //resetting shit after jumpscare
    {
        postProcVolRef.SetActive(true);
        StartCoroutine(pulseProcScript.Pulsate());
        
        yield return new WaitForSeconds(0.7f);
        shadowBaseObj.SetActive(false);
        //yield return new WaitForSeconds(0.5f);

        fovTime = 0;
        reverseFOV = true;
        yield return new WaitForSeconds(0.6f);
        playerCam.fieldOfView = origFOV;
        shadowBaseObj.transform.localPosition = Vector3.zero;
        
        playEvent = false;
        jumpScareOnce = true;
    }

    private void postProcVol()
    {
        postProcVolRef.transform.position = Camera.main.transform.position;
        postProcVolRef.SetActive(true);
    }
}
