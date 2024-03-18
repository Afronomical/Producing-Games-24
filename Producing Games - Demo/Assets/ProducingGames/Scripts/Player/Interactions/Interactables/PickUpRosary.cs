using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRosary : InteractableTemplate
{
    //rosary specific code
    public SoundEffect VoiceLines;

    public override void Interact()
    {
        AudioManager.instance.PlaySound(VoiceLines, gameObject.transform);
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
