using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.EventSystems;
using TMPro;

public class Journal : MonoBehaviour
{
    public List<Document> documents = new List<Document>();
    private Transform[] documentSlots = new Transform[10];
    [HideInInspector] public bool openFrame;  // When the journal is opened
    public Transform slots;
    public GameObject menuPanel;
    public GameObject journalPanel;
    private Inventory inventory;
    private ReadDocument readDocument;

    [Header("Button Prompts")]
    public Image closeButton;
    public Image readButton;
    public Image inventoryButton;
    public Sprite[] closeButtonSprites;
    public Sprite[] inventoryButtonSprites;
    public Sprite[] readButtonSprites;

    void Start()
    {
        for (int i = 0; i < slots.childCount; i++)
        {
            documentSlots[i] = (slots.GetChild(i));
        }

        journalPanel.SetActive(false);
        inventory = GetComponent<Inventory>();
        readDocument = GetComponent<ReadDocument>();
    }



    public void Add(Document newDocument)  // Add a new document to the inventory
    {
        documents.Add(newDocument);
    }


    public void OpenDocument()  // Close the inventory and read the selected document
    {
        CloseJournal();
        //DialogueManager.manager.PlaySound(EventSystem.current.currentSelectedGameObject.GetComponent<Interactable>().thisDocument.sound);
        readDocument.Open(EventSystem.current.currentSelectedGameObject.GetComponent<Interactable>().thisDocument, null);
    }


    public void OpenJournal()  // Open the inventory page
    {
        int i = 0;
        foreach (Document d in documents)  // Set the buttons to the correct documents
        {
            if (i < documentSlots.Length)
            {
                documentSlots[i].GetComponentsInChildren<Image>()[1].sprite = d.pages[0];
                documentSlots[i].GetComponent<Interactable>().thisDocument = d;
                documentSlots[i].GetComponentInChildren<TMP_Text>().text = d.docName;
                documentSlots[i].gameObject.SetActive(true);
            }

            i++;
        }

        // Button prompts
        if (GameManager.manager.controlType.device is Gamepad && Gamepad.current is XInputController)  // Xbox Buttons
        {
            closeButton.overrideSprite = closeButtonSprites[0];
            inventoryButton.overrideSprite = inventoryButtonSprites[0];
            readButton.overrideSprite = readButtonSprites[0];
        }
        else if (GameManager.manager.controlType.device is Gamepad)  // Playstation Buttons
        {
            closeButton.overrideSprite = closeButtonSprites[1];
            inventoryButton.overrideSprite = inventoryButtonSprites[1];
            readButton.overrideSprite = readButtonSprites[1];
        }
        else
        {
            closeButton.overrideSprite = closeButtonSprites[2];
            inventoryButton.overrideSprite = inventoryButtonSprites[2];
            readButton.overrideSprite = readButtonSprites[2];
        }


        menuPanel.SetActive(true);
        journalPanel.SetActive(true);
        inventory.inventoryPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        if (documents.Count != 0)
            EventSystem.current.SetSelectedGameObject(documentSlots[0].gameObject, new BaseEventData(EventSystem.current));
        GameManager.manager.state = GameManager.GameStates.Journal;
    }


    public void CloseJournal()  // Close the journal page
    {
        int i = 0;
        foreach (Document d in documents)
        {
            if (i < documentSlots.Length)
            {
                documentSlots[i].gameObject.SetActive(false);
            }

            i++;
        }

        menuPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.manager.state = GameManager.GameStates.Game;
    }


    public void Inventory()  // Switch from journal to inventory
    {
        CloseJournal();
        journalPanel.SetActive(false);
        inventory.OpenInventory();
    }





    public void OnJournalInput(InputAction.CallbackContext context)
    {
        if (context.performed && journalPanel.activeSelf)
        {
            if (GameManager.manager.state == GameManager.GameStates.Journal) CloseJournal();
            else if (GameManager.manager.state == GameManager.GameStates.Game) OpenJournal();
        }
    }


    public void OnSelectInput(InputAction.CallbackContext context)
    {
        if (context.performed && journalPanel.activeSelf && GameManager.manager.state == GameManager.GameStates.Journal)
        {
            OpenDocument();
        }
    }


    public void OnExitInput(InputAction.CallbackContext context)
    {
        if (context.performed && journalPanel.activeSelf && GameManager.manager.state == GameManager.GameStates.Journal)
        {
            CloseJournal();
        }
    }

    public void OnInventoryInput(InputAction.CallbackContext context)
    {
        if (context.performed && !inventory.inventoryPanel.activeSelf && !openFrame && GameManager.manager.state == GameManager.GameStates.Journal)
        {
            Inventory();
        }
        if (context.canceled && GameManager.manager.state == GameManager.GameStates.Journal)
            openFrame = false;
}
}
