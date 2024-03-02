using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;
    public List<GameObject> lightList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject lightObj in GameObject.FindGameObjectsWithTag("Light"))
        {
            lightList.Add(lightObj);
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
            if (lightObj.GetComponent<InteriorLampFlicker>() != null)
            {
                if (lightObj.GetComponent<InteriorLampFlicker>().dontPowerOff == false)
                {
                    lightObj.SetActive(lightOn);
                }
            }
        }
    }
}
