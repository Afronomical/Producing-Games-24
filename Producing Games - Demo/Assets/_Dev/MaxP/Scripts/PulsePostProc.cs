using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// This script allows a post process volumes effect to pulsate between a set maximum and minimum within a set amount of time.
/// The while loops are timers to update the lerps every frame
/// </summary>

public class PulsePostProc : MonoBehaviour
{

    //private Volume vol;
    private float elapsedTime;
    private float timePerPulse;
    private bool toMax;
    private float firstWeight;
    //private static Volume vol;

    bool goToPlayer;

    [Range(0f, 1f)] public float maxWeight;
    [Range(0f, 1f)] public float minWeight;
    public float pulseTotLength;
    public int pulses;


    void Start()
    {
        //vol = GetComponent<Volume>();
    }

    // Update is called once per frame
    void Update()
    {

        if (goToPlayer)
        {
            gameObject.transform.position = Camera.main.transform.position; //places the volume on the players head. Should make this as an option later
        }
    }

    

    public IEnumerator Pulsate()
    {
        goToPlayer = true;
        timePerPulse = pulseTotLength / pulses;
        
        for (int i = 0; i < pulses; i++) //the main pulse loop, loops for each pulse
        {
            firstWeight = GetComponent<Volume>().weight;
            toMax = !toMax; //"toMax" swaps every loop
            while (elapsedTime < timePerPulse) //lerps post processing weight to min or max depending on if "toMax" is true or false
            {
                GetComponent<Volume>().weight = Mathf.Lerp(firstWeight, toMax ? maxWeight : minWeight, elapsedTime / timePerPulse);
                elapsedTime += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
                
            }
            elapsedTime = 0f;
            
            
        }
        
        firstWeight = GetComponent<Volume>().weight;
        elapsedTime = 0f;
        while (elapsedTime < timePerPulse) //lerping back to 0 post process weight
        {
            GetComponent<Volume>().weight = Mathf.Lerp(firstWeight, 0, elapsedTime / timePerPulse);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            
        }
        GetComponent<Volume>().weight = 0;
        goToPlayer = false;
        gameObject.SetActive(false);
    }
}
