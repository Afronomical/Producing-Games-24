using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public enum GameStates {Game, Inventory, Document, Keypad, Journal}
    public GameStates state;
    public InputControl controlType;

    [Header("Button Prompts")]
    public Image menuPrompt;
    public TMP_Text menuText;
    public GameObject inventory;
    public Sprite[] menuButtonSprites;

    void Start()
    {
        if (manager == null)
            manager = this;
    }

    private void Update()
    {
        if (state == GameStates.Game && controlType != null)  // Update the menu button prompt
        {
            menuPrompt.transform.parent.gameObject.SetActive(true);
            if (inventory.activeSelf)
                menuText.text = "Inventory";
            else
                menuText.text = "Journal";

            if (controlType.device is Gamepad && Gamepad.current is XInputController)  // Xbox Buttons
                menuPrompt.overrideSprite = menuButtonSprites[0];
            else if (controlType.device is Gamepad)  // Playstation Buttons
                menuPrompt.overrideSprite = menuButtonSprites[1];
            else  // Keyboard Buttons
                menuPrompt.overrideSprite = menuButtonSprites[2];
        }
        else
            menuPrompt.transform.parent.gameObject.SetActive(false);
    }
}
