using System.Collections;
using UnityEngine;

public class BlinkingLightsEvent : MonoBehaviour
{
    public Flashlight flashlight;
    public Light[] blinkingLights;
    public float blinkDuration = 5f;

    private bool isBlinking;

    private void Start()
    {
        isBlinking = false;
    }

    public void TriggerBlinkingLights()
    {
        if (!isBlinking)
        {
            StartCoroutine(BlinkingLightsCoroutine());
        }
    }

    private IEnumerator BlinkingLightsCoroutine()
    {
        isBlinking = true;

        // Store initial intensities of lights
        float[] initialIntensities = new float[blinkingLights.Length];
        for (int i = 0; i < blinkingLights.Length; i++)
        {
            initialIntensities[i] = blinkingLights[i].intensity;
        }

        // Turn off lights
        foreach (Light light in blinkingLights)
        {
            light.intensity = 0f;
        }

        // Wait for blinkDuration seconds
        yield return new WaitForSeconds(blinkDuration);

        // Restore initial intensities
        for (int i = 0; i < blinkingLights.Length; i++)
        {
            blinkingLights[i].intensity = initialIntensities[i];
        }

        isBlinking = false;
    }
}