using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EscortNPCTest))]


public class AICharacter : MonoBehaviour
{

    public enum CharacterTypes
    {
        Patient,
        Demon
    }

    public enum States
    {
        Escorted,
        Wandering,
        Abandoned,
        Possessed,
        Dead,
        Bed,
        Praying,
        Hiding,
        Hungry,
        ReqMeds,
        Exorcised,//??

        None
    }

    [Header("Character Stats")]
    public CharacterTypes characterType;
    public int startingHealth = 100;
    public int currentHealth;
    public float startingSanity = 100;
    public float currentSanity;
    public float walkSpeed = 1.0f;
    public float runSpeed = 3.0f;
    public float crawlSpeed = 0.5f;
    public float detectionRadius = 5.0f;
    public bool isPossessed = false; 

    [Header("States")]
    public States currentState;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool knowsAboutPlayer;

    [Header("Components")]
    public StateBaseClass stateScript;
    public GameObject player;
    public Rigidbody rb;
    public NavMeshAgent agent;
   [SerializeField] private DemonItemsSO Demon;
   [ShowOnly] public GameObject bed; 

   

    private void Start()
    {
        currentHealth = startingHealth;
        currentSanity = startingSanity;

        player = FindFirstObjectByType<PlayerMovement>().gameObject;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        //ChangeState(States.Abandoned); //INFO: Starting State
        ChangeState(States.Wandering);

        if(isPossessed)
        {
            /// add demon state 
            Demon = NPCManager.Instance.ChosenDemon;
            ChangeCharacterType(CharacterTypes.Demon);
            
            InitialiseDemonStats();
        }
        
       
    }


    private void Update()
    {
        if (stateScript != null)
            stateScript.UpdateLogic();  // Calls the virtual function for whatever state scripts

        if (currentHealth <= 0)
            ChangeState(States.Dead);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void ChangeState(States newState)  // Will destroy the old state script and create a new one
    {

        if (currentState != newState || stateScript == null)
        {
            if (stateScript != null)
            {
                //destroy current script attached to AI character
                Destroy(stateScript);
            }

            //set the current state of AI character to the new state
            currentState = newState;

            switch (newState)
            {
                case States.Exorcised:
                    stateScript = transform.AddComponent<ExorcisedState>();
                    break;
                case States.Possessed:
                    stateScript = transform.AddComponent<PosessedState>();
                    break;
                case States.Escorted:
                    stateScript = transform.AddComponent<EscortedState>();
                    break;
                case States.Abandoned:
                    stateScript = transform.AddComponent<AbandonedState>();
                    break;
                case States.Bed:
                    stateScript = transform.AddComponent<BedState>();
                    break;
                case States.Wandering:
                    stateScript = transform.AddComponent<WanderingState>();
                    break;
                case States.Dead:
                    stateScript = transform.AddComponent<DeadState>();
                    break;
                case States.Praying:
                    break;
                case States.Hiding:
                    stateScript = transform.AddComponent<HidingState>();
                    break;
                case States.Hungry:
                    break;
                case States.ReqMeds:
                    stateScript = transform.AddComponent<RequestMedicationState>();
                    break;
                case States.None:
                    stateScript = null;
                    break;
                default:
                    stateScript = null;
                    break;
            }

            if (stateScript != null)
                stateScript.character = this;  // Set the reference that state scripts will use
        }
    }
    public void ChangeCharacterType(CharacterTypes type)
    {
        characterType = type; 
    }

    private void InitialiseDemonStats()
    {
        if(isPossessed)
        {
            ////add initialisation here 
            Debug.Log(Demon.DemonName + " stats initialised");
            
           
        }
    }
    
}
