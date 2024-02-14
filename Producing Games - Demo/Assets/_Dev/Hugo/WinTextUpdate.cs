using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class winTextUpdate : MonoBehaviour
{
    public TextMeshProUGUI textMeshProText;

    void Start()
    {
        //get variable from PlayerPrefs 

        //***** need chosenDemon.DemonName to be saved using playerprefs in NPCManager script ******
        string winText = PlayerPrefs.GetString("ChosenDemonName", "the Demon");

      //update the text
        if (textMeshProText != null)
        {
            textMeshProText.text = "You Exorcised " + winText + "!";
        }
    }
}
