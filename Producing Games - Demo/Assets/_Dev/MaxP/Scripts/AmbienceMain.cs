using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceMain : MonoBehaviour
{
    private SoundEffect currentAmbience;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewAmbiencePlay(SoundEffect playedSound)
    {
        if (playedSound != currentAmbience)
        {
            AudioManager.instance.PlayGlobalAmbience(playedSound);
        }
        currentAmbience = playedSound;
    }
}
