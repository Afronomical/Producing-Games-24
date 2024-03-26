using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// <summary>
//
//

public class PagerDisplay : MonoBehaviour
{
    [Header("Messages")]
    public TMP_Text alert;
    public int textWidth = 10;
    private string messageArray;
    private List<string> messageList = new List<string>();
    private float timer = 0;
    private string lastMessage;

    [Header("Time")]
    public TMP_Text clock;

    public static PagerDisplay instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        alert.gameObject.SetActive(false);
        alert.overflowMode = TextOverflowModes.Truncate;
        //alert.text = " ";

        DisplayMessage("This is a test!", 20);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (!alert.IsActive()) DisplayTime();
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

        clock.text = time.ToUpper();
        clock.enabled = true;
    }

    public void DisplayMessage(string message, float displayTime)
    {
        clock.enabled = false;
        alert.gameObject.SetActive(true);

        lastMessage = message;
        SendText(message);
        timer = displayTime;

        //Scroll text across pager screen
        StartCoroutine(ScrollText(0.2f));

        GetComponent<PagerController>().ActiveNotif = true;

        messageList.Add(message);
    }

    private IEnumerator ScrollText(float duration)
    {

        while (timer > 0)
        {
            while (messageArray.Length > 0)
            {
                yield return new WaitForSeconds(duration);
                messageArray = messageArray.Substring(1);
                alert.text = messageArray;
            }

            SendText(lastMessage);
        }

        alert.gameObject.SetActive(false);
        GetComponent<PagerController>().ActiveNotif = false;

    }

    private void SendText(string message)
    {
        messageArray = new String(' ', textWidth) + message.ToUpper();
    }
}