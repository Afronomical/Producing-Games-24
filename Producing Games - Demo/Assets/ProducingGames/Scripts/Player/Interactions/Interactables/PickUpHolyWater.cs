using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHolyWater : InteractableTemplate
{
    //holy water specific code
    public override void Interact()
    {
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
