using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowItem : MonoBehaviour
{

    public Transform interactorSource;
    public float interactionRange = 4f;
    public float throwForceHoriz;
    public float throwForceVert;
    public Transform throwOrigin;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void OnThrowItem(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (InventoryHotbar.instance.inventory.Count == 0)
                InventoryHotbar.instance.currentItem = null;
            if (InventoryHotbar.instance.currentItem != null)
            {
                GameObject go = null;
            
                go = GameObject.Instantiate(InventoryHotbar.instance.currentItem.prefab, throwOrigin.position, Quaternion.identity);

                Vector3 throwForceToAdd = throwOrigin.forward * throwForceHoriz + throwOrigin.up * throwForceVert;

                go.GetComponent<Rigidbody>().AddForce(throwForceToAdd, ForceMode.Impulse);

                InventoryHotbar.instance.RemoveFromInventory(InventoryHotbar.instance.currentItem);
            }
        }
    }
}
