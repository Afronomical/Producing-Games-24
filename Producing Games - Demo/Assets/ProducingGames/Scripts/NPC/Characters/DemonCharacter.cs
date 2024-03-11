using Steamworks;
using Unity.VisualScripting;
using UnityEngine;

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

    [Header("Demon Settings")]
    public DemonStates currentState;
    public float attackRadius = 2.0f;


    [Header("Components")]
    public DemonStateBaseClass demonStateScript;
    public Transform soundDestination;

    [Header("Sound Effects")]
    public SoundEffect ghostRoaming;
    public SoundEffect impStage1Grunt;
    public SoundEffect impRoaming;
    public SoundEffect impWalking;

    protected Callback<UserAchievementStored_t> m_UserAchievementStored;

    bool exorcised = false;

    public override void Start()
    {
        base.Start();

        characterType = CharacterTypes.Demon;
    }

    private void Update()
    { 


        if (demonStateScript != null)
            demonStateScript.UpdateLogic();  // Calls the virtual function for whatever state scripts

        // INFO: Will go into the chase state whenever it sees the player, so long as its not already
        // attacking the player or is not exorcised
        if (raycastToPlayer.PlayerDetected() && currentState != DemonStates.Attack && currentState != DemonStates.Exorcised)
            ChangeDemonState(DemonStates.Chase);
    }

    public void ChangeDemonState(DemonStates newState)  // Will destroy the old state script and create a new one
    {

        if (currentState != newState || demonStateScript == null)
        {
            // INFO: If the demon was previously inactive, it will be activated again
            // ready for its new state
            if (currentState == DemonStates.Inactive) gameObject.SetActive(true);

            if (demonStateScript != null)
                Destroy(demonStateScript); // destroy current script attached to AI character

            //set the current state of AI character to the new state
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


            if (demonStateScript != null)
                demonStateScript.character = this;  // Set the reference that state scripts will use
        }
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("HolyWater"))
        {
                SteamAPI.Init();

            //steam achievement for banishing demon
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

    public void ReactToSound(Transform pos)
    {
        soundDestination = pos;
        ChangeDemonState(DemonStates.Distracted);
    }

    void OnAchievementStored(UserAchievementStored_t pCallback)
    {

    }
}
