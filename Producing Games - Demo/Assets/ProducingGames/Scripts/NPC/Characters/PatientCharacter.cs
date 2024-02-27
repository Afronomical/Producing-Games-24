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
        Prayer,
        Hiding,
        Hungry,
        ReqMeds,
        Panic
    }

    [Header("Patient Settings")]
    public PatientStates currentState;
    public int startingHealth = 100;
    public int currentHealth;
    public float startingSanity = 100;
    public float currentSanity;
    public bool isPossessed = false;
    [Range(0, 1)] public float cowerRadiusPercentage = 0.25f;
    public float calmingDuration = 5.0f;
    public float distanceFromDestination = 3.0f;

    [Header("Components")]
    public PatientStateBaseClass patientStateScript;
    public DemonItemsSO demonSO;
    public GameObject demon;
    public GameObject bed;

    public float DistanceFromDemon { get; private set; }
    private DemonCharacter demonCharacter;

    public override void Start()
    {
        base.Start();

        characterType = CharacterTypes.Patient;

        currentHealth = startingHealth;
        currentSanity = startingSanity;

        if (isPossessed)                                
        {
            // INFO: Retrieves the scriptable object of the chosen demon                       
            demonSO = NPCManager.Instance.ChosenDemon;
            InitialiseDemonStats();

            // INFO: Instantiates the demon and saves it on the game manager so it can be used elsewhere
            GameObject GO = Instantiate(demonSO.demonPrefab, NPCManager.Instance.GetDemonInstantionLocation().transform.position, Quaternion.identity);
            GameManager.Instance.demon = GO;
        }

        ChangePatientState(PatientStates.Abandoned); //INFO: Starting State
    }

    private void Update()
    {
        // INFO: Assign a reference to the demon for each patient
        if (GameManager.Instance.demon != null && demon == null)
        {
            demon = GameManager.Instance.demon;
            demonCharacter = demon.GetComponent<DemonCharacter>();
        }

        if (patientStateScript != null)
            patientStateScript.UpdateLogic();  // Calls the virtual function for whatever state scripts

        // INFO: Monitors health to check whether patient has died
        if (currentHealth <= 0 && currentState != PatientStates.Dead)
            ChangePatientState(PatientStates.Dead);

        if (demon != null)
        {
            // INFO: Logic for detecting how far away the demon is from the patient and what state to enter
            DistanceFromDemon = Vector3.Distance(transform.position, demon.transform.position);

            // INFO: So long as the demon is active and hasn't been exorcised he can scare
            // patients and cause them to go into the panic state
            if (DistanceFromDemon < detectionRadius && currentState != PatientStates.Panic &&
                (demonCharacter.currentState != DemonCharacter.DemonStates.Inactive ||
                demonCharacter.currentState != DemonCharacter.DemonStates.Exorcised))
                ChangePatientState(PatientStates.Panic);
        }
    }

    public void ChangePatientState(PatientStates newState)  // Will destroy the old state script and create a new one
    {

        if (currentState != newState || patientStateScript == null)
        {
            // INFO: If the previous state had the patient remain stationary, we will need to grant the patient
            // movement again for the new state that they're going to go into
            if (currentState == PatientStates.Bed || currentState == PatientStates.ReqMeds || currentState == PatientStates.Prayer)
            {
                rb.useGravity = true;
                agent.enabled = true;
            }

            if (patientStateScript != null)
                Destroy(patientStateScript); // destroy current script attached to AI character

            //remove all animations
            animator.SetBool("isHungry", false);
            animator.SetBool("isPraying", false);
            animator.SetBool("reqMeds", false);
            animator.SetBool("inBed", false);


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
                PatientStates.Prayer => transform.AddComponent<PrayerState>(),
                PatientStates.Hiding => transform.AddComponent<HidingState>(),
                PatientStates.Hungry => transform.AddComponent<HungryState>(),
                PatientStates.ReqMeds => transform.AddComponent<RequestMedicationState>(),
                PatientStates.Panic => transform.AddComponent<PanicState>(),
                PatientStates.None => null,
                _ => null,
            };

            if (patientStateScript != null)
                patientStateScript.character = this;  // Set the reference that state scripts will use
        }
    }

    private void InitialiseDemonStats()
    {
        //Debug.Log(demonSO.demonName + " stats initialised");
        // INFO: Initialise further demon stats here?
    }
}
