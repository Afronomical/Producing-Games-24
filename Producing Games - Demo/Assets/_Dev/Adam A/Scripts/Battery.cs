using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : InteractableTemplate
{
    [Header("Flashlight Reference/Charge Amount")]
    public Flashlight flashlightRef;
    [Range(0.0f, 200f)]public int chargeGainAmount = 50;

    //When the Battery is picked up, charge the flashlight by the specified amount (chargeGainAmount)
    public override void Interact()
    {
        flashlightRef.batteryCharge += chargeGainAmount;

        if(flashlightRef.batteryCharge >= flashlightRef.maxBatteryCharge)
            flashlightRef.batteryCharge = flashlightRef.maxBatteryCharge;

        Destroy(gameObject);
        
    }
}
