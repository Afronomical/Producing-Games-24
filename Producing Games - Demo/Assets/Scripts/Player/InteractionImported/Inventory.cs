using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Inventory : MonoBehaviour, IDragHandler
{
    public List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<InteractiveObject, InventoryItem> itemDictionary = new Dictionary<InteractiveObject, InventoryItem>();
    private int inventoryIndex;
    private Transform displayItem;
    private Transform displayItemL;
    private Transform displayItemR;
    private Journal journal;

    [Header("Inventory Parts")]
    public GameObject inventoryPanel;
    public GameObject menuPanel;
    public TMP_Text itemName;
    public TMP_Text itemCount;
    public TMP_Text itemDescription;
    public Image closeButton;
    public Image journalButton;
    public Image equipButton;
    public Image unequipButton;
    public Image dropButton;
    public Transform dropPosition;
    public Camera itemDisplayCamera;
    public float rotateSpeed;

    [Header("Button Prompts")]
    public Sprite[] closeButtonSprites;
    public Sprite[] journalButtonSprites;
    public Sprite[] equipButtonSprites;
    public Sprite[] unequipButtonSprites;
    public Sprite[] dropButtonSprites;


    private void Start()
    {
        inventoryPanel.SetActive(true);
        menuPanel.SetActive(false);
        journal = GetComponent<Journal>();
    }


    private void OnEnable()
    {
        PickUp.OnItemCollected += Add;
    }


    private void OnDisable()
    {
        PickUp.OnItemCollected -= Add;
    }


    private void Update()
    {
        if (displayItem != null)
            displayItem.eulerAngles += new Vector3(0, rotateSpeed * Time.deltaTime);
    }


    public void Add(InteractiveObject itemData)  // Add an item to the inventory
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))  // If it already exits in the inventory
        {
            item.AddToStack();  // Add one to the stack
        }
        else  // If we don't have one yet
        {
            InventoryItem newItem = new InventoryItem(itemData);  // Create a new item type
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
        }
    }


    public void Remove(InteractiveObject itemData)  // Remove an item from the inventory
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RemoveFromStack();  // Remove one from the stack
            
            if (item.stackSize == 0)  // If there are none left
            {
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }
        }
    }


    public void OpenInventory()  // Open the inventory screen
    {
        SwitchItem(0);

        if (GameManager.manager.controlType.device is XInputController)  // XBox button prompts
        {
            closeButton.overrideSprite = closeButtonSprites[0];
            journalButton.overrideSprite = journalButtonSprites[0];
            equipButton.overrideSprite = equipButtonSprites[0];
            unequipButton.overrideSprite = unequipButtonSprites[0];
            dropButton.overrideSprite = dropButtonSprites[0];
        }
        else if (GameManager.manager.controlType.device is Gamepad)  // Playstation button prompts
        {
            closeButton.overrideSprite = closeButtonSprites[1];
            journalButton.overrideSprite = journalButtonSprites[1];
            equipButton.overrideSprite = equipButtonSprites[1];
            unequipButton.overrideSprite = unequipButtonSprites[1];
            dropButton.overrideSprite = dropButtonSprites[1];
        }
        else  // Keyboard button prompts
        {
            closeButton.overrideSprite = closeButtonSprites[2];
            journalButton.overrideSprite = journalButtonSprites[2];
            equipButton.overrideSprite = equipButtonSprites[2];
            unequipButton.overrideSprite = unequipButtonSprites[2];
            dropButton.overrideSprite = dropButtonSprites[2];
        }

        menuPanel.SetActive(true);
        inventoryPanel.SetActive(true);
        journal.journalPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        closeButton.transform.parent.GetComponent<Button>().Select();
        GameManager.manager.state = GameManager.GameStates.Inventory;
    }


    public void CloseInventory()  // Close the inventory screen
    {
        menuPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.manager.state = GameManager.GameStates.Game;
    }


    public void SwitchItem(int changeIndex)  // Move left or right on the inventory
    {
        if (inventory.Count > 1)  // Change the inventory index
            inventoryIndex += changeIndex;
        if (inventoryIndex >= inventory.Count)
            inventoryIndex = 0;
        else if (inventoryIndex < 0)
            inventoryIndex = inventory.Count - 1;

        if (inventory.Count != 0)  // If the inventory isn't empty
        {
            InteractiveObject currentSelection = inventory[inventoryIndex].item;
            itemName.text = currentSelection.name;
            itemCount.text = "x" + itemDictionary[currentSelection].stackSize;
            itemDescription.text = currentSelection.description;
            equipButton.transform.parent.gameObject.SetActive(true);
            dropButton.transform.parent.gameObject.SetActive(true);
            if (itemDictionary[currentSelection].stackSize == 1)
                itemCount.gameObject.SetActive(false);
            else
                itemCount.gameObject.SetActive(true);


            if (displayItem != null)  // Destroy the 3D models
                Destroy(displayItem.gameObject); 
            if (displayItemL != null)
                Destroy(displayItemL.gameObject);
            if (displayItemR != null)
                Destroy(displayItemR.gameObject);

            displayItem = Instantiate(currentSelection.prefab, new Vector3(999.7f, 1000.1f, 1000.315f), Quaternion.identity).transform;  // Create the center 3D model
            displayItem.GetComponent<Rigidbody>().isKinematic = true;
            displayItem.Find("Outline").gameObject.SetActive(true);


            if (inventory.Count != 1)  // If there is more than one thing in the inventory
            {
                GameObject nItem;  // New 3D model

                if (inventoryIndex != 0)
                    nItem = inventory[inventoryIndex - 1].item.prefab;
                else
                    nItem = inventory[inventory.Count - 1].item.prefab;

                displayItemL = Instantiate(nItem, new Vector3(1000.82f, 1000, 1000.3f), Quaternion.identity).transform;   // Create the left 3D model
                displayItemL.GetComponent<Rigidbody>().isKinematic = true;

                if (inventoryIndex != inventory.Count - 1)
                    nItem = inventory[inventoryIndex + 1].item.prefab;
                else
                    nItem = inventory[0].item.prefab;

                displayItemR = Instantiate(nItem, new Vector3(999.39f, 1000, 999.31f), Quaternion.identity).transform;  // Create the right center 3D model
                displayItemR.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else  // If there is nothing in the inventory
        {
            itemName.text = "";
            itemCount.text = "";
            itemDescription.text = "";
            equipButton.transform.parent.gameObject.SetActive(false);
            dropButton.transform.parent.gameObject.SetActive(false);
            if (displayItem != null)
                Destroy(displayItem.gameObject);
            if (displayItemL != null)
            {
                Destroy(displayItemL.gameObject);
                Destroy(displayItemR.gameObject);
            }
        }

        if (dropPosition.childCount != 1)
            unequipButton.transform.parent.gameObject.SetActive(false);
        else
            unequipButton.transform.parent.gameObject.SetActive(true);
    }


    public void OnDrag(PointerEventData eventData)  // Rotating the 3D model
    {
        displayItem.eulerAngles += new Vector3(-eventData.delta.y, -eventData.delta.x);
    }


    public void EquipItem()  // Equip the selected item
    {
        if (inventory.Count != 0)
        {
            if (dropPosition.childCount != 0)
            {
                GameObject heldItem;
                for (int i = 0; i < dropPosition.childCount; i++)  // Destroy items in player hand
                {
                    heldItem = dropPosition.GetChild(i).gameObject;
                    Add(heldItem.GetComponent<PickUp>().thisObject);
                    Destroy(heldItem);
                }
            }

            GameObject newItem = Instantiate(inventory[inventoryIndex].item.prefab, dropPosition.position, dropPosition.rotation);
            newItem.GetComponent<Rigidbody>().isKinematic = true;
            newItem.GetComponent<Collider>().enabled = false;
            newItem.transform.parent = dropPosition;
            //DialogueManager.manager.PlaySound(inventory[inventoryIndex].item.sound);
            Remove(inventory[inventoryIndex].item);
            CloseInventory();
        }
    }


    public void UnequipItem()  // Unequip the held item
    {if (dropPosition.childCount != 0)
        {
            GameObject heldItem;
            heldItem = dropPosition.GetChild(0).gameObject;
            Add(heldItem.GetComponent<PickUp>().thisObject);  // Add it back to the inventory
            unequipButton.transform.parent.gameObject.SetActive(false);
            //DialogueManager.manager.PlaySound(heldItem.GetComponent<PickUp>().thisObject.sound);
            Destroy(heldItem);
            SwitchItem(0);
        }
    }


    public void DropItem()  // Drop the selected item on the ground
    {
        if (inventory.Count != 0)
        {
            Instantiate(inventory[inventoryIndex].item.prefab, dropPosition.position + new Vector3(0, 0, 0.1f), dropPosition.rotation);
            //DialogueManager.manager.PlaySound(inventory[inventoryIndex].item.sound);
            Remove(inventory[inventoryIndex].item);
            SwitchItem(0);
        }
    }


    public void Journal()  // Switch to the journal screen
    {
        CloseInventory();
        inventoryPanel.SetActive(false);
        journal.OpenJournal();
    }


    public void OnInventoryInput(InputAction.CallbackContext context)
    {
        if (context.performed && inventoryPanel.activeSelf)
        {
            if (GameManager.manager.state == GameManager.GameStates.Inventory) CloseInventory();
            else if (GameManager.manager.state == GameManager.GameStates.Game) OpenInventory();
        }
        GameManager.manager.controlType = context.control;
    }


    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.manager.state == GameManager.GameStates.Inventory)
        {
            if (context.ReadValue<float>() > 0)
                SwitchItem(1);
            else if (context.ReadValue<float>() < 0)
                SwitchItem(-1);
        }
        GameManager.manager.controlType = context.control;
    }

    public void OnRotateInput(InputAction.CallbackContext context)
    {
        if (GameManager.manager.controlType.device is Gamepad && GameManager.manager.state == GameManager.GameStates.Inventory && displayItem != null)
        {
            displayItem.eulerAngles += new Vector3(-context.ReadValue<Vector2>().y * Time.deltaTime * 7, -context.ReadValue<Vector2>().x * Time.deltaTime * 7);
        }
    }

    public void OnEquipInput(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.manager.state == GameManager.GameStates.Inventory)
        {
            EquipItem();
        }
    }

    public void OnUnequipInput(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.manager.state == GameManager.GameStates.Inventory)
        {
            UnequipItem();
        }
    }

    public void OnDropInput(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.manager.state == GameManager.GameStates.Inventory)
        {
            DropItem();
        }
    }

    public void OnExitInput(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.manager.state == GameManager.GameStates.Inventory)
        {
            CloseInventory();
        }
    }

    public void OnJournalInput(InputAction.CallbackContext context)
    {
        if (context.performed && !journal.journalPanel.activeSelf && GameManager.manager.state == GameManager.GameStates.Inventory)
        {
            journal.openFrame = true;
            Journal();
        }
    }
}
