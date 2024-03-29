using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TMP_Text))]
public class SetTextToTextBox : MonoBehaviour
{
    [TextArea(2, 3)]
    [SerializeField] private string message = "Press BUTTONPROMPT to interact.";

    [Header("Setup for sprites")]
    [SerializeField] private ListOfSpriteAssets listOfSpriteAssets;
    [SerializeField] private DeviceType deviceType;
    [SerializeField] private InputActions inputAction;

    private PlayerControls playerControls;
    private TMP_Text textBox;

    private void Awake()
    {
        playerControls = new PlayerControls();
        textBox = GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetText();
    }

    [ContextMenu(itemName:"Set Text")]
    private void SetText()
    {
        if((int)deviceType > listOfSpriteAssets.spriteAssets.Count-1)
        {
            Debug.Log(message: $"Missing Sprite Asset for {deviceType}");
            return;
        }

        switch (inputAction)
        {
            case InputActions.Look:
                textBox.text = SwapTextWithSprite.ReadAndReplaceBinding(message,
                    playerControls.Player.Look.bindings[(int)deviceType],
                    listOfSpriteAssets.spriteAssets[(int)deviceType]);
                break;

            case InputActions.Movement:
                textBox.text = SwapTextWithSprite.ReadAndReplaceBinding(message,
                    playerControls.Player.Movement.bindings[(int)deviceType],
                    listOfSpriteAssets.spriteAssets[(int)deviceType]);
                break;

            case InputActions.Jump:
                textBox.text = SwapTextWithSprite.ReadAndReplaceBinding(message,
                    playerControls.Player.Jump.bindings[(int)deviceType],
                    listOfSpriteAssets.spriteAssets[(int)deviceType]);
                break;

            case InputActions.Sprint:
                textBox.text = SwapTextWithSprite.ReadAndReplaceBinding(message,
                    playerControls.Player.Sprint.bindings[(int)deviceType],
                    listOfSpriteAssets.spriteAssets[(int)deviceType]);
                break;

            case InputActions.Crouch:
                textBox.text = SwapTextWithSprite.ReadAndReplaceBinding(message,
                    playerControls.Player.Crouch.bindings[(int)deviceType],
                    listOfSpriteAssets.spriteAssets[(int)deviceType]);
                break;

            case InputActions.Flashlight:
                textBox.text = SwapTextWithSprite.ReadAndReplaceBinding(message,
                    playerControls.Player.Flashlight.bindings[(int)deviceType],
                    listOfSpriteAssets.spriteAssets[(int)deviceType]);
                break;

            case InputActions.DropItem:
                textBox.text = SwapTextWithSprite.ReadAndReplaceBinding(message,
                    playerControls.Player.DropItem.bindings[(int)deviceType],
                    listOfSpriteAssets.spriteAssets[(int)deviceType]);
                break;

            case InputActions.PickUp:
                textBox.text = SwapTextWithSprite.ReadAndReplaceBinding(message,
                    playerControls.Player.PickUp.bindings[(int)deviceType],
                    listOfSpriteAssets.spriteAssets[(int)deviceType]);
                break;

            case InputActions.SeeTasks:
                textBox.text = SwapTextWithSprite.ReadAndReplaceBinding(message,
                    playerControls.Player.SeeTasks.bindings[(int)deviceType],
                    listOfSpriteAssets.spriteAssets[(int)deviceType]);
                break;

            case InputActions.OpenPager:
                textBox.text = SwapTextWithSprite.ReadAndReplaceBinding(message,
                    playerControls.Player.OpenPager.bindings[(int)deviceType],
                    listOfSpriteAssets.spriteAssets[(int)deviceType]);
                break;

            case InputActions.ConsumeItem:
                textBox.text = SwapTextWithSprite.ReadAndReplaceBinding(message,
                    playerControls.Player.ConsumeItem.bindings[(int)deviceType],
                    listOfSpriteAssets.spriteAssets[(int)deviceType]);
                break;

            case InputActions.Scroll:
                textBox.text = SwapTextWithSprite.ReadAndReplaceBinding(message,
                    playerControls.Player.Scroll.bindings[(int)deviceType],
                    listOfSpriteAssets.spriteAssets[(int)deviceType]);
                break;

            case InputActions.OpenBook:
                textBox.text = SwapTextWithSprite.ReadAndReplaceBinding(message,
                playerControls.Player.OpenBook.bindings[(int)deviceType],
                    listOfSpriteAssets.spriteAssets[(int)deviceType]);
                break;

            case InputActions.StopInspecting:
                textBox.text = SwapTextWithSprite.ReadAndReplaceBinding(message,
                playerControls.Player.StopInspecting.bindings[(int)deviceType],
                    listOfSpriteAssets.spriteAssets[(int)deviceType]);
                break;

            default:
                break;
        }
            
    }

    private enum DeviceType
    {
        Keyboard = 0,
        Gamepad = 1
    }

    private enum InputActions
    {
        Look = 0,
        Movement = 1,
        Jump = 2,
        Sprint = 3,
        Crouch = 4,
        Flashlight = 5,
        DropItem = 6,
        PickUp = 7,
        SeeTasks = 8,
        OpenPager = 9,
        ConsumeItem = 10,
        Scroll = 11,
        OpenBook = 12,
        StopInspecting = 13
    }
}
