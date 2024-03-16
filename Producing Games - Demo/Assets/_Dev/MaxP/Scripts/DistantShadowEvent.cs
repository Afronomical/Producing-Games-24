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
    public int eventChance = 15;
    public float successResetSecTime = 100;
    public float failResetSecTime = 40;
    public float moveAwaySpeed = 100;
    public float moveAwayDistance = 15;
    public GameObject[] shadowSpawnLocations;
    public GameObject shadowManObj;
    private int spawnLocIndex;
    private bool canPlay = true;
    private GameObject playerObj;
    private Vector3 standLoc;
    private bool shouldMove;
    private bool startMove;
    private float moveTime;
    private Camera playerCam;
    private float origFOV;
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
            if (Vector3.Angle(playerCam.transform.forward, camRelLoc) < (origFOV + fovLeniency)) //moves shadow man if player is within specified distance
            {
                startMove = true;
            }
            if (startMove)
            {
                Vector3 move = Vector3.Lerp(shadowManObj.transform.position, playerObj.transform.position, Time.deltaTime * moveAwaySpeed);
                move.y = shadowManObj.transform.position.y;
                shadowManObj.transform.position = move;
                moveTime += Time.deltaTime;
                if (distance < 3 || moveTime > 5) //end the event, reset everything suuuuuurely
                {
                    shadowManObj.SetActive(false);
                    startMove = false;
                    shouldMove = false;
                    moveTime = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) //play event if chance hits, else event starts after delay
    {
        if (other.CompareTag("Player"))
        {
            if (canPlay) //(Random.Range(1, 100) <= GameManager.Instance.eventChance) && 
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
        standLoc = shadowSpawnLocations[Random.Range(0, shadowSpawnLocations.Length)].transform.position;
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

    
}
