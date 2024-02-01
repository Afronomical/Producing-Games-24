using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeDestroyTest : InteractableTemplate
{
    public override void Interact()
    {
        Debug.Log("***Cube code is running*** \n Collected " + collectible.name);
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
