using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Written by: Matej Cincibus
/// Moderated by: ...
/// 
/// Handles all the functionality specific to demon NPCs
/// </summary>

public class DemonCharacter : AICharacter
{
    public enum DemonStates
    {
        None,

        Patrol,
        Chase,
        Attack,
        Exorcised,
        Inactive
    }

    public DemonStates currentState;
    public DemonStateBaseClass demonStateScript;

    [Header("Demon Settings")]
    public float attackRadius = 2.0f;
    

    public override void Start()
    {
        base.Start();

        characterType = CharacterTypes.Demon;
       

        ChangeDemonState(DemonStates.Patrol); //INFO: Starting State
    }

    private void Update()
    {
        if (demonStateScript != null)
            demonStateScript.UpdateLogic();  // Calls the virtual function for whatever state scripts

        // INFO: Will go into the chase state whenever it sees the player, so long as its not already
        // attacking the player
        if (raycastToPlayer.PlayerDetected() && currentState != DemonStates.Attack)
        {
            ChangeDemonState(DemonStates.Chase);
        }
    }

    public void ChangeDemonState(DemonStates newState)  // Will destroy the old state script and create a new one
    {

        if (currentState != newState || demonStateScript == null)
        {
            if (demonStateScript != null)
            {
                //destroy current script attached to AI character
                Destroy(demonStateScript);
            }

            //set the current state of AI character to the new state
            currentState = newState;

            switch (newState)
            {
                case DemonStates.Patrol:
                    demonStateScript = transform.AddComponent<PatrolState>();
                    break;
                case DemonStates.Chase:
                    demonStateScript = transform.AddComponent<ChaseState>();
                    break;
                case DemonStates.Attack:
                    demonStateScript = transform.AddComponent<AttackState>();
                    break;
                case DemonStates.Exorcised:
                    demonStateScript = transform.AddComponent<ExorcisedState>();
                    break;
                case DemonStates.Inactive:
                    demonStateScript = transform.AddComponent<InactiveState>();
                    break;
                case DemonStates.None:
                    demonStateScript = null;
                    break;
                default:
                    demonStateScript = null;
                    break;
            }

            if (demonStateScript != null)
                demonStateScript.character = this;  // Set the reference that state scripts will use
        }
    }
}
