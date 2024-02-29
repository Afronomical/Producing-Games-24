using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXTestScript : MonoBehaviour
{
    // Reference to the CameraVFXManager
    public CameraVFXManager vfxManager;

    // Start is called before the first frame update
    void Start()
    {
        // Check if the CameraVFXManager is assigned
        if (vfxManager == null)
        {
            Debug.LogError("CameraVFXManager is not assigned to VFXTestScript!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Example: Trigger VFX effect when pressing the spacebar
        if (Input.GetKeyDown(KeyCode.J))
        {
            // Trigger the "Explosion" event
            vfxManager.TriggerVFX("Explosion");
        }
    }
}
