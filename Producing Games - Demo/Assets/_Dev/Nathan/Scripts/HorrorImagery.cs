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
    [Range(0f, 75f)] public float DetectionRange = 0;


    private bool canSpawn = true;
    private Vector3 ImgSpawnPos;
    private Vector3 ImgPos;
    private Vector3 ImgCurrentPos;
    private Camera playerCam;
    private float playerFOV;

    void Start()
    {
        HorrorImage.SetActive(false);
        playerCam = Camera.main;
        playerFOV = playerCam.fieldOfView;
    }

    void Update()
    {
        ImgPos = HorrorImage.transform.position;
        ImgPos.y = playerCam.transform.position.y;
        ImgCurrentPos = (ImgPos - playerCam.transform.position).normalized;
        
        //finds and angle between the player and the img, if in the detection range then img will disappear
        if (Vector3.Angle(playerCam.transform.forward, ImgCurrentPos) < (playerFOV - DetectionRange)) 
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
            if (canSpawn && (Random.Range(1, 101) <= GameManager.Instance.eventChance)) 
            {
                SpawnImagery();
                canSpawn = false;
                StartCoroutine(ResetTimer(EventResetTimer));
            }
            else
            {
                canSpawn = false;
                StartCoroutine(ResetTimer(EventFailedRestTimer));
            }
        }
    }

    private void SpawnImagery() //spawns Imagery in random location set out from the array
    {
        int imageryArr = Random.Range(0, ImagerySpawnLocations.Length);
        ImgSpawnPos = ImagerySpawnLocations[imageryArr].transform.position;
       
        HorrorImage.SetActive(true);
        HorrorImage.transform.position = ImgSpawnPos;
    }
    private IEnumerator ResetTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        canSpawn = true;
    }
    private void ResetImagery()
    {
        HorrorImage.SetActive(false);       
    }  
}
