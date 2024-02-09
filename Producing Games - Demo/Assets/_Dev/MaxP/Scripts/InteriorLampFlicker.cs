using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorLampFlicker : MonoBehaviour
{
    private Light wallLight;
    private float origIntensity;
    private bool isFlickering;
    public int randFlickerChance = 500;
    public int avgFlickerCount = 20;
    [Range(0.0f, 1f)] public float flickerLightPower = 0.7f;
    [Range(0.0f, 1f)] public float flickerLightPowerDiff = 0.3f;

    void Start()
    {
        wallLight = GetComponent<Light>();
        origIntensity = wallLight.intensity;
    }

    
    void Update()
    {
        WallFlicker();
    }

    private void WallFlicker()
    {
        int flickerRandRange = Random.Range(1, randFlickerChance);
        
        if (flickerRandRange == 1)
        {
            StartCoroutine(Flickering());
        }

    }

    public IEnumerator Flickering()
    {
        if (isFlickering == false)
        {
            isFlickering = true;
            int flickCount = Random.Range(avgFlickerCount - 5, avgFlickerCount + 5);
            for (int i = 0; i < flickCount; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
                wallLight.intensity = origIntensity * Random.Range(flickerLightPower, flickerLightPower + flickerLightPowerDiff);
            }
            yield return new WaitForSeconds(0.2f);
            wallLight.intensity = origIntensity;

            isFlickering = false;
        }

    }
}
