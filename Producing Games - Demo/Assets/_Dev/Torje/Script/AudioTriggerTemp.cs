using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerTemp : MonoBehaviour
{
    public AudioSource churchSource,cryptSource;
    public SoundEffect church, crypt; 


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.StopSound(church);
            AudioManager.instance.PlaySound(church, churchSource.transform);
        }
        else
        {
            AudioManager.instance.StopSound(crypt);
            AudioManager.instance.PlaySound(crypt, transform);
        }
        
    }   
}
