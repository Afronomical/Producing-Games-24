using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager: MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player;
    //// AI references. 

    [Range(1, 60)] public float hourLength;
    public int startingHour = 1;
    public int finalHour = 8;
    public Transform playerStartPosition;

    public int currentHour;
    public float currentTime;
    

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {

    }
   

    public void StartGame()
    {
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

        currentTime = 0;
    }


    private void UpdateTime()
    {
        currentTime += Time.deltaTime * (60 / hourLength);

        if (currentTime >= 60)
        {
            StartShiftEnd();
        }
    }


    public void EndHour()
    {
        currentHour++;

        if (currentHour > finalHour)
        {
            EndGame();
        }
    }


    private void StartShiftEnd()
    {

    }
}
