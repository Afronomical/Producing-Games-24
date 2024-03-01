using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXTestScript : MonoBehaviour
{
    // Reference to the CameraShake script
    public CShakeEffect cameraShake;

    void Update()
    {
        // Check if the "K" key is pressed
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Trigger camera shake effect
            if (cameraShake != null)
            {
                cameraShake.TriggerEffect();
            }
            else
            {
                Debug.LogError("CameraShake script is not assigned.");
            }
        }
    }
}
