using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class SetTextToTextBox : MonoBehaviour
{
    [TextArea(2, 3)]
    [SerializeField] private string message = "Press BUTTONPROMPT to interact.";

    [Header("Setup for sprites")]
    [SerializeField] private ListOfSpriteAssets listOfSpriteAssets;
    [SerializeField] private DeviceType deviceType;

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

        textBox.text = SwapTextWithSprite.ReadAndReplaceBinding(message,
            playerControls.Player.OpenPager.bindings[(int)deviceType],
            listOfSpriteAssets.spriteAssets[(int)deviceType]);
            
    }

    private enum DeviceType
    {
        Keyboard = 0,
        Gamepad = 1
    }
}
