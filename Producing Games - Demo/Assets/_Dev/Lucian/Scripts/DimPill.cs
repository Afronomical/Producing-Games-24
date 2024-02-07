using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimPill : InteractableTemplate
{
    public override void Interact()
    {
        Debug.Log("***Player has taken dim pill***");
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
