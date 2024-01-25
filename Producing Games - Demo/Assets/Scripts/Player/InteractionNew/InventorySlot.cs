using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public List<InteractiveObject> inventory = new List<InteractiveObject>();
    int currentIndex;

    public List<GameObject> itemSlots = new List<GameObject>();

    public static InventorySlot instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        foreach (Transform child in transform)
        {
            itemSlots.Add(child.gameObject);
        }
    }

    private void Start()
    {
        currentIndex = itemSlots.Count/2;
    }

    private void Update()
    {

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
        
        int centerSlotIndex = itemSlots.Count/2;
        itemSlots[centerSlotIndex - 2].GetComponent<Image>().sprite = GetItemFromInventory(currentIndex - 2).objectImage;
        itemSlots[centerSlotIndex - 1].GetComponent<Image>().sprite = GetItemFromInventory(currentIndex - 1).objectImage;
        itemSlots[centerSlotIndex].GetComponent<Image>().sprite = GetItemFromInventory(currentIndex).objectImage;
        itemSlots[centerSlotIndex + 1].GetComponent<Image>().sprite = GetItemFromInventory(currentIndex + 1).objectImage;
        itemSlots[centerSlotIndex + 2].GetComponent<Image>().sprite = GetItemFromInventory(currentIndex + 2).objectImage;
    }

    InteractiveObject GetItemFromInventory(int itemIndex)
    {
        if (itemIndex < 0)
            itemIndex = inventory.Count + itemIndex;

        if (itemIndex > inventory.Count)
            itemIndex = itemIndex - inventory.Count;


        if (itemIndex < 0)
            return inventory[0];
        else if (itemIndex > inventory.Count)
            return inventory[inventory.Count];
        else
            return inventory[itemIndex];
    }
}
