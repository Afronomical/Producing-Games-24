using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LowLevel;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

/// <summary>
/// basically is a copy of proxflicker script but does it to each light in the trigger box
/// </summary>

public class DemonLightFlicker : MonoBehaviour
{
    private List<InteriorLampFlicker> lights;
    
    private float origFlickerLightPower;
    private int origFlickerCount;
    private float origFlickerLightDiff;
    private InteriorLampFlicker lightScript;
    [Range(-0.5f, 1.0f)] public float maxBrightAtMinRange = -0.26f;
    [Range(0.0f, 0.2f)] public float maxDiffAtMinRange = 0.1f;
    public float maxDist;
    public float minDist;

    void Start()
    {
        lights = new List<InteriorLampFlicker>();
    }


    void Update()
    {
        if (lights != null)
        {
            foreach (InteriorLampFlicker light in lights) //for each light apply flicker
            {
                lightScript = light;
                Vector3 thisLoc = transform.position;
                Vector3 lightLoc = light.transform.position;
                float distance = Vector3.Distance(thisLoc, lightLoc);
                Debug.Log(distance);
                bool resetFlicker = false;
                origFlickerLightPower = lightScript.flickerLightPower;
                origFlickerCount = lightScript.avgFlickerCount;
                origFlickerLightDiff = lightScript.flickerLightPowerDiff;
                if (distance < maxDist && !(distance < minDist)) //&& distance > minDist
                {
                    resetFlicker = true;

                    StartCoroutine(lightScript.Flickering());
                    lightScript.flickerLightPower = Mathf.Lerp(maxBrightAtMinRange, origFlickerLightPower, distance / maxDist);
                    lightScript.flickerLightPowerDiff = Mathf.Lerp(maxDiffAtMinRange, origFlickerLightDiff, distance / maxDist);
                    lightScript.avgFlickerCount = 7;
                }
                else if (distance < minDist)
                {
                    StartCoroutine(lightScript.Flickering());
                }
                else if (resetFlicker)
                {
                    lightScript.avgFlickerCount = origFlickerCount;
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other) //adds anything with "Light" tag to the list
    {
        if (other.CompareTag("Light"))
        {
            lights.Add(other.GetComponent<InteriorLampFlicker>());
        }
    }

    private void OnTriggerExit(Collider other) //removes anything with "Light" tag from the list
    {
        if (other.CompareTag("Light"))
        {
            lights.Remove(other.GetComponent<InteriorLampFlicker>());
        }
    }

}
