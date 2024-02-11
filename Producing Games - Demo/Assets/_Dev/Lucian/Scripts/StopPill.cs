using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPill : InteractableTemplate, IConsumable
{
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void Consume()
    {
        Debug.Log("Consuming stop pill");
        player.GetComponent<PlayerMovement>().stoppedEffect = true;
        player.GetComponent<PlayerMovement>().walkSpeed = 0;
        player.GetComponent<PlayerMovement>().sprintSpeed = 0;
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
