using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class InventoryHotbar : MonoBehaviour
{
    public List<InteractiveObject> inventory = new List<InteractiveObject>();
    public int currentIndex;
    public InteractiveObject emptySlot;

    public List<GameObject> itemSlots = new List<GameObject>();
    private int centerSlotIndex = 2;

    public float changeSlotDelay;
    private float changeSlotTimer;

    public static InventoryHotbar instance;

    public InteractiveObject currentItem;

    public delegate void ItemPickedUpEvent();
    public event ItemPickedUpEvent OnItemPickedUp;
    public event Action OnItemSelected;


    public Transform spawnPos;
    public GameObject go = null;
    bool holding = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        foreach (Transform child in transform)
        {
            itemSlots.Add(child.gameObject);
        }
    }


    private void Update()
    {
        changeSlotTimer += Time.deltaTime;

        if (inventory.Count != 0)
        {
            currentItem = inventory[currentIndex];
            //go = currentItem.prefab.gameObject;
        }
    }


    public void OnScrollInput(InputAction.CallbackContext context)
    {
        if (context.performed && changeSlotTimer >= changeSlotDelay)
        {
            if (context.ReadValue<Vector2>().y > 0)
            {
                ScrollInventory(1);
            }
            else if (context.ReadValue<Vector2>().y < 0)
            {
                ScrollInventory(-1);
            }

            changeSlotTimer = 0;
        }
    }


    public void AddToInventory(InteractiveObject item)
    {
        inventory.Add(item);  // Add the item to the inventory
        ScrollInventory(0);  // Refresh the inventory images

        // Trigger the event when an item is picked up
        if (OnItemPickedUp != null)
            OnItemPickedUp.Invoke();
    }


    public void RemoveFromInventory(InteractiveObject item)
    {
        inventory.Remove(item);  // Remove the item from the inventory
        Destroy(go);
        currentItem = null; // Clears holding value in-case this was the last item in the inventory
        ScrollInventory(0);  // Refresh the inventory images
    }


    public void ScrollInventory(int dir)
    {
        OnItemSelected?.Invoke();
        currentIndex += dir;
        if (currentIndex < 0)
            currentIndex = inventory.Count - 1;
        else if (currentIndex > inventory.Count - 1)
            currentIndex = 0;
        
        itemSlots[centerSlotIndex - 2].GetComponent<Image>().sprite = GetItemFromInventory(currentIndex - 2).objectImage;
        itemSlots[centerSlotIndex - 1].GetComponent<Image>().sprite = GetItemFromInventory(currentIndex - 1).objectImage;
        itemSlots[centerSlotIndex].GetComponent<Image>().sprite = GetItemFromInventory(currentIndex).objectImage;
        itemSlots[centerSlotIndex + 1].GetComponent<Image>().sprite = GetItemFromInventory(currentIndex + 1).objectImage;
        itemSlots[centerSlotIndex + 2].GetComponent<Image>().sprite = GetItemFromInventory(currentIndex + 2).objectImage;

        //if inventory has at least one item
        if (inventory.Count != 0)
        {
            Debug.Log("Currently holding " + inventory[currentIndex].objectName);
            if (!holding)
            {
                //instantiate the object currently held
                go = Instantiate(inventory[currentIndex].prefab, spawnPos.position, Quaternion.identity);
                go.transform.parent = spawnPos.transform;
                go.transform.GetComponent<Rigidbody>().useGravity = false;
                go.transform.GetComponent<Collider>().enabled = false;
                go.layer = 9;
                holding = true;
                //if (go.gameObject.TryGetComponent(out IConsumable cons))
                //{
                //    PlayerInteractor.instance.consumable = cons;
                //}
                //else
                //{
                //    PlayerInteractor.instance.consumable = null;
                //}
            }
            else
            {
                Destroy(go);
                go = Instantiate(inventory[currentIndex].prefab, spawnPos.position, Quaternion.identity);
                go.transform.parent = spawnPos.transform;
                go.transform.GetComponent<Rigidbody>().useGravity = false;
                go.transform.GetComponent<Collider>().enabled = false;
                go.layer = 9;
                //if (go.gameObject.TryGetComponent(out IConsumable cons))
                //{
                //    PlayerInteractor.instance.consumable = cons;
                //}
                //else
                //{
                //    PlayerInteractor.instance.consumable = null;
                //}
            }
            if(go != null)
            {
                if (go.gameObject.TryGetComponent(out IConsumable cons))
                {
                    PlayerInteractor.instance.consumable = cons;
                }
                else
                {
                    PlayerInteractor.instance.consumable = null;
                }

            }
            go.layer = 9;
        }
            
        
    }

    InteractiveObject GetItemFromInventory(int itemIndex)
    {
        /*if (itemIndex < 0)
            itemIndex = inventory.Count - 1 + itemIndex;

        if (itemIndex > inventory.Count - 1)
            itemIndex = itemIndex - inventory.Count - 1;*/


        if (itemIndex < 0)
            return emptySlot;
        else if (itemIndex > inventory.Count - 1)
            return emptySlot;
        else
            return inventory[itemIndex];
    }
}