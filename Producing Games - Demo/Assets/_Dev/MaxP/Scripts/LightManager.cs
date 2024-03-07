using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script that adds all gameobjects with the "Light" tag to a list. And a function any other script can call to turn all lights on or off, with a check to make sure we actually want them to do this.
/// </summary>

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;
    public List<GameObject> lightList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject lightObj in GameObject.FindGameObjectsWithTag("Light"))
        {
            lightList.Add(lightObj); //is there a more optimised way of doing this?
        }
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
        foreach (GameObject lightObj in lightList)
        {
            if (lightObj.GetComponent<InteriorLampFlicker>() != null && lightObj.GetComponent<InteriorLampFlicker>().dontPowerOff == false) // could i do the null check in start to make it more optimised?
            {
                lightObj.SetActive(lightOn);
            }
        }
    }
}
