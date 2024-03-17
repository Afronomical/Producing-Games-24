using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerTemp : MonoBehaviour
{
    public AudioSource Churhc,crypt;
    public AudioClip AMB_church, AMB_crypt; 


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

           Churhc.enabled = true;

            Churhc.PlayOneShot(AMB_church);
            crypt.enabled = false;
            
        }
        else
        {
            crypt.enabled = true;
            crypt.PlayOneShot(AMB_crypt);
            Churhc.enabled = false;
        }
        
    }   
}
