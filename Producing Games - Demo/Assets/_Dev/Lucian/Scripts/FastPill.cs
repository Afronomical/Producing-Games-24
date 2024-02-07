using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastPill : InteractableTemplate, IConsumable
{
    public void Consume()
    {
        Debug.Log("Consuming fast pill");
        GameObject.Find("Player").GetComponent<PlayerMovement>().walkSpeed *= 2;
        InventoryHotbar.instance.RemoveFromInventory(collectible);
        Destroy(gameObject);
    }

    public override void Interact()
    {
        Debug.Log("***Player has taken fast pill***");
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }

    
}
