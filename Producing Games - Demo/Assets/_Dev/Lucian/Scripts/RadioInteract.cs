using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioInteract : InteractableTemplate
{
    public SoundEffect radioNoiseSoundEffect;

    void PlayRadioNoise()
    {
        AudioManager.instance.PlaySound(radioNoiseSoundEffect, this.transform);
    }

    public override void Interact()
    {
        PlayRadioNoise();
    }

    
}
