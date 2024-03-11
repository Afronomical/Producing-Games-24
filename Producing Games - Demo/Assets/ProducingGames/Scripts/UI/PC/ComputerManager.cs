using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComputerManager : MonoBehaviour
{
    public static ComputerManager instance;
    public TMP_Text clock;
    public GameObject cameraScreen;
    public GameObject shopScreen;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        cameraScreen.SetActive(false);
    }

    public void OpenCameras()
    {
        cameraScreen.SetActive(true);
    }

    public void OpenShop()
    {
        shopScreen.SetActive(true);
    }

    public void Patients()
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
