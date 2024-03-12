using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLineOnTrigger : MonoBehaviour
{ 
    [Header("Voice Line")]
    public SoundEffect VoiceLine;
    private bool hasPlayed;
    
    // Start is called before the first frame update
    void Start()
    {
        hasPlayed = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !hasPlayed)
        {
            AudioManager.instance.PlaySound(VoiceLine, gameObject.transform);
            hasPlayed = true;
        }
    }
}
