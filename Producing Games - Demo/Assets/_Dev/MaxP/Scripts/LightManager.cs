using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script that adds all gameobjects with the "Light" tag to a list. And a function any other script can call to turn all lights on or off, with a check to make sure we actually want them to do this.
/// </summary>

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;
    //public List<GameObject> lightList = new List<GameObject>();
    public InteriorLampFlicker[] lightList;

    // Start is called before the first frame update
    void Start()
    {
        //foreach(GameObject lightObj in GameObject.FindObjectOfType<InteriorLampFlicker>())
        //{
        //    lightList.Add(lightObj); //is there a more optimised way of doing this?
        //}

        lightList = FindObjectsByType<InteriorLampFlicker>(FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.M))
        //{
        //    AllLightToggle(true);
        //}
        //if (Input.GetKey(KeyCode.N))
        //{
        //    AllLightToggle(false);
        //}
    }

    void AllLightToggle(bool lightOn)
    {
        foreach (InteriorLampFlicker lightObj in lightList)
        {
            if (lightObj.dontPowerOff == false) // could i do the null check in start to make it more optimised?        lightObj.GetComponent<InteriorLampFlicker>() != null && 
            {
                lightObj.transform.gameObject.SetActive(lightOn);
            }
        }
    }
}
