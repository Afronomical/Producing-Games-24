using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonPill : InteractableTemplate
{
    public override void Interact()
    {
        Debug.Log("***Player has taken demon pill***");
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
