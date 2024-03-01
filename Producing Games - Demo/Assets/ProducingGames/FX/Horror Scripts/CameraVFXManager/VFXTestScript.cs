using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXTestScript : MonoBehaviour
{
    // Reference to the CameraVFXManager
    public CameraVFXManager vfxManager;

    // Event name associated with the FOV bounce effect
    public string fovBounceEventName = "FOVBounce";

    void Update()
    {
        // Check if the "K" key is pressed
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Trigger the FOV bounce effect through the CameraVFXManager
            if (vfxManager != null)
            {
                vfxManager.TriggerVFX(fovBounceEventName);
            }
            else
            {
                Debug.LogError("CameraVFXManager is not assigned.");
            }
        }
    }
}

