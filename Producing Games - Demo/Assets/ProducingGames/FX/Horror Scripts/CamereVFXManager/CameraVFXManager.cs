using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraVFXManager : MonoBehaviour
{
    // List of VFX scripts to be called
    public List<CameraVFXScript> vfxScripts = new List<CameraVFXScript>();

    // Function to trigger VFX based on event name
    public void TriggerVFX(string eventName)
    {
        // Sort the list based on priority before triggering
        vfxScripts.Sort((x, y) => y.priority.CompareTo(x.priority));

        foreach (CameraVFXScript vfxScript in vfxScripts)
        {
            if (vfxScript.eventName == eventName)
            {
                vfxScript.Trigger();
                break; // Stop after triggering the highest priority script
            }
        }
    }
}

[System.Serializable]
public class CameraVFXScript
{
    // Event name associated with this VFX script
    public string eventName;

    // Reference to the VFX script
    public MonoBehaviour vfxScript;

    // Priority of the VFX script
    public int priority;

    // Reference to the camera to affect
    public Camera targetCamera;

    // Projection settings
    public float fieldOfView;
    public float nearClipPlane;
    public float farClipPlane;

    // Volume profile for post-processing effects
    public VolumeProfile volumeProfile;

    // Function to trigger the VFX script
    public void Trigger()
    {
        if (vfxScript != null)
        {
            // Call the VFX script's function to trigger the effect
            vfxScript.SendMessage("TriggerEffect", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            Debug.LogWarning("VFX script is not assigned for event: " + eventName);
        }
    }

    // Apply projection settings to the target camera
    public void ApplyProjectionSettings()
    {
        if (targetCamera != null)
        {
            targetCamera.fieldOfView = fieldOfView;
            targetCamera.nearClipPlane = nearClipPlane;
            targetCamera.farClipPlane = farClipPlane;
        }
        else
        {
            Debug.LogWarning("Target camera is not assigned for event: " + eventName);
        }
    }

    // Apply volume profile to the target camera
    public void ApplyVolumeProfile()
    {
        if (volumeProfile != null && targetCamera != null)
        {
            Volume volume = targetCamera.GetComponent<Volume>();
            if (volume != null)
            {
                volume.profile = volumeProfile;
            }
            else
            {
                Debug.LogWarning("Volume component not found on the target camera.");
            }
        }
        else
        {
            Debug.LogWarning("Volume profile or target camera is not assigned for event: " + eventName);
        }
    }

}
