using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandLightFlicker : MonoBehaviour
{
    public bool isFlickering = false;
    private float flickerDelay;
    public float flickerInterval; //lower the number the faster the flicker

    void Update()
    {
        if (isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }
        
    }
     
    IEnumerator FlickeringLight()
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
