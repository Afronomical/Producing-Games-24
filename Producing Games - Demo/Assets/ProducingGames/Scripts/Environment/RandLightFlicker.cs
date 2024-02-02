using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To use this script, attach the script to given light object in scene
//The lower the flicker interval the faster the light will flicker

public class RandLightFlicker : MonoBehaviour
{
    public bool isFlickering = false;
    private float flickerDelay;
    public float flickerInterval; 

    void Update()
    {
        if (isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }
        
    }
     
   public IEnumerator FlickeringLight()
   {
        isFlickering=true;
        this.gameObject.GetComponent<Light>().enabled = false;
        flickerDelay = Random.Range(0.01f, flickerInterval);
        yield return new WaitForSeconds(flickerDelay);
        this.gameObject.GetComponent<Light>().enabled = true;
        flickerDelay = Random.Range(0.01f, flickerInterval);
        yield return new WaitForSeconds(flickerDelay);
        isFlickering = false;
   }
}
