using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject flashlight;
    public bool isOn;
    
    void Start()
    {
        isOn = false;
        flashlight.SetActive(false);
    }


    void Update()
    {
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
    }
}
