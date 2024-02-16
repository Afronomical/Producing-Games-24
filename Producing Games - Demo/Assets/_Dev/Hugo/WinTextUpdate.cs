using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinTextUpdate : MonoBehaviour
{
    public TextMeshProUGUI textMeshProText;

    void Start()
    {
       

        string winText = NPCManager.Instance.ChosenDemon.DemonName;

      //update the text
        if (textMeshProText != null)
        {
            textMeshProText.text = "You Exorcised " + winText + "!";
        }
    }
}
