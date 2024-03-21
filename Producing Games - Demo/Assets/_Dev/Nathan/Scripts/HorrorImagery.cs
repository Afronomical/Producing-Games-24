using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorImagery : MonoBehaviour
{
    [Header("Horror Image")]
    public GameObject HorrorImage;
    [Space]
    [Header("Imagery Spawn Locations")]
    public GameObject[] ImagerySpawnLocations;
    [Space]
    [Header("Event Reset Timers")]
    public float EventResetTimer = 100;
    public float EventFailedRestTimer = 40;


    private bool canPlay = true;
    private GameObject playerObj;
    private Vector3 standLoc;    
    private bool hasBeenSeen;    
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
        if (hasBeenSeen)
        {
            float distance = Vector3.Distance(HorrorImage.transform.position, playerObj.transform.position); //Returns distance between imagery and player
            
            Vector3 camRelY = HorrorImage.transform.position;
            camRelY.y = playerCam.transform.position.y;
            Vector3 camRelLoc = (camRelY - playerCam.transform.position).normalized;

            if (Vector3.Angle(playerCam.transform.forward, camRelLoc) < (origFOV + fovLeniency))
            {
                ResetImagery();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canPlay && (Random.Range(1, 101) <= GameManager.Instance.eventChance)) 
            {
                SpawnImagery();
                canPlay = false;
                StartCoroutine(ResetTimer(EventResetTimer));
            }
            else
            {
                canPlay = false;
                StartCoroutine(ResetTimer(EventFailedRestTimer));
            }
        }
    }

    private void SpawnImagery() //spawns Imagery in random location set out from the array
    {
        int imageryArr = Random.Range(0, ImagerySpawnLocations.Length);
        standLoc = ImagerySpawnLocations[imageryArr].transform.position;
       
        HorrorImage.SetActive(true);
        HorrorImage.transform.position = standLoc;
        HorrorImage.transform.LookAt(playerObj.transform, Vector3.up);       
    }
    private IEnumerator ResetTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPlay = true;
    }

    private void ResetImagery()
    {
        HorrorImage.SetActive(false);       
    }

   

}
