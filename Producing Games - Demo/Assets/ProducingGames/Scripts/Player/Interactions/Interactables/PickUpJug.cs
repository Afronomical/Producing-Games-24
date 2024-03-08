using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpJug : InteractableTemplate
{
    public float capacity;
    private float minCapacity = 0;
    private float maxCapacity = 5;

    private void Start()
    {
        capacity = minCapacity;
        collectible.tooltipText = ("Pick up Holy Water Jug");
    }
    public override void Interact()
    {
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
        GameManager.Instance.playerHasJug = true;
    }
}
