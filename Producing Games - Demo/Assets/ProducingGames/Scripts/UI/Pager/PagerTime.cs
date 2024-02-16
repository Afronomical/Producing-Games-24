using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PagerTime : MonoBehaviour
{
    public TMP_Text clock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayTime();
    }

    private void DisplayTime()
    {
        string time = "0";
        time += GameManager.Instance.currentHour.ToString();

        time += ":";
        if (GameManager.Instance.currentTime < 10)
        {
            time += "0";
        }
        time += (int)GameManager.Instance.currentTime;
        time += "AM";

        clock.text = time;
    }
}
