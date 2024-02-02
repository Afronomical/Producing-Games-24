using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    public GameObject flashlight;
    public Light light;

    public float[] intensities;
    private int intensityIndex = 0;


    void Update()
    {
        if (intensityIndex == 0)
        {
            flashlight.SetActive(false);
            light.intensity = 0;
        }

        else if (light.intensity != intensities[intensityIndex])
        {
            flashlight.SetActive(true);
            light.intensity = intensities[intensityIndex];
        }


        /*if (light.intensity > maxIntensity)
            light.intensity = maxIntensity;       

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (!isOn && Input.GetKeyDown(KeyCode.F))
        {
            flashlight.SetActive(true);
            isOn = true;            
        }
        else if (isOn && Input.GetKeyDown(KeyCode.F))
        {
            flashlight.SetActive(false);
            isOn = false;
        }

        if(isOn)
        {
            light.intensity += scrollInput * m_Intensity;
            light.intensity = Mathf.Max(0, light.intensity);
        }  */
    }

    public void OnFlashlightInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            intensityIndex++;
            if (intensityIndex >= intensities.Length)
                intensityIndex = 0;
        }
    }
}
