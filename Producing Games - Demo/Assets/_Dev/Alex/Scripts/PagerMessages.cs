using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PagerMessages : MonoBehaviour
{
    public TMP_Text alert;
    private float timer = 0;

    public static PagerMessages instance;

    private void Start()
    {
        if (instance == null)
            instance = this;

        alert.text = " ";
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            alert.text = " ";
        }
    }

    public void DisplayMessage(string message, float displayTime)
    {
        timer = displayTime;
        alert.text = message;
    }
}
