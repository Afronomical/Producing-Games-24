using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

/// <summary>
/// Written by: Matej Cincibus
/// Moderated by: ...
/// 
/// Handles all the functionality specific to patient NPCs
/// </summary>

public enum SafetyChoices
{
    HidingSpot,
    Bedroom
}

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
        Panic,
        Scared
    }

    [Header("Patient Settings:")]
    public PatientStates currentState;
    public SafetyChoices safetyChoice;
    public bool isPossessed = false;

    [Space(10)]

    public int startingHealth = 100;
    public int currentHealth;

    [Space(10)]

    public float startingSanity = 100;
    public float currentSanity;

    [Space(10)]

    [Min(0)] public float cowerRadius = 2.0f;
    [Min(0)] public float distanceFromDestination = 3.0f;
    [Tooltip("The distance at which the patient stops from the player when being escorted")]
    [Min(0)] public float distanceFromPlayer = 3.0f;

    [Space(10)]

    [Header("Timer Durations:")]
    [Min(0)] public float calmingDuration = 5.0f;
    [Min(0)] public float abandonedDuration = 5.0f;
    [Tooltip("The length of time the patient remains in the escorted state after having lost visibility of the player")]
    [Min(0)] public float aloneEscortedDuration = 5.0f;
    [Tooltip("The length of time the patient remains idling once they've arrived at their wandering location")]
    [Min(0)] public float wanderingIdleDuration = 3.0f;

    [Space(10)]

    [Header("Tasks")]
    [HideInInspector] public bool hungry = false;
    [HideInInspector] public bool hasBeenHiding = false;
    [HideInInspector] public bool hasBeenHungry = false;
    [HideInInspector] public bool hasBeenGreedy = false;

    [Header("Components")]
    public PatientStateBaseClass patientStateScript;
    public GameObject bed;
    public SoundEffect scaredNPC; 

    public float DistanceFromDemon { get; private set; }
    public Transform BedDestination { get; private set; }
    public PatientStates PreviousState { get; private set; }

    private GameObject demonGO;
    private DemonCharacter demonCharacter;


    private void Awake()
    {
        player = FindFirstObjectByType<PlayerMovement>().gameObject;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        raycastToPlayer = GetComponent<RaycastToPlayer>();
        animator = GetComponent<Animator>();
    }

    public override void Start()
    {
        base.Start();

        characterType = CharacterTypes.Patient;

        currentHealth = startingHealth;
        currentSanity = startingSanity;

        BedDestination = bed.transform.Find("PatientPosition");

        if (isPossessed)
        {
            // INFO: Instantiates the demon and saves it to the game manager so it can be used elsewhere
            GameObject GO = Instantiate(NPCManager.Instance.ChosenDemon.demonPrefab,
                                        NPCManager.Instance.GetDemonInstantionLocation().transform.position,
                                        Quaternion.identity);

            GameManager.Instance.demon = GO;
        }

        // INFO: Starting State
        ChangePatientState(PatientStates.Abandoned);
    }

    private void Update()
    {
        // INFO:  Calls the virtual function for whatever state scripts
        if (patientStateScript != null)
            patientStateScript.UpdateLogic();

        // INFO: Assign a reference to the demon for each patient
        if (GameManager.Instance.demon != null && demonGO == null)
        {
            demonGO = GameManager.Instance.demon;
            demonCharacter = demonGO.GetComponent<DemonCharacter>();
        }

        // INFO: Monitors health to check whether patient has died
        if (currentHealth <= 0 && currentState != PatientStates.Dead)
            ChangePatientState(PatientStates.Dead);

        // INFO: Causes the patient to go into panic/scared if a horror
        // event occurs near them
        LocateNearestHorrorEvent();

        // INFO: Causes the patient to go into panic/scared if the demon
        // is near them
        LocateDemon();
    }

    /// <summary>
    /// Will destroy the old state script and create a new one
    /// </summary>
    /// <param name="newState"></param>
    public void ChangePatientState(PatientStates newState)
    {
        // INFO: Remove all animations
        animator.SetBool("isHungry", false);
        animator.SetBool("isPraying", false);
        animator.SetBool("reqMeds", false);
        animator.SetBool("inBed", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isTerrified", false);
        animator.SetBool("isScared", false);

        if (currentState != newState || patientStateScript == null)
        {
            // INFO: If the previous state had the patient remain stationary, we will need to grant the patient
            // movement again for the new state that they're going to go into
            if (currentState == PatientStates.Bed || currentState == PatientStates.ReqMeds || currentState == PatientStates.Prayer)
            {
                rb.useGravity = true;
                agent.enabled = true;
            }

            // INFO: If the patient has a path set from a previous state, this will get rid of it
            if (agent.hasPath)
                agent.ResetPath();

            if (patientStateScript != null)
                Destroy(patientStateScript); // destroy current script attached to AI character

           

            // INFO: Set the previous state of the patient to the current state
            PreviousState = currentState;

            // INFO: Set the current state of the patient to the new state
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
                PatientStates.Scared => transform.AddComponent<ScaredState>(),
                PatientStates.None => null,
                _ => null,
            };

            // INFO: Set the reference that state scripts will use
            if (patientStateScript != null)
                patientStateScript.character = this;
        }
    }

    /// <summary>
    /// Detects horror events near the patient
    /// </summary>
    private void LocateNearestHorrorEvent()
    {
        // INFO: Given that horror events aren't happening/can't happen we exit
        if (HorrorEventManager.Instance == null || HorrorEventManager.Instance.Events.Count < 1)
            return;

        // INFO: Goes through all horror events currently happening
        foreach (GameObject item in HorrorEventManager.Instance.Events)
        {
            float distanceFromHorrorEvent = (item.transform.position - transform.position).sqrMagnitude;

            // INFO: Given that the event is too far from the patient we continue
            if (distanceFromHorrorEvent > detectionRadius)
                continue;

            PanicOrScaredDecider();
        }
    }

    /// <summary>
    /// Detects how close the demon is near the patient
    /// </summary>
    private void LocateDemon()
    {
        // INFO: If there is no demon or the demon is inactive/has been exorcised
        // we exit
        if (demonGO == null || demonCharacter.currentState == DemonCharacter.DemonStates.Inactive
                            || demonCharacter.currentState == DemonCharacter.DemonStates.Exorcised) 
            return;

        // INFO: Logic for detecting how far away the demon is from the patient and what state to enter
        DistanceFromDemon = (transform.position - demonGO.transform.position).sqrMagnitude;

        // INFO: Given that the demon is too far from the patient we return
        if (DistanceFromDemon > detectionRadius)
            return;

        PanicOrScaredDecider();
    }

    /// <summary>
    /// Decides between the panic or scared patient states
    /// based on the in-game hour
    /// </summary>
    private void PanicOrScaredDecider()
    {
        // INFO: If the patient is already panicking/scared or they are dead
        // we can return
        if (currentState == PatientStates.Panic ||
            currentState == PatientStates.Scared ||
            currentState == PatientStates.Dead)
            return;

        ChangePatientState(GameManager.Instance.currentHour < 7 ? PatientStates.Scared : PatientStates.Panic);
    }

    /// <summary>
    /// The patient chooses to either hide or run back to their bedroom when
    /// they enter the panicked/scared state
    /// </summary>
    public Vector3 SafetyChooser()
    {
        switch (safetyChoice)
        {
            case SafetyChoices.HidingSpot:
                return NPCManager.Instance.RandomHidingLocation();
            case SafetyChoices.Bedroom:
                return bed.transform.position;
            default:
                break;
        }

        Debug.LogError("Unable to choose a safety location for the patient!");
        return Vector3.zero;
    }

    /// <summary>
    /// Function that handles the detection of the bed belonging to a specific patient
    /// </summary>
    /// <returns></returns>
    public bool CheckBedInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == bed)
                return true;
        }
        return false;
    }
}
