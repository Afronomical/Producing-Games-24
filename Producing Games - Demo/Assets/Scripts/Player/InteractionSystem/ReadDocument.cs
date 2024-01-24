using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.EventSystems;

public class ReadDocument : MonoBehaviour
{
    public GameObject documentScreen;
    public GameObject physicalObject;
    public Document currentDocument;
    private Journal journal;
    private int pageIndex;
    public Image page;
    private bool plainText;
    //public Sound paperSound;

    [Header("References")]
    public GameObject plainTextPanel;
    public TMP_Text plainTextText;
    public TMP_Text documentNameText;
    public Image takeButton;
    public Image plainTextButton;
    public Image returnButton;
    public Image previousButton;
    public Image nextButton;

    [Header("Button Prompts")]
    public Sprite[] returnButtonSprites;
    public Sprite[] takeButtonSprites;
    public Sprite[] plainTextButtonSprites;


    private void OnEnable()
    {
        PickUpDocument.OnDocumentCollected += Open;
        journal = GetComponent<Journal>();
    }


    private void OnDisable()
    {
        PickUpDocument.OnDocumentCollected -= Open;
    }


    public void Open(Document documentData, GameObject obj)  // Open a document
    {
        Cursor.lockState = CursorLockMode.None;
        GameManager.manager.state = GameManager.GameStates.Document;
        documentScreen.SetActive(true);
        pageIndex = 0;
        currentDocument = documentData;
        page.sprite = currentDocument.pages[0];
        documentNameText.text = currentDocument.docName;
        plainText = false;
        plainTextPanel.SetActive(false);

        if (obj != null)  // If it has been picked up
        {
            physicalObject = obj;
            takeButton.transform.parent.gameObject.SetActive(true);
        }
        else  // If it has been opened from the journal
        {
            takeButton.transform.parent.gameObject.SetActive(false);
        }


        previousButton.gameObject.SetActive(false);  
        if (pageIndex < currentDocument.pages.Length - 1)  // If there is another page
            nextButton.gameObject.SetActive(true);
        else
            nextButton.gameObject.SetActive(false);

        // Button prompts
        if (GameManager.manager.controlType.device is Gamepad && Gamepad.current is XInputController)
        {
            returnButton.overrideSprite = returnButtonSprites[0];
            takeButton.overrideSprite = takeButtonSprites[0];
            plainTextButton.overrideSprite = plainTextButtonSprites[0];
        }
        else if (GameManager.manager.controlType.device is Gamepad)
        {
            returnButton.overrideSprite = returnButtonSprites[1];
            takeButton.overrideSprite = takeButtonSprites[1];
            plainTextButton.overrideSprite = plainTextButtonSprites[1];
        }
        else
        {
            returnButton.overrideSprite = returnButtonSprites[2];
            takeButton.overrideSprite = takeButtonSprites[2];
            plainTextButton.overrideSprite = plainTextButtonSprites[2];
        }
    }


    public void TurnPage(int indexChange)  // Turn the page
    {
        if (pageIndex + indexChange > -1 && pageIndex + indexChange < currentDocument.pages.Length)
        {
            pageIndex += indexChange;
            
            //if(indexChange != 0)
                //DialogueManager.manager.PlaySound(paperSound);
        }

        if (pageIndex == 0) previousButton.gameObject.SetActive(false);  // If this is the first page
        else previousButton.gameObject.SetActive(true);

        if (pageIndex + 1 == currentDocument.pages.Length)  // If this is the last page
            nextButton.gameObject.SetActive(false);
        else
            nextButton.gameObject.SetActive(true);

        page.sprite = currentDocument.pages[pageIndex];
        plainTextText.text = currentDocument.plainText[pageIndex];
    }


    public void Take()  // Add the document to the journal
    {
        if (physicalObject != null)
        {
            Destroy(physicalObject);
            physicalObject = null;
            journal.Add(currentDocument);
            Return();
        }
    }


    public void Return()  // Close the document reader
    {
        documentScreen.SetActive(false);
        currentDocument = null;

        if (physicalObject == null)  // If it is from the journal
        {
            journal.OpenJournal();  // Go back
        }
        else  // If it has been picked up
        {
            physicalObject = null;
            Cursor.lockState = CursorLockMode.Locked;
            GameManager.manager.state = GameManager.GameStates.Game;
        }
    }


    public void TogglePlainText()  // Toggle the plain text overlay
    {
        if (plainText)  // Turn off
        {
            plainText = false;
            plainTextPanel.SetActive(false);
        }
        else  // Turn on
        {
            plainText = true;
            plainTextText.text = currentDocument.plainText[pageIndex];
            plainTextPanel.SetActive(true);
        }
    }





    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.manager.state == GameManager.GameStates.Document)
        {
            if (context.ReadValue<float>() > 0)
                TurnPage(1);
            else if (context.ReadValue<float>() < 0)
                TurnPage(-1);
        }
    }


    public void OnReturnInput(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.manager.state == GameManager.GameStates.Document)
        {
            Return();
        }
    }


    public void OnTakeInput(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.manager.state == GameManager.GameStates.Document)
        {
            Take();
        }
    }

    public void OnPlainTextInput(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.manager.state == GameManager.GameStates.Document)
        {
            TogglePlainText();
        }
    }
}
