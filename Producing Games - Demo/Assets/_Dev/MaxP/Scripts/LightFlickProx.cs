using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickProx : MonoBehaviour
{
    private Flashlight flashScript;
    private GameObject player;
    private Vector3 thisLoc;
    private Vector3 playerLoc;
    private float distance;
    private float origFlickerLightPower;
    private int origFlickerCount;
    private float origFlickerLightDiff;
    bool resetFlicker;
    public float maxDist = 10.0f;
    public float minDist = 1.0f;
    [Range(-0.5f, 1.0f)] public float maxBrightAtMinRange = -0.26f;
    [Range(0.0f, 0.2f)] public float maxDiffAtMinRange = 0.1f;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        flashScript = player.GetComponent<Flashlight>();
        origFlickerLightPower = flashScript.flickerLightPower;
        origFlickerCount = flashScript.avgFlickerCount;
        origFlickerLightDiff = flashScript.flickerLightPowerDiff;
    }

    
    void Update()
    {
        thisLoc = transform.position;
        playerLoc = player.transform.position;
        distance = Vector3.Distance(thisLoc, playerLoc);
        
        if (distance < maxDist && !(distance < minDist)) //&& distance > minDist
        {
            resetFlicker = true;
            flashScript.shouldReset = false;
            StartCoroutine(flashScript.Flickering());
            flashScript.flickerLightPower = Mathf.Lerp(maxBrightAtMinRange, origFlickerLightPower, distance / maxDist);
            flashScript.flickerLightPowerDiff = Mathf.Lerp(maxDiffAtMinRange, origFlickerLightDiff, distance / maxDist);
            flashScript.avgFlickerCount = 7;
        }
        else if (distance < minDist)
        {
            StartCoroutine(flashScript.Flickering());
            //Debug.Log("mghbsaddjkhfbaibaigbakladgjhbgk,");
        }
        else if (resetFlicker)
        {
            flashScript.avgFlickerCount = origFlickerCount;
            resetFlicker = false;
            flashScript.shouldReset = true;
        }
        
    }
}
