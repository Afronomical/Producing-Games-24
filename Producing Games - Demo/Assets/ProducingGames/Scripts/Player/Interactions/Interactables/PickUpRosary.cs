using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRosary : InteractableTemplate
{
    //rosary specific code
    public override void Interact()
    {
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
