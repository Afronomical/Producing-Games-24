using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRock : InteractableTemplate
{
    public SoundEffect dropNoise;
    
    public override void Interact()
    {
        InventoryHotbar.instance.AddToInventory(collectible);
        this.gameObject.SetActive(false); //simply disabling the object for now because of the audiosource reference
        //Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))// != null) //anything it hits it will play a sound(for now)
        {
            AudioManager.instance.PlaySound(dropNoise, this.transform);
        }
    }


}
