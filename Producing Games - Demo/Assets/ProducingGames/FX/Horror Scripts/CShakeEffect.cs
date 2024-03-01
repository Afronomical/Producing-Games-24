using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CShakeEffect : MonoBehaviour
{
    // Intensity of the shake effect
    public float shakeIntensity = 0.1f;

    // Duration of the shake effect
    public float shakeDuration = 0.5f;

    // Transform of the camera to shake
    private Transform cameraTransform;

    // Initial position of the camera
    private Vector3 initialPosition;

    // Camera to shake
    public Camera cameraToShake;

    void Awake()
    {
        // If camera is not set, use the main camera
        if (cameraToShake == null)
        {
            cameraToShake = Camera.main;
        }

        if (cameraToShake != null)
        {
            cameraTransform = cameraToShake.transform;
        }
        else
        {
            Debug.LogError("No camera assigned to CameraShake script.");
        }
    }

    // Function to trigger the camera shake effect
    public void TriggerEffect()
    {
        initialPosition = cameraTransform.localPosition;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", shakeDuration);
    }

    // Function to apply the shake effect
    private void DoShake()
    {
        if (shakeDuration > 0)
        {
            cameraTransform.localPosition = initialPosition + Random.insideUnitSphere * shakeIntensity;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            cameraTransform.localPosition = initialPosition;
        }
    }

    // Function to stop the shake effect
    private void StopShake()
    {
        CancelInvoke("DoShake");
        cameraTransform.localPosition = initialPosition;
    }
}
