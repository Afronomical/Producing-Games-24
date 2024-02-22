using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpJug : InteractableTemplate
{

    public override void Interact()
    {
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
        GameManager.Instance.playerHasJug = true;
    }
}
