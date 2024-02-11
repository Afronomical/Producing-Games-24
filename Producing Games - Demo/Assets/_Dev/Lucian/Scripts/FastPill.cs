using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastPill : InteractableTemplate, IConsumable
{

    GameObject player;

    private void Start()
    {
        player = GameManager.Instance.player;// = GameObject.Find("Player");
    }

    public void Consume()
    {
        Debug.Log("Consuming fast pill");
        player.GetComponent<PlayerMovement>().boostedEffect = true;
        player.GetComponent<PlayerMovement>().walkSpeed *= 2;
        player.GetComponent<PlayerMovement>().sprintSpeed *= 2;
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
