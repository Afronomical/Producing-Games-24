using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickTest : InteractableTemplate
{
    public override void Interact()
    {
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
