using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightUI : MonoBehaviour
{

    Flashlight flashlight;
    float currentBatteryCharge;


    [SerializeField] FlashlightLEDs[] BatteryIndicatorLEDs = new FlashlightLEDs[4];

    
    private void Update()
    {
        currentBatteryCharge = Mathf.Clamp((flashlight.batteryCharge / flashlight.maxBatteryCharge), 0, 1);


        switch (currentBatteryCharge)
        {
            case 0:

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


                break;

            case float i when (i > 0.25 && i < 0.50):

                BatteryIndicatorLEDs[3].ledState = FlashlightLEDs.LEDState.Dead;
                BatteryIndicatorLEDs[2].ledState = FlashlightLEDs.LEDState.Dead;
                BatteryIndicatorLEDs[1].ledState = FlashlightLEDs.LEDState.Flashing;
                BatteryIndicatorLEDs[0].ledState = FlashlightLEDs.LEDState.Charged;

                break;

            case float i when (i < 0.50 && i < 0.75):

                BatteryIndicatorLEDs[3].ledState = FlashlightLEDs.LEDState.Dead;
                BatteryIndicatorLEDs[2].ledState = FlashlightLEDs.LEDState.Flashing;
                BatteryIndicatorLEDs[1].ledState = FlashlightLEDs.LEDState.Charged;
                BatteryIndicatorLEDs[0].ledState = FlashlightLEDs.LEDState.Charged;

                break;

            case float i when (i < 0.75 && i < 1):

                BatteryIndicatorLEDs[3].ledState = FlashlightLEDs.LEDState.Flashing;
                BatteryIndicatorLEDs[2].ledState = FlashlightLEDs.LEDState.Charged;
                BatteryIndicatorLEDs[1].ledState = FlashlightLEDs.LEDState.Charged;
                BatteryIndicatorLEDs[0].ledState = FlashlightLEDs.LEDState.Charged;

                break;
            default:
                break;
        }



    }

    public void ChangeBatteryIndicatorLevel()
    {

    }



}
