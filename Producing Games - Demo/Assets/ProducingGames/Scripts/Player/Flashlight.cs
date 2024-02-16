using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    public GameObject flashlight;
    public Light light;

    public float[] intensities;
    
    
    [Header("Battery Settings")]
    [Range(1, 200)] public float batteryCharge;
    [Range(1, 10)] public float batteryDrainRate;
    private float maxBatteryCharge;
    private int intensityIndex = 0;

    [Header("Flashlight Flickering Settings")]
    public int randFlickerChance = 500;
    public int avgFlickerCount = 20;
    [HideInInspector] public bool shouldReset = true;
    [Range(0.0f, 1f)] public float flickerLightPower = 0.7f;
    [Range(0.0f, 1f)] public float flickerLightPowerDiff = 0.3f;
    private bool isFlickering;
    private float oldIntensity;

    private void Start() => maxBatteryCharge = batteryCharge; //This will be used to make sure when you pickup a battery, your flashlight isn't Max Charge
    
    void Update()
    {
        //Debug.Log(light.intensity);

        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(Flickering());
        }
        
        FlashFlicker();


        //Higher the intensity of the flashlight, the faster the battery will drain
        batteryCharge -= batteryDrainRate * Time.deltaTime * intensities[intensityIndex];
    }


    public void OnFlashlightInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            intensityIndex++;
            if (intensityIndex >= intensities.Length)
                intensityIndex = 0;
            IntensityChange();
        }
    }

    private void IntensityChange()
    {
        if (intensityIndex == 0)
        {
            flashlight.SetActive(false);
            light.intensity = 0;
        }

        else if (light.intensity != intensities[intensityIndex])
        {
            flashlight.SetActive(true);
            light.intensity = intensities[intensityIndex];
        }

        oldIntensity = light.intensity;
    }

    private void FlashFlicker()
    {
        int flickerRandRange = Random.Range(1, randFlickerChance);
        int oldIntensityIndex = intensityIndex;
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
                light.intensity = oldIntensity * Random.Range(flickerLightPower, flickerLightPower + flickerLightPowerDiff);
            }
            if (shouldReset)
            {
                yield return new WaitForSeconds(0.2f);
                IntensityChange();
            }
            
            isFlickering = false;
        }
        
    }
}
