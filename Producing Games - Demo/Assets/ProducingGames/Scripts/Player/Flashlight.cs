using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject flashlight;
    public bool isOn;
    public Light light;
    public float m_Intensity = 1.0f;
    public float maxIntensity = 2.0f;   

    void Start()
    {
        isOn = false;
        flashlight.SetActive(false);
    }

    void Update()
    {
        if (light.intensity > maxIntensity)
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
        }              
    }
}
