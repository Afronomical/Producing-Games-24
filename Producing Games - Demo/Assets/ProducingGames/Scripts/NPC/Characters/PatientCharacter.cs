using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// Written by: Matej Cincibus
/// Moderated by: ...
/// 
/// Handles all the functionality specific to patient NPCs
/// </summary>

[RequireComponent(typeof(EscortNPCTest))]

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

    public PatientStates currentState;
    public PatientStateBaseClass patientStateScript;

    public int startingHealth = 100;
    public int currentHealth;
    public float startingSanity = 100;
    public float currentSanity;
    public bool isPossessed = false;

    public DemonItemsSO demon;
    public GameObject bed;

    public override void Start()
    {
        base.Start();

        characterType = CharacterTypes.Patient;

        currentHealth = startingHealth;
        currentSanity = startingSanity;

        if (isPossessed)                                
        {                                               
            // INFO: Possessed state will need to be changed elsewhere
            // INFO: Add demon state                         
            demon = NPCManager.Instance.ChosenDemon;
            InitialiseDemonStats();

            GameObject GO = Instantiate(demon.demonPrefab, bed.transform.position, Quaternion.identity);
            GameManager.Instance.demon = GO;
            GO.GetComponent<DemonCharacter>().ChangeDemonState(DemonCharacter.DemonStates.Inactive);
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
            if (currentState == PatientStates.Bed || currentState == PatientStates.ReqMeds) agent.enabled = true;

            if (patientStateScript != null)
            {
                //destroy current script attached to AI character
                Destroy(patientStateScript);
            }

            //set the current state of AI character to the new state
            currentState = newState;

            switch (newState)
            {
                case PatientStates.Possessed:
                    patientStateScript = transform.AddComponent<PossessedState>();
                    break;
                case PatientStates.Escorted:
                    patientStateScript = transform.AddComponent<EscortedState>();
                    break;
                case PatientStates.Abandoned:
                    patientStateScript = transform.AddComponent<AbandonedState>();
                    break;
                case PatientStates.Bed:
                    patientStateScript = transform.AddComponent<BedState>();
                    break;
                case PatientStates.Wandering:
                    patientStateScript = transform.AddComponent<WanderingState>();
                    break;
                case PatientStates.Dead:
                    patientStateScript = transform.AddComponent<DeadState>();
                    break;
                case PatientStates.Praying:
                    patientStateScript = transform.AddComponent<PrayerState>();
                    break;
                case PatientStates.Hiding:
                    patientStateScript = transform.AddComponent<HidingState>();
                    break;
                case PatientStates.Hungry:
                    patientStateScript = transform.AddComponent<HungryState>();
                    break;
                case PatientStates.ReqMeds:
                    patientStateScript = transform.AddComponent<RequestMedicationState>();
                    break;
                case PatientStates.None:
                    patientStateScript = null;
                    break;
                default:
                    patientStateScript = null;
                    break;
            }

            if (patientStateScript != null)
                patientStateScript.character = this;  // Set the reference that state scripts will use
        }
    }

    private void InitialiseDemonStats()                         
    {                                                           
        if (isPossessed)                                        
        {                                                       
            // add initialisation here                         
            Debug.Log(demon.demonName + " stats initialised");  
        }                                                       
    }
}
