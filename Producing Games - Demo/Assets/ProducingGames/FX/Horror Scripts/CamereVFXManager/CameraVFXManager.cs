using System.Collections.Generic;
using UnityEngine;

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
}