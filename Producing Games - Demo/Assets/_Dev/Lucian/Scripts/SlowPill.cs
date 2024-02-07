using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPill : InteractableTemplate
{
    public override void Interact()
    {
        Debug.Log("***Player has taken slow pill***");
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
