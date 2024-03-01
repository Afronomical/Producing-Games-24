using UnityEngine;

/// <summary>
/// Written by: Matej Cincibus
/// Moderated by: ...
/// 
/// This class handles the key interactable object
/// </summary>

public class KeyInteractable : InteractableTemplate
{
    public override void Interact()
    {
        Debug.Log("***Player has taken key***");
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
