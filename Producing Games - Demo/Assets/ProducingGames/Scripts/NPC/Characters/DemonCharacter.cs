using Steamworks;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static PatientCharacter;

/// <summary>
/// Written by: Matej Cincibus
/// Moderated by: ...
/// 
/// Handles all the functionality specific to demon NPCs
/// </summary>
/// 

public interface IHear
{
    void ReactToSound(Transform pos);
}

public class DemonCharacter : AICharacter, IHear
{
    public enum DemonStates
    {
        None,

        Patrol,
        Chase,
        Attack,
        Exorcised,
        Inactive,
        Distracted
    }

    [Header("Demon Settings:")]
    public DemonStates currentState;

    [Space(10)]

    [Min(0)] public float attackRadius = 2.0f;
    [Min(0)] public float distanceFromDestination = 3.0f;

    [Space(10)]

    [Header("Timer Durations:")]
    [Tooltip("The length of time the demon remains idling once they've arrived at their patrol location")]
    [Min(0)] public float patrolIdleDuration = 3.0f;
    [Tooltip("The length of time the demon remains in the chase state after having lost visibility of the player")]
    [Min(0)] public float chaseAloneDuration = 5.0f;
    [Tooltip("The length of time the demon camps the hiding spot that the player is currently at")]
    [Min(0)] public float campingDuration = 3.0f;

    [Header("Components:")]
    public DemonStateBaseClass demonStateScript;
    public Transform soundDestination = null;

    [Space(10)]

    [Header("Sound Effects:")]
    public SoundEffect ghostRoaming;
    public SoundEffect impStage1Grunt;
    public SoundEffect impRoaming;
    public SoundEffect impWalking;

    protected Callback<UserAchievementStored_t> m_UserAchievementStored;

    public DemonStates PreviousState { get; private set; }

    public PlayerMovement playerMovement;

    private bool inRageMode = false;

    public bool IsInRageMode() => inRageMode;

    public void SetInRageMode(bool inRageMode) { this.inRageMode = inRageMode; }

    public override void Start()
    {
        base.Start();

        characterType = CharacterTypes.Demon;

        // INFO: Get Local Reference to Player
        playerMovement = GameManager.Instance.player.GetComponent<PlayerMovement>();

        this.ChangeDemonState(DemonStates.Patrol);
    }

    private void Update()
    {
        // INFO: Calls the virtual function for whatever state scripts
        if (demonStateScript != null)
            demonStateScript.UpdateLogic();

        // INFO: Will go into the chase state whenever it sees the player, so long as its not already
        // attacking the player or is not exorcised or the player isn't currently hiding
        if (raycastToPlayer.LookForPlayer() && 
            currentState != DemonStates.Attack && 
            currentState != DemonStates.Exorcised &&
            !playerMovement.isHiding)
            ChangeDemonState(DemonStates.Chase);
    }

    /// <summary>
    /// Will destroy the old state script and create a new one
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeDemonState(DemonStates newState)
    {
        // INFO: Remove all animations
        //ResetAnimation();

        if (currentState != newState || demonStateScript == null)
        {
            // INFO: If the demon was previously inactive, it will be activated again
            // ready for its new state
            if (currentState == DemonStates.Inactive) gameObject.SetActive(true);

            // INFO: If the demon has a path set from a previous state, this will get rid of it
            if (agent != null && agent.hasPath)
                agent.ResetPath();

            // INFO: Destroy current script attached to demon character
            if (demonStateScript != null)
                Destroy(demonStateScript);

            // INFO: Set the previous state of the patient to the current state
            PreviousState = currentState;

            // INFO: Set the current state of the demon to the new state
            currentState = newState;

            demonStateScript = newState switch
            {
                DemonStates.Patrol => transform.AddComponent<PatrolState>(),
                DemonStates.Chase => transform.AddComponent<ChaseState>(),
                DemonStates.Attack => transform.AddComponent<AttackState>(),
                DemonStates.Exorcised => transform.AddComponent<ExorcisedState>(),
                DemonStates.Inactive => transform.AddComponent<InactiveState>(),
                DemonStates.Distracted => transform.AddComponent<DistractedState>(),
                DemonStates.None => null,
                _ => null,
            };

            // INFO: Set the reference that state scripts will use
            if (demonStateScript != null)
                demonStateScript.character = this;
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("HolyWater"))
        {
                SteamAPI.Init();

            // INFO: Steam achievement for banishing demon
            if (!SteamManager.Initialized)
            {
                Debug.LogWarning("Steam Manager doesn't exist!");
                
                //return;

            }
            //else
            //{
                //SteamUserStats.GetAchievement("ACH_WIN_100_GAMES", out bool completed);

                //if (!completed)
                //{
                    m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

                    SteamUserStats.SetAchievement("ACH_WIN_100_GAMES");
                    SteamUserStats.StoreStats();
                //}

            //}

            Destroy(collision.gameObject);
            ChangeDemonState(DemonStates.Exorcised);
        }
    }

    private void ResetAnimation()
    {
        animator.SetBool("isConfused", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isChasing", false);
    }

    /// <summary>
    /// Causes the demon to become distracted by the sound that has occured
    /// and will make it walk over to it
    /// </summary>
    /// <param name="pos"></param>
    public void ReactToSound(Transform pos)
    {
        soundDestination = pos;
        ChangeDemonState(DemonStates.Distracted);
    }

    void OnAchievementStored(UserAchievementStored_t pCallback)
    {

    }

    /// <summary>
    /// Used to check the distance from the player and whether the player is hiding to either
    /// attack the player 
    /// </summary>
    public void CheckBeforeAttack()
    {
        // INFO: Final check before attacking player so the demon doesn't
        // get stuck in attacking state
        if (playerMovement.isHiding)
        {
            animator.SetBool("isAttacking", false);
            ChangeDemonState(DemonStates.Patrol);
            return;
        }

        // INFO: If the player is outside of the demons attack radius when the demon slashes at them then
        // the player won't be killed and the demon will go into the chase state.
        if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) > attackRadius)
        {
            ChangeDemonState(DemonStates.Chase);
        }
        else
        {
            GameManager.Instance.DemonCaptureEvent();
        }

        animator.SetBool("isAttacking", false);
    }
}
