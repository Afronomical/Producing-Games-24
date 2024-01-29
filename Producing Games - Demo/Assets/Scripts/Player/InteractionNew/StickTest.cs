using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickTest : InteractableTemplate
{
    public override void Interact()
    {
        Debug.Log("***Stick code is running*** \n Collected " + collectible.name);
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
