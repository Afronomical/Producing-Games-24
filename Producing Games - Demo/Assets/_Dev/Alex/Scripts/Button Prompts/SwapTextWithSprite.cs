using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public static class SwapTextWithSprite
{
   public static string ReadAndReplaceBinding(string textToDisplay, InputBinding actionNeeded, TMP_SpriteAsset spriteAsset)
    {
        string stringButtonName = actionNeeded.ToString();
        stringButtonName = RenameInput(stringButtonName);

        textToDisplay = textToDisplay.Replace(
            oldValue:"BUTTONPROMPT", newValue: $"<sprite=\"{spriteAsset.name}\" name =\"{stringButtonName}\">");

        return textToDisplay;
    }

    private static string RenameInput(string stringButtonName)
    {
        stringButtonName = stringButtonName.Replace(
            oldValue:"Interact:", newValue: String.Empty);

        stringButtonName = stringButtonName.Replace(
            oldValue: "<Keyboard>/", newValue: "Keyboard_");

        stringButtonName = stringButtonName.Replace(
            oldValue: "<Gamepad>/", newValue: "Gamepad_");

        return stringButtonName;
    }
}
