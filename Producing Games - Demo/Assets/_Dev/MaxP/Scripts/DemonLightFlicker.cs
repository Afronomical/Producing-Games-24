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
    private List<Collider> colliders;
    
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
        colliders = new List<Collider>();
    }


    void Update()
    {
        if (colliders != null)
        {
            foreach (Collider collider in colliders) //for each light apply flicker
            {
                GameObject otherObj = collider.gameObject;
                lightScript = otherObj.GetComponent<InteriorLampFlicker>();

                Vector3 thisLoc = transform.position;
                Vector3 lightLoc = otherObj.transform.position;
                float distance = Vector3.Distance(thisLoc, lightLoc);
                Debug.Log(distance);
                bool resetFlicker = false;
                float origFlickerLightPower = lightScript.flickerLightPower;
                int origFlickerCount = lightScript.avgFlickerCount;
                float origFlickerLightDiff = lightScript.flickerLightPowerDiff;
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
                    resetFlicker = false;

                }
            }
        }
        
    }


    private void OnTriggerEnter(Collider other) //adds anything with "Light" tag to the list
    {
        if (other.CompareTag("Light"))
        {
            colliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other) //removes anything with "Light" tag from the list
    {
        if (other.CompareTag("Light"))
        {
            colliders.Remove(other);
        }
    }

}
