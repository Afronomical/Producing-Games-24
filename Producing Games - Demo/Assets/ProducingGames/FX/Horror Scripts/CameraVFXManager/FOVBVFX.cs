using System.Collections;
using UnityEngine;

public class FOVBVFX : MonoBehaviour
{
    [Header("FOV Settings")]
    // Maximum field of view for the camera
    public float maxFOV = 90f;

    // Duration of the FOV bounce effect
    public float bounceTime = 0.5f;

    // Speed of FOV change during bounce
    public float changeSpeed = 1f;

    // Transform of the camera to apply FOV changes
    private Transform cameraTransform;

    // Initial field of view of the camera
    private float initialFOV;

    // Camera to apply FOV changes
    public Camera cameraToBounce;

    void Awake()
    {
        // If camera is not set, use the main camera
        if (cameraToBounce == null)
        {
            cameraToBounce = Camera.main;
        }

        if (cameraToBounce != null)
        {
            cameraTransform = cameraToBounce.transform;
            initialFOV = cameraToBounce.fieldOfView;
        }
        else
        {
            Debug.LogError("No camera assigned to FOVBounceEffect script.");
        }
    }

    // Function to trigger the FOV bounce effect
    public void TriggerEffect()
    {
        StartCoroutine(BounceFOV());
    }

    // Coroutine for the FOV bounce effect
    IEnumerator BounceFOV()
    {
        float delta = 0f;

        while (delta < bounceTime)
        {
            float t = delta / bounceTime;
            float targetFOV = Mathf.Lerp(initialFOV, maxFOV, t);

            cameraToBounce.fieldOfView = targetFOV;

            delta += Time.deltaTime * changeSpeed;
            yield return null;
        }

        // Reset FOV to initial value
        cameraToBounce.fieldOfView = initialFOV;
    }
}

