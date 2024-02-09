using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager: MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player;
    //// AI references. 

    [Header("Hour System")]
    [Range(1, 60)] public float hourLength = 10;
    public int startingHour = 1;
    public int finalHour = 8;
    public Transform playerStartPosition;

    public int currentHour;
    public float currentTime;
    public bool shiftEndActive = false;

    [Header("Sanity")]
    [Range(0, 100)] public int startingSanity = 100;
    [Range(0, 100)] public int currentSanity;
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }


    private void Start()
    {
        StartGame();
    }


    private void LateUpdate()
    {
        UpdateTime();
    }


    public void StartGame()
    {
        currentSanity = startingSanity;
        currentHour = startingHour;
        StartHour();
    }


    public void EndGame()
    {
        ////if playerstate == win 
        ///win game 
        ///levelmanager.endgame or roll credits 
        ///
        ///if playerstate == lose 
        ///lose game. main menu or restart level 
    }


    private void StartHour()
    {
        // Fade?

        player.transform.position = playerStartPosition.position;
        player.transform.rotation = playerStartPosition.rotation;

        // Put Patients in bed

        currentTime = 0;
        shiftEndActive = false;

        PatientTaskManager.instance.SetHourlyTasks();
        PatientTaskManager.instance.SetRandomTasks();
    }


    private void UpdateTime()
    {
        currentTime += Time.deltaTime * (1 / hourLength);

        if (currentTime >= 60)
        {
            //StartShiftEnd();
            EndHour();
        }
    }


    public void EndHour()
    {
        currentHour++;
        PatientTaskManager.instance.ClearTasks();

        if (currentHour <= finalHour)
        {
            StartHour();
        }
        else 
        {
            EndGame();
        }
    }


    private void StartShiftEnd()
    {
        shiftEndActive = true;
        // Rage mode activate
    }
}
