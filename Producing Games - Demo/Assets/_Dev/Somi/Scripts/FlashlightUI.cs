using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class FlashlightUI : MonoBehaviour
{

    Flashlight flashlight;
    float currentBatteryPercent;
    float currentIndexPercent;




    [SerializeField] FlashlightLEDs[] BatteryIndicatorLEDs = new FlashlightLEDs[4];

    //[SerializeField] FlashlightLEDs[] NewBatteryIndicators = new FlashlightLEDs[4];

    private void Start()
    {
        flashlight = FindFirstObjectByType<Flashlight>().GetComponent<Flashlight>();
    }

    private void Update()
    {
        
        currentBatteryPercent = Mathf.Clamp((flashlight.batteryCharge / flashlight.maxBatteryCharge), 0, 1);

        /*float currentLEDIndex = 0;
        int flashingIndex = 0;
        foreach (var LED in BatteryIndicatorLEDs)
        {
            currentIndexPercent = (currentLEDIndex + 1) / BatteryIndicatorLEDs.Length;
            currentLEDIndex += 1;

            if (currentBatteryPercent == currentIndexPercent)
            {
                LED.ledState = FlashlightLEDs.LEDState.Charged;
                //flashingIndex = ((int)currentIndexPercent + 1) * BatteryIndicatorLEDs.Length;
            }
            else if (currentBatteryPercent > currentIndexPercent)
            {
                LED.ledState = FlashlightLEDs.LEDState.Charged;
            }
            else if (currentBatteryPercent < currentIndexPercent)
            {
                LED.ledState = FlashlightLEDs.LEDState.Dead;
            }
            else
            {
                LED.ledState = FlashlightLEDs.LEDState.Charged;
                Debug.Log("Broken");
            }
            //Debug.Log(currentBatteryPercent + " " + currentIndexPercent);

            //BatteryIndicatorLEDs[flashingIndex].ledState = FlashlightLEDs.LEDState.Flashing;
        }

        foreach (var item in BatteryIndicatorLEDs)
        {

        }*/


        switch (currentBatteryPercent)
        {
            case float i when (i <= 0):

                BatteryIndicatorLEDs[3].ledState = FlashlightLEDs.LEDState.Dead;
                BatteryIndicatorLEDs[2].ledState = FlashlightLEDs.LEDState.Dead;
                BatteryIndicatorLEDs[1].ledState = FlashlightLEDs.LEDState.Dead;
                BatteryIndicatorLEDs[0].ledState = FlashlightLEDs.LEDState.Dead;

                break;

            case float i when (i > 0 && i < 0.25):

                BatteryIndicatorLEDs[3].ledState = FlashlightLEDs.LEDState.Dead;
                BatteryIndicatorLEDs[2].ledState = FlashlightLEDs.LEDState.Dead;
                BatteryIndicatorLEDs[1].ledState = FlashlightLEDs.LEDState.Dead;
                BatteryIndicatorLEDs[0].ledState = FlashlightLEDs.LEDState.Flashing;

                Debug.Log(" 1");
                break;

            case float i when (i > 0.25 && i < 0.50):

                BatteryIndicatorLEDs[3].ledState = FlashlightLEDs.LEDState.Dead;
                BatteryIndicatorLEDs[2].ledState = FlashlightLEDs.LEDState.Dead;
                BatteryIndicatorLEDs[1].ledState = FlashlightLEDs.LEDState.Flashing;
                BatteryIndicatorLEDs[0].ledState = FlashlightLEDs.LEDState.Charged;
                Debug.Log(" 2");
                break;

            case float i when (i > 0.50 && i < 0.75):

                BatteryIndicatorLEDs[3].ledState = FlashlightLEDs.LEDState.Dead;
                BatteryIndicatorLEDs[2].ledState = FlashlightLEDs.LEDState.Flashing;
                BatteryIndicatorLEDs[1].ledState = FlashlightLEDs.LEDState.Charged;
                BatteryIndicatorLEDs[0].ledState = FlashlightLEDs.LEDState.Charged;
                Debug.Log(" 3");
                break;

            case float i when (i > 0.75 && i < 1):

                BatteryIndicatorLEDs[3].ledState = FlashlightLEDs.LEDState.Flashing;
                BatteryIndicatorLEDs[2].ledState = FlashlightLEDs.LEDState.Charged;
                BatteryIndicatorLEDs[1].ledState = FlashlightLEDs.LEDState.Charged;
                BatteryIndicatorLEDs[0].ledState = FlashlightLEDs.LEDState.Charged;
                Debug.Log(" 4");
                break;

            case float i when (i == 1):

                BatteryIndicatorLEDs[3].ledState = FlashlightLEDs.LEDState.Charged;
                BatteryIndicatorLEDs[2].ledState = FlashlightLEDs.LEDState.Charged;
                BatteryIndicatorLEDs[1].ledState = FlashlightLEDs.LEDState.Charged;
                BatteryIndicatorLEDs[0].ledState = FlashlightLEDs.LEDState.Charged;
                Debug.Log(" 5");
                break;


        }



    }



}
