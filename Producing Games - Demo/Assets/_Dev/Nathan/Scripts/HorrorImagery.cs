using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para> Written By: Nathan Jowett  </para>
/// Moderated By: Lucian Dusciac
/// <para> Demonic image will spawn at random location from an array, if player looks at the image it will disappear. 
/// Detection range is key, if it is equal to player FOV then you will have to look right at object for it to disappear.
/// </summary>


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
    [Space]
    [Header("Spotting Distance, this value will minus from player FOV")]
    [Range(0f, 90f)] public float DetectionRange = 0;


    private bool canPlay = true;
    private GameObject playerObj;
    private Vector3 imgPos;    
    private Camera playerCam;
    private float origFOV;

    void Start()
    {
        HorrorImage.SetActive(false);
        playerObj = GameManager.Instance.player;
        playerCam = Camera.main;
        origFOV = playerCam.fieldOfView;
    }


    void Update()
    {
        Vector3 ImgLoc = HorrorImage.transform.position;
        ImgLoc.y = playerCam.transform.position.y;
        Vector3 ImgCurrentLoc = (ImgLoc - playerCam.transform.position).normalized;
        
        //finds and angle between the player and the img, if in the detection range then img will disappear
        if (Vector3.Angle(playerCam.transform.forward, ImgCurrentLoc) < (origFOV - DetectionRange)) 
        {
            ResetImagery();
        }
    }

    /// <summary>
    /// Slight difference to other events, this can happen multiple times every shift. more likely if player sanity is lower
    /// </summary>
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
        imgPos = ImagerySpawnLocations[imageryArr].transform.position;
       
        HorrorImage.SetActive(true);
        HorrorImage.transform.position = imgPos;   
    }
    private IEnumerator ResetTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        canPlay = true;
    }

    private void ResetImagery()
    {
        HorrorImage.SetActive(false);       
    }

   

}
