using System;
using System.Collections;
using System.Diagnostics.Tracing;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Written By: Aaron Moreland
/// 
/// Manages the main game loop and can be used to get important references such as the player and sanity level
/// </summary>


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player;
    public bool tutorial;
    public Animation fadeAnim;

    [Header("Hour System")]
    [Range(1, 60)] public float hourLength = 60;
    [Range(1, 60)] public float shiftLength = 10;
    public int startingHour = 1;
    public int finalHour = 8;
    public Transform playerStartPosition;
    public ShiftChange studyDoor, studyBed;

    public int currentHour;
    public float currentTime;
    public bool inStudy, shiftEndActive;

    public bool exorcismFailed = false;

    [Header("Sanity")]
    [Range(0, 100)] public int startingSanity = 100;
    private int currentSanity;
    public SanityEventTracker.SanityLevels sanityLevel;
    [HideInInspector] public SanityEventTracker sanityEvents;

    [Header("Patients")]
    public GameObject demon; 
    private int patientCount;

    [Header("Object References")]
    public GameObject altar;
    public GameObject jug;
    public bool playerHasJug = false;

    [Header("Dynamic Event Chances")] //Chance of dynamic event activating based on player sanity
    [Range(0,100)] public int eventChance = 100;
    public int saneChance;
    public int deleriousChance;
    public int derrangedChance;
    public int hystericalChance;
    public int madnessChance;
    //public bool eventTriggered;
    [Space]
    public GameObject captureBox;
    private CapturedBox captureBoxScript;
    private DynamicEventBool DynamicEventBool;

    public GameObject jumpscareTimelineGO;
    private CinematicMangerScript cinematicManagerScript;
    private int cineChance;

    public int salary; //Money received after shift

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }


    private void Start()
    {
        StartGame();
    }


    private void LateUpdate()
    {
        UpdateTime();
        DynamicEventChance();
    }


    public void StartGame()
    {        
        currentSanity = startingSanity;
        currentHour = startingHour;
        StartHour();

        sanityEvents = GetComponent<SanityEventTracker>();
        patientCount = NPCManager.Instance.patientList.Count;
        altar = FindFirstObjectByType<ExorcismTable>().gameObject;
        captureBoxScript = captureBox.GetComponent<CapturedBox>();
        cinematicManagerScript=jumpscareTimelineGO.GetComponent<CinematicMangerScript>();
        //jug = FindFirstObjectByType<PickUpJug>().gameObject;
        CommandConsole.Instance.IncrementTime += IncrementTimeBy5;
        CommandConsole.Instance.EndHour += EndHourCommand;
    }


    public void EndGame(bool win)
    {
        Cursor.lockState = CursorLockMode.Confined;
        if (win)
            LevelManager.LoadScene(LevelManager.Scenes.WinScreen);
        else
            LevelManager.LoadScene(LevelManager.Scenes.LoseScreen);
    }


    private void StartHour()
    {
        player.GetComponent<PlayerInput>().enabled = true;
        FadeIn();

        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = playerStartPosition.position;
        player.transform.rotation = playerStartPosition.rotation;
        player.GetComponent<CharacterController>().enabled = true;

        foreach (GameObject AI in NPCManager.Instance.patientList)
        {
            PatientCharacter patientCharacter = AI.GetComponent<PatientCharacter>();

            // INFO: Set the current states to none ready for the next hour
            patientCharacter.currentState = PatientCharacter.PatientStates.None;

            // Put all NPCs in bed
            patientCharacter.ChangePatientState(PatientCharacter.PatientStates.Bed);
        }

        if (demon)
        {
            DemonCharacter demonCharacter = demon.GetComponent<DemonCharacter>();

            demonCharacter.SetInRageMode(false);
            demonCharacter.ChangeDemonState(DemonCharacter.DemonStates.Inactive);
        }
  
        

        currentTime = 0;
        inStudy = true;
        shiftEndActive = false;

        if (tutorial && TutorialTaskManager.instance != null)
        {
            TutorialTaskManager.instance.SetHourlyTasks();
            TutorialTaskManager.instance.SetPlayerTask();
            TutorialTaskManager.instance.SetRandomTasks();
        }
        else
        {
            PatientTaskManager.instance.SetHourlyTasks();
            PatientTaskManager.instance.SetPlayerTask();
            PatientTaskManager.instance.SetRandomTasks();
        }

        if (DynamicEventBool)
            DynamicEventBool.resetDynamicEventBool();
    }

    public void InitializeCheats()
    {

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
                StartCoroutine(EndHour());
            }
        }
    }
    private void IncrementTimeBy5()
    {
        currentTime += 5;
    }
    private void EndHourCommand()
    {
        StartCoroutine(EndHour());
    }
    public IEnumerator EndHour()
    {
        currentHour++;
        //studyDoor.collectible = studyDoor.startShiftSO;
        studyBed.ChangeShift();
        sanityEvents.EndHour();
        PatientTaskManager.instance.ClearTasks();
        EconomyManager.instance.AddIncome(salary);

        if (currentHour <= finalHour)
        {
            player.GetComponent<PlayerInput>().enabled = false;
            FadeOut();
            yield return new WaitForSeconds(3);
            //if(EconomyManager.instance.boughtItems.Count > 0) EconomyManager.instance.SpawnItem();
            StartHour();  // Move to the next hour
        }
        else  // If the final hour just ended
        {
            EndGame(false);  // Lose the game
        }
    }


    public void OpenDoor(Transform startShiftPosition)
    {
        StartCoroutine(StartShift(startShiftPosition));
    }
    public void CloseDoor()
    {
        StartCoroutine(EndHour());
    }


    public IEnumerator StartShift(Transform startShiftPosition)  // Called when the player leaves the study
    {
        inStudy = false;  // Starts the timer
        studyDoor.ChangeShift();
        /*studyDoor.collectible = studyDoor.endHourSO;
        player.GetComponent<PlayerInput>().enabled = false;
        FadeOut();

        yield return new WaitForSeconds(3);

        FadeIn();
        player.GetComponent<PlayerInput>().enabled = true;
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = startShiftPosition.position;
        player.transform.rotation = startShiftPosition.rotation;
        player.GetComponent<CharacterController>().enabled = true;*/
        yield return new WaitForSeconds(0);
    }


    private void StartShiftEnd()  // When the player has been out for too long
    {
        shiftEndActive = true;
        foreach(GameObject character in NPCManager.Instance.patientList)
        {
            if (character.GetComponent<PatientCharacter>().isPossessed)
                character.GetComponent<PatientCharacter>().ChangePatientState(PatientCharacter.PatientStates.Possessed);
        }

        if (demon != null)
        {
            DemonCharacter demonCharacter = demon.GetComponent<DemonCharacter>();

            demonCharacter.SetInRageMode(true);
            demonCharacter.ChangeDemonState(DemonCharacter.DemonStates.Patrol);
        }

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

    public void FadeOut()
    {
        fadeAnim.Play("FadeIn");
    }

    public void FadeIn()
    {
        fadeAnim.Play("FadeOut");
    }

    public void DynamicEventChance()
    {
        if (sanityLevel == SanityEventTracker.SanityLevels.Sane)
           eventChance = saneChance;
        else if (sanityLevel == SanityEventTracker.SanityLevels.Delirious)
            eventChance = deleriousChance;
        else if (sanityLevel == SanityEventTracker.SanityLevels.Derranged)
            eventChance = derrangedChance;
        else if (sanityLevel == SanityEventTracker.SanityLevels.Hysterical)
            eventChance = hystericalChance;
        else if (sanityLevel == SanityEventTracker.SanityLevels.Madness)
            eventChance = madnessChance;
    }

    public void DemonCaptureEvent()
    {
       cineChance=UnityEngine.Random.Range(0, 3);
       if (cineChance==1)
       {
        StartCoroutine(captureBoxScript.MainEvent());
        }
        else if(cineChance==2)
        {
            cinematicManagerScript.StartBasementJumpscare();
        }
       else
        {
           cinematicManagerScript.StartHallwayJumpscare();
        }
       

    }

    /// <summary>
    /// Used to activate demon regardless of whether shift has ended or not
    /// </summary>
    /// <param name="demonState">The demon you want the state to be in when they spawn</param>
    /// <param name="spawnLocation">The location at which you want the demon to spawn at</param>
    public void ActivateDemon(DemonCharacter.DemonStates demonState, Vector3 spawnLocation = default)
    {
        if (demon != null)
        {
            DemonCharacter demonCharacter = demon.GetComponent<DemonCharacter>();

            demon.SetActive(true);
            demonCharacter.ChangeDemonState(demonState);
            demonCharacter.SetInRageMode(true);

            // INFO: If a spawn location has not been provided, spawn them by the possessed patients bed
            if (spawnLocation == default)
            {
                foreach (GameObject character in NPCManager.Instance.patientList)
                {
                    if (character.GetComponent<PatientCharacter>().isPossessed)
                        demonCharacter.agent.Warp(character.GetComponent<PatientCharacter>().bed.transform.position);
                }
            }
            else
                demonCharacter.agent.Warp(spawnLocation);
        }
    }

    /// <summary>
    /// Deactivates the demon whenever called
    /// </summary>
    public void DeactivateDemon()
    {
        if (demon != null)
        {
            DemonCharacter demonCharacter = demon.GetComponent<DemonCharacter>();

            demonCharacter.SetInRageMode(false);
            demonCharacter.ChangeDemonState(DemonCharacter.DemonStates.Inactive);
        }
    }
}
