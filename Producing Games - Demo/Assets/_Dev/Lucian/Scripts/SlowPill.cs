using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPill : InteractableTemplate, IConsumable
{
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void Consume()
    {
        Debug.Log("Consuming slow pill");
        player.GetComponent<PlayerMovement>().slowedEffect = true;
        player.GetComponent<PlayerMovement>().walkSpeed /= 2;
        player.GetComponent<PlayerMovement>().sprintSpeed /= 2;
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
