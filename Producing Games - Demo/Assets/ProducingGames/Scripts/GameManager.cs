using System;
using System.Collections;
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
    public Animation fadeAnim;

    [Header("Hour System")]
    [Range(1, 60)] public float hourLength = 60;
    [Range(1, 60)] public float shiftLength = 10;
    public int startingHour = 1;
    public int finalHour = 8;
    public Transform playerStartPosition;
    public StudyDoorInteractable studyDoor;

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
        Cursor.lockState = CursorLockMode.Confined;
        if (win)
            LevelManager.LoadScene(LevelManager.Scenes.WinScreen);
        else
            LevelManager.LoadScene(LevelManager.Scenes.LoseScreen);
    }


    private IEnumerator StartHour()
    {
        player.GetComponent<PlayerInput>().enabled = true;
        FadeIn();

        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = playerStartPosition.position;
        player.transform.rotation = playerStartPosition.rotation;
        player.GetComponent<CharacterController>().enabled = true;

        foreach (GameObject AI in NPCManager.Instance.patientList)  // Put all NPCs in bed
        {
            AI.GetComponent<PatientCharacter>().ChangePatientState(PatientCharacter.PatientStates.Bed);
        }

        if (demon) demon.GetComponent<DemonCharacter>().ChangeDemonState(DemonCharacter.DemonStates.Inactive);

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
                StartCoroutine(EndHour());
            }
        }
    }


    public IEnumerator EndHour()
    {
        currentHour++;
        studyDoor.collectible = studyDoor.startShiftSO;
        sanityEvents.EndHour();
        PatientTaskManager.instance.ClearTasks();

        if (currentHour <= finalHour)
        {
            player.GetComponent<PlayerInput>().enabled = false;
            FadeOut();
            yield return new WaitForSeconds(3);
            //if(EconomyManager.instance.boughtItems.Count > 0) EconomyManager.instance.SpawnItem();
            StartCoroutine(StartHour());  // Move to the next hour
        }
        else  // If the final hour just ended
        {
            EndGame(false);  // Lose the game
        }
    }


    public IEnumerator StartShift(Transform startShiftPosition)  // Called when the player leaves the study
    {
        inStudy = false;  // Starts the timer
        studyDoor.collectible = studyDoor.endHourSO;
        player.GetComponent<PlayerInput>().enabled = false;
        FadeOut();

        yield return new WaitForSeconds(3);

        FadeIn();
        player.GetComponent<PlayerInput>().enabled = true;
        player.GetComponent<CharacterController>().enabled = false;
        GameManager.Instance.player.transform.position = startShiftPosition.position;
        GameManager.Instance.player.transform.rotation = startShiftPosition.rotation;
        player.GetComponent<CharacterController>().enabled = true;
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
            demon.GetComponent<DemonCharacter>().ChangeDemonState(DemonCharacter.DemonStates.Patrol);

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
}
