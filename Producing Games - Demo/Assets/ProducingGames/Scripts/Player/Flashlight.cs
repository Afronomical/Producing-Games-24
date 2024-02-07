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

    private int flickerChance = 500;
    public float flickerMultiplier = 1;


    void Update()
    {
        


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

        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(Flickering());
        }
        FlashFlicker();
    }

    private void Start()
    {
        
    }

    public void OnFlashlightInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            intensityIndex++;
            if (intensityIndex >= intensities.Length)
                intensityIndex = 0;
            IntensityChange();
        }
    }

    private void IntensityChange()
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
    }

    private void FlashFlicker()
    {
        int flickerRandRange = Random.Range(1, flickerChance);
        int oldIntensityIndex = intensityIndex;
        if (flickerRandRange == 1)
        {
            StartCoroutine(Flickering());
            Debug.Log(flickerRandRange);
        }
        
    }

    IEnumerator Flickering()
    {
        float oldIntensity = light.intensity;
        int flickCount = Random.Range(5, 20);
        for (int i = 0; i < flickCount; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            light.intensity = oldIntensity * Random.Range(0.7f, 1.0f);
        }
        yield return new WaitForSeconds(Random.Range(0.2f, 1.0f));
        Debug.Log("LightDone");
        IntensityChange();
    }
}
