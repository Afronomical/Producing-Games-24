using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// <summary>
//
//

public class PagerDisplay : MonoBehaviour
{
    [Header("Messages")]
    public TMP_Text alert;
    private char[] messageArray;
    private List<string> messageList = new List<string>();
    private float timer = 0;

    //Scrolling text
    //private Vector3 originalPos;
    //private Vector3 currentPos;
    //private Vector3 targetPos;

    [Header("Time")]
    public TMP_Text clock;


    public static PagerDisplay instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        //originalPos = alert.transform.position;
        //currentPos = originalPos;
        //targetPos = alert.transform.position - new Vector3(-1.5f, 0, 0);
    }

    private void Start()
    {
        alert.gameObject.SetActive(false);
        alert.text = " ";

        DisplayMessage("This is a test for the scrolling text functionality!", 10);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            alert.text = " ";
        }

        if(!alert.IsActive()) DisplayTime();

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
        clock.enabled = true;
    }

    public void DisplayMessage(string message, float displayTime)
    {
        clock.enabled = false;
        alert.gameObject.SetActive(true);

        //Scroll text across pager screen
        StartCoroutine(ScrollText(1));

        timer = displayTime;
        messageArray = message.ToCharArray();
        foreach(char c in messageArray)
        {
            messageList.Add(c.ToString());
        }
        //messageArray.Capacity = 20;
        alert.text = messageArray.ToString();
    }

    private IEnumerator ScrollText(float duration)
    {
        //currentPos = Vector3.MoveTowards(originalPos, targetPos, 0.1f);
        //yield return new WaitForSeconds(duration/10);
        //if(duration <= 0) { currentPos = originalPos; }

        alert.overflowMode = TextOverflowModes.Truncate;
        //alert.text =

        yield return new WaitForSeconds(duration);
        
        messageList.RemoveAt(0);
    }
}
