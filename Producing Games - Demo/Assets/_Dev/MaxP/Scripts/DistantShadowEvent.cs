using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.Rendering;

/// <summary>
/// spawns a shadow man at a point randomly selected out of the array.
/// shadow man will then move away if player gets too close.
/// Then will disable shadow man and event will evently be able to be retriggered 
/// </summary>

public class DistantShadowEvent : MonoBehaviour
{
    //public int eventChance = 15;
    public bool testing;
    public float successResetSecTime = 100;
    public float failResetSecTime = 40;
    public float moveAwaySpeed = 100;
    public float moveAwayDistance = 15;
    public bool moveToPlayer;
    public GameObject[] shadowSpawnLocations;
    public GameObject[] shadowMoveToLocations;
    public GameObject shadowManObj;
    public SoundEffect moveSound;
    private int spawnLocIndex;
    private bool canPlay = true;
    private GameObject playerObj;
    private Vector3 standLoc;
    private Vector3 moveLoc;
    private bool shouldMove;
    private bool startMove;
    private float moveTime;
    private Camera playerCam;
    private float origFOV;
    private bool soundOnce = true;
    [Range(-30f, 30f)] public float fovLeniency = 0;


    void Start()
    {
        playerObj = GameManager.Instance.player;
        playerCam = Camera.main;
        origFOV = playerCam.fieldOfView;
    }

    
    void Update()
    {
        if (shouldMove)
        {
            
            float distance = Vector3.Distance(shadowManObj.transform.position, playerObj.transform.position);
            Vector3 camRelY = shadowManObj.transform.position;
            camRelY.y = playerCam.transform.position.y;
            Vector3 camRelLoc = (camRelY - playerCam.transform.position).normalized;
            if ((Vector3.Angle(playerCam.transform.forward, camRelLoc) < (origFOV + fovLeniency)) && distance < moveAwayDistance) //moves shadow man if player is within specified distance
            {
                startMove = true;
            }
            if (startMove)
            {
                PlaySoundOnce();
                if (moveToPlayer)
                {
                    Vector3 move = Vector3.Lerp(shadowManObj.transform.position, playerObj.transform.position, Time.deltaTime * moveAwaySpeed);
                    move.y = shadowManObj.transform.position.y;
                    shadowManObj.transform.position = move;
                    moveTime += Time.deltaTime;
                }
                else //move to alternate spot instead of moving to player
                {
                    shadowManObj.transform.position = Vector3.Lerp(shadowManObj.transform.position, moveLoc, Time.deltaTime * moveAwaySpeed);
                    moveTime += Time.deltaTime;
                }
                if (distance < 3 || moveTime > 5) //end the event, reset everything suuuuuurely
                {
                    ResetShadow();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) //play event if chance hits, else event starts after delay
    {
        if (other.CompareTag("Player"))
        {
            if (canPlay && testing ? true : (Random.Range(1, 100) <= GameManager.Instance.eventChance)) //(Random.Range(1, 100) <= GameManager.Instance.eventChance) && 
            {
                ShadowSpawnEvent();
                canPlay = false;
                StartCoroutine(EventResetTimer(successResetSecTime));
            }
            else
            {
                canPlay = false;
                StartCoroutine(EventResetTimer(failResetSecTime));
            }
        }
        
    }

    private void ShadowSpawnEvent() //spawns shadow in place slelected at random out of locations stored in an array
    {
        int shadowArraySlot = Random.Range(0, shadowSpawnLocations.Length);
        standLoc = shadowSpawnLocations[shadowArraySlot].transform.position;
        moveLoc = shadowMoveToLocations[shadowArraySlot].transform.position;
        shadowManObj.SetActive(true);
        shadowManObj.transform.position = standLoc;
        shadowManObj.transform.LookAt(playerObj.transform, Vector3.up);
        shouldMove = true;

    }
    private IEnumerator EventResetTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPlay = true;
    }

    private void ResetShadow()
    {
        shadowManObj.SetActive(false);
        startMove = false;
        shouldMove = false;
        moveTime = 0;
        soundOnce = true;
    }

    private void PlaySoundOnce()
    {
        if (soundOnce)
        {
            AudioManager.instance.PlaySound(moveSound, playerObj.transform);
            soundOnce = false;
        }
        
    }
    
}
