using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHotbar : MonoBehaviour
{
    public List<InteractiveObject> inventory = new List<InteractiveObject>();
    public int currentIndex;
    public InteractiveObject emptySlot;

    public List<GameObject> itemSlots = new List<GameObject>();
    private int centerSlotIndex = 2;

    public static InventoryHotbar instance;

    public InteractiveObject currentItem;

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
        if (Input.GetKeyDown(KeyCode.LeftArrow)) ScrollInventory(-1);
        if (Input.GetKeyDown(KeyCode.RightArrow)) ScrollInventory(1);
    }


    public void AddToInventory(InteractiveObject item)
    {
        inventory.Add(item);  // Add the item to the inventory
        ScrollInventory(0);  // Refresh the inventory images
    }


    public void RemoveFromInventory(InteractiveObject item)
    {
        inventory.Remove(item);  // Remove the item from the inventory
        ScrollInventory(0);  // Refresh the inventory images
    }


    void ScrollInventory(int dir)
    {
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

        currentItem = inventory[currentIndex];

        Debug.Log("Currently holding " + inventory[currentIndex].objectName);
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
