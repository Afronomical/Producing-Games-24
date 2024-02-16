using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class JumpShadow : MonoBehaviour
{
    public int eventChance = 15;
    public float successResetSecTime = 100;
    public float failResetSecTime = 40;
    public GameObject shadowManObj;
    public float followDistance = 5;
    public float heightOffset;
    private bool canPlay = true;
    private GameObject playerObj;
    private CharacterController characterController;
    private bool playEvent;
    



    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        characterController = playerObj.GetComponent<CharacterController>();
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
        if ((Random.Range(1, eventChance) == 1) && canPlay)
        {
            playEvent = true;
            shadowManObj.SetActive(true);
            canPlay = false;
            StartCoroutine(EventResetTimer(successResetSecTime));
        }
        else
        {
            canPlay = false;
            StartCoroutine(EventResetTimer(failResetSecTime));
        }
    }
    private IEnumerator EventResetTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPlay = true;
    }
    
    private void Event()
    {
        if (characterController.velocity.magnitude > 0)
        {
            Vector3 followPosition = playerObj.transform.position - playerObj.transform.forward * followDistance;
            followPosition.y -= heightOffset;
            shadowManObj.transform.LookAt(playerObj.transform, Vector3.up);
            shadowManObj.transform.position = followPosition;//Vector3.Lerp(shadowManObj.transform.position, followPosition, Time.deltaTime);
        }
    }
}
