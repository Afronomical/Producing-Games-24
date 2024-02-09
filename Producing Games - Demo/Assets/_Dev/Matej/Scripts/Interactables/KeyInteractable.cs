using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteractable : InteractableTemplate
{
    public override void Interact()
    {
        Debug.Log("***Player has taken key***");
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
