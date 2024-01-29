using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroyTest : InteractiveBaseClass
{
    public override void Interact()
    {
        Debug.Log("Collected " + gameObject.name);
        //Destroy(this.GetComponent<InteractableTemplate>().collectible.obj.gameObject);
        //Destroy(gameObject);
        //InventoryHotbar.instance.AddToInventory(GetComponent<InteractiveObject>());
        //this.gameObject.SetActive(false);
    }
}
