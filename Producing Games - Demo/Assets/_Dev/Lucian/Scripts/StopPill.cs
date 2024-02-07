using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPill : InteractableTemplate
{
    public override void Interact()
    {
        Debug.Log("***Player has taken stop pill***");
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
