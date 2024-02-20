using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Written by: Matej Cincibus
/// Moderated by: ...
/// 
/// Handles all the functionality specific to patient NPCs
/// </summary>

[RequireComponent(typeof(PatientInteractor))]

public class PatientCharacter : AICharacter
{
    public enum PatientStates
    {
        None,

        Escorted,
        Wandering,
        Abandoned,
        Possessed,
        Dead,
        Bed,
        Praying,
        Hiding,
        Hungry,
        ReqMeds
    }

    [Header("Patient Settings")]
    public PatientStates currentState;
    public int startingHealth = 100;
    public int currentHealth;
    public float startingSanity = 100;
    public float currentSanity;
    public bool isPossessed = false;

    [Header("Components")]
    public PatientStateBaseClass patientStateScript;
    public DemonItemsSO demon;
    public GameObject bed;

    public override void Start()
    {
        base.Start();

        characterType = CharacterTypes.Patient;

        currentHealth = startingHealth;
        currentSanity = startingSanity;

        ChangePatientState(PatientStates.Abandoned); //INFO: Starting State

        if (isPossessed)
        {
            // INFO: Retrieves the scriptable object of the chosen demon
            demon = NPCManager.Instance.ChosenDemon;
            InitialiseDemonStats();

            // INFO: Instantiates the demon and saves it on the game manager so it can be used elsewhere
            GameObject GO = Instantiate(demon.demonPrefab, bed.transform.position, Quaternion.identity);
            GameManager.Instance.demon = GO;
        }
    }

    private void Update()
    {
        if (patientStateScript != null)
            patientStateScript.UpdateLogic();  // Calls the virtual function for whatever state scripts

        // INFO: Monitors health to check whether patient has died
        if (currentHealth <= 0)
            ChangePatientState(PatientStates.Dead);
    }

    public void ChangePatientState(PatientStates newState)  // Will destroy the old state script and create a new one
    {

        if (currentState != newState || patientStateScript == null)
        {
            // INFO: If the previous state had the patient remain stationary, we will need to grant the patient
            // movement again for the new state that they're going to go into
            if (currentState == PatientStates.Bed || currentState == PatientStates.ReqMeds) agent.enabled = true;

            if (patientStateScript != null)
                Destroy(patientStateScript); // destroy current script attached to AI character

            //set the current state of AI character to the new state
            currentState = newState;

            patientStateScript = newState switch
            {
                PatientStates.Possessed => transform.AddComponent<PossessedState>(),
                PatientStates.Escorted => transform.AddComponent<EscortedState>(),
                PatientStates.Abandoned => transform.AddComponent<AbandonedState>(),
                PatientStates.Bed => transform.AddComponent<BedState>(),
                PatientStates.Wandering => transform.AddComponent<WanderingState>(),
                PatientStates.Dead => transform.AddComponent<DeadState>(),
                PatientStates.Praying => transform.AddComponent<PrayerState>(),
                PatientStates.Hiding => transform.AddComponent<HidingState>(),
                PatientStates.Hungry => transform.AddComponent<HungryState>(),
                PatientStates.ReqMeds => transform.AddComponent<RequestMedicationState>(),
                PatientStates.None => null,
                _ => null,
            };
            if (patientStateScript != null)
                patientStateScript.character = this;  // Set the reference that state scripts will use
        }
    }

    private void InitialiseDemonStats()
    {
        Debug.Log(demon.demonName + " stats initialised");
        // INFO: Initialise further demon stats here?
    }
}
