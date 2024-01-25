using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;


public class Interactor : MonoBehaviour
{
    public float interactRange;
    private Transform highlightedObject, outline;
    public GameObject tooltip;
    public Image buttonPrompt;
    public GameObject tooltipName, tooltipPrompt;
    private bool input;
    private InputControl control;


    void Update()
    {
        if (GameManager.manager.state == GameManager.GameStates.Game)
        {
            if (highlightedObject != null)
            {
                outline.gameObject.SetActive(false);
                tooltip.SetActive(false);
                highlightedObject = null;
            }

            RaycastHit hit;
            if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactRange))  // Raycast forwards
            {
                highlightedObject = hit.transform;
                if (highlightedObject.TryGetComponent(out Interactable interact))  // If it has a component that inherits from interactable
                {

                    if (outline = highlightedObject.transform.Find("Outline"))  // Find the outline object
                    {
                        outline.gameObject.SetActive(true);
                        tooltip.SetActive(true);
                        if (interact.thisObject != null)  // If it is an object
                        {
                            tooltipName.GetComponent<TMPro.TMP_Text>().text = interact.thisObject.objectName;  // GET INFO--------------------------------------------
                            tooltipPrompt.GetComponent<TMPro.TMP_Text>().text = interact.thisObject.tooltipText;
                            
                            if (GameManager.manager.controlType.device is Gamepad && Gamepad.current is XInputController)  // Xbox Buttons
                                buttonPrompt.overrideSprite = interact.thisObject.contImage;
                            else if (GameManager.manager.controlType.device is Gamepad)  // Playstation Buttons
                                buttonPrompt.overrideSprite = interact.thisObject.psImage;
                            else  // Keyboard Buttons
                                buttonPrompt.overrideSprite = interact.thisObject.kbImage;
                        }

                        else // If it is a document
                        {
                            tooltipName.GetComponent<TMPro.TMP_Text>().text = interact.thisDocument.docName;  // GET INFO--------------------------------------------
                            tooltipPrompt.GetComponent<TMPro.TMP_Text>().text = interact.thisDocument.tooltipText;

                            // Update button prompts
                            if (GameManager.manager.controlType.device is Gamepad && Gamepad.current is XInputController)  // Xbox Buttons
                                buttonPrompt.overrideSprite = interact.thisDocument.contImage;
                            else if (GameManager.manager.controlType.device is Gamepad)  // Playstation Buttons
                                buttonPrompt.overrideSprite = interact.thisDocument.psImage;
                            else  // Keyboard Buttons
                                buttonPrompt.overrideSprite = interact.thisDocument.kbImage;
                        }

                        if (input)  // If the player has pressed the interact button
                        {
                            interact.Interact();
                            tooltip.SetActive(false);
                        }
                    }
                }
                else
                {
                    highlightedObject = null;
                }
            }

            input = false;
        }
    }


    public void InteractInput(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.manager.state == GameManager.GameStates.Game)
            input = true;
    }
}