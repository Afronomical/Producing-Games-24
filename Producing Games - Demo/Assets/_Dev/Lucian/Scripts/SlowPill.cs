using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPill : InteractableTemplate, IConsumable
{
    public void Consume()
    {
        Debug.Log("Consuming slow pill");
        GameObject.Find("Player").GetComponent<PlayerMovement>().walkSpeed /= 2;
        InventoryHotbar.instance.RemoveFromInventory(collectible);
        Destroy(gameObject);
    }

    public override void Interact()
    {
        Debug.Log("***Player has taken slow pill***");
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
