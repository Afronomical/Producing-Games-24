using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPill : InteractableTemplate, IConsumable
{
    public void Consume()
    {
        Debug.Log("Consuming stop pill");
        GameObject.Find("Player").GetComponent<PlayerMovement>().walkSpeed = 0;
        InventoryHotbar.instance.RemoveFromInventory(collectible);
        Destroy(gameObject);
    }

    public override void Interact()
    {
        Debug.Log("***Player has taken stop pill***");
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
