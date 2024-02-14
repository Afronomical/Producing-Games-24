using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player;

    [Header("Hour System")]
    [Range(1, 60)] public float hourLength = 60;
    [Range(1, 60)] public float shiftLength = 10;
    public int startingHour = 1;
    public int finalHour = 8;
    public Transform playerStartPosition;

    public int currentHour;
    public float currentTime;
    public bool inStudy, shiftEndActive;

    [Header("Sanity")]
    [Range(0, 100)] public int startingSanity = 100;
    private int currentSanity;
    public SanityEventTracker.SanityLevels sanityLevel;
    [HideInInspector] public SanityEventTracker sanityEvents;

    private int patientCount;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }


    private void Start()
    {
        sanityEvents = GetComponent<SanityEventTracker>();
        patientCount = NPCManager.Instance.GetPatientCount();
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
        StartCoroutine(StartHour());
    }


    public void EndGame()
    {
        Debug.Log("Game has ENDED");
        ////if playerstate == win 
        ///win game 
        ///levelmanager.endgame or roll credits 
        ///
        ///if playerstate == lose 
        ///lose game. main menu or restart level 
    }


    private IEnumerator StartHour()
    {
        // Fade out

        Time.timeScale = 0;
        yield return new WaitForSeconds(0);
        Time.timeScale = 1;

        // Fade in

        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = playerStartPosition.position;
        player.transform.rotation = playerStartPosition.rotation;
        player.GetComponent<CharacterController>().enabled = true;

        foreach (GameObject AI in NPCManager.Instance.NPCS)  // Put all NPCs in bed
        {
            AI.GetComponent<AICharacter>().ChangeState(AICharacter.States.Bed);

            //if (AI.GetComponent<AICharacter>().isPossessed)  <------ Leave Rage mode
            //    AI.GetComponent<AICharacter>().

        }

        currentTime = 0;
        inStudy = true;
        shiftEndActive = false;

        PatientTaskManager.instance.SetHourlyTasks();
        PatientTaskManager.instance.SetRandomTasks();
    }


    private void UpdateTime()
    {
        if (!inStudy)
        {
            currentTime += Time.deltaTime * (1 / hourLength);

            if (!shiftEndActive && currentTime >= shiftLength)  // When the shift ends
            {
                StartShiftEnd();  // Start shift end phase
            }

            if (currentTime >= 60)
            {
                EndHour();
            }
        }
    }


    public void EndHour()
    {
        currentHour++;
        sanityEvents.EndHour();
        PatientTaskManager.instance.ClearTasks();

        if (currentHour <= finalHour)
        {
            StartCoroutine(StartHour());
        }
        else 
        {
            EndGame();
        }
    }


    public void StartShift()
    {
        inStudy = false;
    }


    private void StartShiftEnd()
    {
        shiftEndActive = true;
        
        /*
        foreach(GameObject AI in NPCManager.Instance.NPCS)
        {
            if (AI.GetComponent<AICharacter>().isPossessed) <---- ENTER RAGE MODE HERE
                AI.GetComponent<AICharacter>().
        }*/

        // Lock patient doors
    }



    public int GetSanity()
    {
        return currentSanity;
    }

    public void AddSanity(int add)
    {
        currentSanity = Mathf.Clamp(currentSanity + add, 0, startingSanity);
        sanityEvents.ChangeSanity(currentSanity);
    }

    public void DecrementRemainingPatients() 
    { 
        patientCount--; 
        CheckRemainingPatients();
    }

    private void CheckRemainingPatients()
    {
        if (patientCount <= 0)
        {
            EndGame();
        }
    }
}
