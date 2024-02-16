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
    public float lerpSpeed = 1f;
    private bool canPlay = true;
    private GameObject playerObj;
    private CharacterController characterController;
    private bool playEvent;
    private float rotAngle;
    



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
        Debug.Log(playerObj.transform.localEulerAngles.y);
        //Debug.Log(playerObj.transform.rotation.y);
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
            //Vector3 followPosition = playerObj.transform.position - playerObj.transform.forward * followDistance;
            //followPosition.y -= heightOffset;
            //shadowManObj.transform.LookAt(playerObj.transform, Vector3.up);
            //shadowManObj.transform.position = followPosition;//Vector3.Lerp(shadowManObj.transform.position, followPosition, Time.deltaTime);
            if (playerObj.transform.rotation.y > 0)
            {
                rotAngle = (Mathf.Abs(playerObj.transform.rotation.y)) * 180;
                
                rotAngle += 180;
                
            }
            else
            {
                
            }
            //rotAngle = (Mathf.Abs(playerObj.transform.rotation.y)) * 360;
            shadowManObj.transform.position = playerObj.transform.position;
            //shadowManObj.transform.localEulerAngles = new Vector3(0, (Mathf.Lerp(shadowManObj.transform.rotation.y, playerObj.transform.rotation.y, Time.deltaTime * lerpSpeed)), 0);
            shadowManObj.transform.rotation = Quaternion.Lerp(shadowManObj.transform.rotation, playerObj.transform.rotation, Time.deltaTime * lerpSpeed);
        }
    }
}
