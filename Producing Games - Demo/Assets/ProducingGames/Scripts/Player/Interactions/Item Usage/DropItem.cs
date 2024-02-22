using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class DropItem : MonoBehaviour
{
    
    public Transform interactorSource;
    public float interactionRange = 4f;
    public SoundEffect failDropSound;
    private Camera cam;

    private float altarDist;


    [Header("Throw Force Values")]
    public float throwForceHoriz;
    public float throwForceVert;
    public Transform throwOrigin;

    void Start()
    {
        cam = Camera.main;
        
    }

    private void Update()
    {
        altarDist = Vector3.Distance(GameManager.Instance.altar.transform.position,this.transform.position);
        //Debug.Log(altarDist);
    }

    //event for 'R' key
    public void OnDropInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //if button is being held throw item
           if(context.interaction is HoldInteraction)
           {
                Throw();
           }
           else if(context.interaction is PressInteraction)   //if button is being pressed drop item
           {
                Drop();
           }
           
           
        }
    }

    //dropping code
    private void Drop()
    {
        if (InventoryHotbar.instance.inventory.Count == 0)
            InventoryHotbar.instance.currentItem = null;

        if (InventoryHotbar.instance.currentItem != null && altarDist > 2f)
        {
            PerformDrop();
        }
        else if (InventoryHotbar.instance.currentItem != null && altarDist < 2f && InventoryHotbar.instance.currentItem.prefab.GetComponent<InteractableTemplate>().isExorcismObject == false)
        {
            AudioManager.instance.PlaySound(failDropSound,this.gameObject.transform);
        }
        else if(InventoryHotbar.instance.currentItem != null && altarDist < 2f && InventoryHotbar.instance.currentItem.prefab.GetComponent<InteractableTemplate>().isExorcismObject == true)
        {
            PerformDrop();
        }
        
       
        
    }

    //throwing code
    private void Throw()
    {
        if (InventoryHotbar.instance.inventory.Count == 0)//checks if current item exists in hand
            InventoryHotbar.instance.currentItem = null;
        if (InventoryHotbar.instance.currentItem != null)
        {
            GameObject go = null;
            //creates the item from holding position
            go = GameObject.Instantiate(InventoryHotbar.instance.currentItem.prefab, throwOrigin.position, Quaternion.identity);
            //throwing force added to object
            Vector3 throwForceToAdd = throwOrigin.forward * throwForceHoriz + throwOrigin.up * throwForceVert;

            go.GetComponent<Rigidbody>().AddForce(throwForceToAdd, ForceMode.Impulse);
            //removing the item from inventory
            InventoryHotbar.instance.RemoveFromInventory(InventoryHotbar.instance.currentItem);
        }
    }

    private void PerformDrop()
    {
        GameObject go = null;
        //check if player is looking at ground
        Ray r = new Ray(interactorSource.position, interactorSource.forward);

        //if player looking at ground place item on floor
        if (Physics.Raycast(r, out RaycastHit hit, interactionRange) && hit.collider != null)
        {
            go = GameObject.Instantiate(InventoryHotbar.instance.currentItem.prefab, hit.point, Quaternion.Euler(90, 0, 0));
        }//if player is not looking at the ground spawn it in front of him
        else
            go = GameObject.Instantiate(InventoryHotbar.instance.currentItem.prefab, cam.transform.position + cam.transform.forward * 1.2f, Quaternion.Euler(90, 0, 0));

        InventoryHotbar.instance.RemoveFromInventory(InventoryHotbar.instance.currentItem);
    }
}
