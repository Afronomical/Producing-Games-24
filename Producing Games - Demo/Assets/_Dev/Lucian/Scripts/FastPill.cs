using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastPill : InteractableTemplate
{
    public override void Interact()
    {
        Debug.Log("***Player has taken fast pill***");
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
