using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightOnTrigger : MonoBehaviour
{
    

    RandLightFlicker Flicker;
    public bool isFlickering = false;
    private float flickerDelay;
    public float flickerInterval;

    
    private void OnTriggerStay()
    {
        if (isFlickering == false)
        {
             StartCoroutine(TriggerFlicker());
        }
       
    }

    public IEnumerator TriggerFlicker()
    {
        isFlickering = true;
        this.gameObject.GetComponent<Light>().enabled = false;
        flickerDelay = Random.Range(0.01f, flickerInterval);
        yield return new WaitForSeconds(flickerDelay);
        this.gameObject.GetComponent<Light>().enabled = true;
        flickerDelay = Random.Range(0.01f, flickerInterval);
        yield return new WaitForSeconds(flickerDelay);
        isFlickering = false;
    }

   

}
