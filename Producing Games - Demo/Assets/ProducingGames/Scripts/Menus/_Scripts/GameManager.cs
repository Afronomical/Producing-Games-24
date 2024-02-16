using System;
using System.Collections;
using UnityEngine;


/// <summary>
/// Written By: Aaron Moreland
/// 
/// Manages the main game loop and can be used to get important references such as the player and sanity level
/// </summary>


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

    [Header("Patients")]
    public GameObject demon; 
    private int patientCount;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }


    private void Start()
    {
        sanityEvents = GetComponent<SanityEventTracker>();
        patientCount = NPCManager.Instance.patientList.Count;
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


    public void EndGame(bool win)
    {
        Debug.Log("Game has ENDED");
        // <--- if win, show win screen
        // <--- else show lose screen
    }


    private IEnumerator StartHour()
    {
        // <--- Fade out
        // <--- Freeze player
        yield return new WaitForSeconds(0);
        // <--- Unfreeze player
        // <--- Fade in

        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = playerStartPosition.position;
        player.transform.rotation = playerStartPosition.rotation;
        player.GetComponent<CharacterController>().enabled = true;

        foreach (GameObject AI in NPCManager.Instance.patientList)  // Put all NPCs in bed
        {
            //AI.GetComponent<AICharacter>().ChangeState(AICharacter.States.Bed);

            //if (AI.GetComponent<AICharacter>().isPossessed)  <------ Leave Rage mode
            //    AI.GetComponent<AICharacter>().

        }

        // demon. <-- Do the same for demon

        currentTime = 0;
        inStudy = true;
        shiftEndActive = false;

        PatientTaskManager.instance.SetHourlyTasks();
        PatientTaskManager.instance.SetRandomTasks();

        yield return new WaitForSeconds(0);
    }


    private void UpdateTime()
    {
        if (!inStudy)  // If the player is not in the study
        {
            currentTime += Time.deltaTime * (1 / hourLength);  // Increment the minutes timer

            if (!shiftEndActive && currentTime >= shiftLength)  // When the shift ends
            {
                StartShiftEnd();  // Start shift end phase
            }

            if (currentTime >= 60)  // If the player has been out for the whole hour
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
            StartCoroutine(StartHour());  // Move to the next hour
        }
        else  // If the final hour just ended
        {
            EndGame(false);  // Lose the game
        }
    }


    public void StartShift()  // Called when the player leaves the study
    {
        inStudy = false;  // Starts the timer
    }


    private void StartShiftEnd()  // When the player has been out for too long
    {
        shiftEndActive = true;
        
        /*
        foreach(GameObject AI in NPCManager.Instance.NPCS)
        {
            if (AI.GetComponent<AICharacter>().isPossessed) <--- ENTER RAGE MODE HERE
                AI.GetComponent<AICharacter>().
        }*/

        // <--- Lock patient doors
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
        if (patientCount <= 0)  // If all patients are dead
        {
            EndGame(false);  // Lose the game
        }
    }
}
