using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoseTextUpdate : MonoBehaviour
{
    public TextMeshProUGUI textMeshProText;

    void Start()
    {
       

       
        string demonName = NPCManager.Instance.ChosenDemon.DemonName;

      

        if (GameManager.Instance.currentHour >= 8)
        {

            textMeshProText.text = "Ran out of time!";

        }

        else
        {
            textMeshProText.text = demonName + " eviscerated all the patients";
        }

        
    }
}
