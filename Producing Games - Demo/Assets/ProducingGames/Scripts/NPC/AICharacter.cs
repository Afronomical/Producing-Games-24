using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(RaycastToPlayer))]

public class AICharacter : MonoBehaviour
{
    public enum CharacterTypes
    {
        Patient,
        Demon
    }

    [Header("Character Settings")]
    public CharacterTypes characterType;
    public float walkSpeed = 1.0f;
    public float runSpeed = 3.0f;
    public float crawlSpeed = 0.5f;
    public float detectionRadius = 5.0f;
    

    [Header("States")]
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool knowsAboutPlayer;

    [Header("Components")]
    public GameObject player;
    public Rigidbody rb;
    public NavMeshAgent agent;
    public RaycastToPlayer raycastToPlayer;

    [Header("Debugging Tools")]
    public Color detectionRadiusColor = Color.white;

    public virtual void Start()
    {
        
        
        // INFO: If no specific color has been chosen, then default
        // values are provided
        if (detectionRadiusColor == Color.white)
        {
            switch (characterType)
            {
                case CharacterTypes.Patient:
                    detectionRadiusColor = Color.green;
                    break;
                case CharacterTypes.Demon:
                    detectionRadiusColor = Color.red;
                    break;
                default:
                    detectionRadiusColor = Color.yellow;
                    break;
            }
        }

        player = FindFirstObjectByType<PlayerMovement>().gameObject;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        raycastToPlayer = GetComponent<RaycastToPlayer>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = detectionRadiusColor;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void ChangeCharacterType(CharacterTypes type)
    {
        characterType = type;
    }
}
