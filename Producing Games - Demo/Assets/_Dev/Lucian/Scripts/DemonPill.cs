using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonPill : InteractableTemplate, IConsumable
{
    public void Consume()
    {
        Debug.Log("Consuming demon pill");
        Debug.Log("DEMON IS BANISHED");
        InventoryHotbar.instance.RemoveFromInventory(collectible);
        Destroy(gameObject);
    }

    public override void Interact()
    {
        Debug.Log("***Player has taken demon pill***");
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
