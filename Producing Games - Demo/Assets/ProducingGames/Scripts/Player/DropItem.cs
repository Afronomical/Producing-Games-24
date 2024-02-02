using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DropItem : MonoBehaviour
{
    public Transform interactorSource;
    public float interactionRange = 4f;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }


    public void OnDropInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (InventoryHotbar.instance.inventory.Count == 0)
                InventoryHotbar.instance.currentItem = null;
            if (InventoryHotbar.instance.currentItem != null)
            {
                GameObject go = null;

                Ray r = new Ray(interactorSource.position, interactorSource.forward);

                if (Physics.Raycast(r, out RaycastHit hit, interactionRange) && hit.collider != null)
                {
                    go = GameObject.Instantiate(InventoryHotbar.instance.currentItem.prefab, hit.point, Quaternion.Euler(90, 0, 0));
                }
                else
                    go = GameObject.Instantiate(InventoryHotbar.instance.currentItem.prefab, cam.transform.position + cam.transform.forward * 1.2f, Quaternion.Euler(90, 0, 0));

                InventoryHotbar.instance.RemoveFromInventory(InventoryHotbar.instance.currentItem);
            }
        }
    }
}
