using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    public GameObject flashlight;
    public GameObject clipboardFlashlight;
    public Light light;
    public SoundEffect toggleSound;

    public float[] intensities;
    
    
    [Header("Battery Settings")]
    [Range(0, 100)] public float batteryCharge;
    [Range(0, 10)] public float batteryDrainRate;
    [NonSerialized] public float maxBatteryCharge;
    private bool unlimitedBatteryActivated;
    [HideInInspector] public int intensityIndex = 0;

    [Header("Flashlight Flickering Settings")]
    public int randFlickerChance = 500;
    public int avgFlickerCount = 20;
    [HideInInspector] public bool shouldReset = true;
    [Range(0.0f, 1f)] public float flickerLightPower = 0.7f;
    [Range(0.0f, 1f)] public float flickerLightPowerDiff = 0.3f;
    private bool isFlickering;
    private float oldIntensity; 


    private void Start()
    {
        CommandConsole.Instance.ToggleFlashlight += UnlimitedBatteryToggle;
        maxBatteryCharge = batteryCharge; //This will be used to make sure when you pickup a battery, your flashlight isn't Max Charge
    }

    void Update()
    {
        //Debug.Log(light.intensity);

        //If the battery charge is 0, it will turn off the flashlight
        //The changes to the intensity system made this useless
        /*
        if(batteryCharge <= 0 && !unlimitedBatteryActivated)
        {
            intensityIndex = 0;
            IntensityChange();
        }
        */

        //Higher the intensity of the flashlight, the faster the battery will drain
        if (!unlimitedBatteryActivated)
        {
            batteryCharge -= batteryDrainRate * Time.deltaTime * intensityIndex;
        }
        else
        {
            batteryCharge = maxBatteryCharge;
        }

        if (clipboardFlashlight.activeSelf && batteryCharge <= 0)
            clipboardFlashlight.SetActive(false);
        else if (!clipboardFlashlight.activeSelf && batteryCharge > 0)
            clipboardFlashlight.SetActive(true);
        
        //Called every frame to account for the decreasing charge
        IntensityChange();
        
        FlashFlicker();

    }


    public void OnFlashlightInput(InputAction.CallbackContext context)
    {
        if (context.performed && flashlight.activeInHierarchy)
        {
            intensityIndex++;
            if (intensityIndex >= intensities.Length)
                intensityIndex = 0;
            IntensityChange();

            // Moved the sound to the actual action that triggers it
            AudioManager.instance.PlaySound(toggleSound, null);
        }
    }

    public void IntensityChange()
    {
        if (light.intensity != intensities[intensityIndex])
        {
            flashlight.SetActive(true);
            // Sets the intensity to a value proportional to the charge in the battery
            light.intensity = batteryCharge / maxBatteryCharge * intensities[intensityIndex];
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

    public void UnlimitedBatteryToggle() => unlimitedBatteryActivated = !unlimitedBatteryActivated;
}
