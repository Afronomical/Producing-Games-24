using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;


public class DimPill : InteractableTemplate, IConsumable
{
    public Image panel;

    private void Start()
    {
        panel = GameObject.Find("CameraDimOverlay").GetComponent<Image>();
    }
    public void Consume()
    {
        Debug.Log("Consuming dim pill");
        panel.enabled=true;
        InventoryHotbar.instance.RemoveFromInventory(collectible);
        Destroy(gameObject);
    }

    public override void Interact()
    {
        Debug.Log("***Player has taken dim pill***");
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
