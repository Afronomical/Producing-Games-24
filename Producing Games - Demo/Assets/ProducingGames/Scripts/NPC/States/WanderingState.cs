using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: Matt Brake 
/// 
/// The wandering state allows the NPC to wander around the map by choosing a destination
/// location from a list of potential locations that the NPC can travel to
/// </summary>

public class WanderingState : StateBaseClass
{
    private Vector3 wanderDestination;
    private readonly float distanceFromDestination = 1.1f;

    // INFO: Timer variables used to define the duration that an NPC waits at a location
    private float currentIdleTime = 0.0f;
    private readonly float maxIdleTime = 3.0f;

    private void Awake()
    {
        // INFO: Enables the NPCs movement capabilities
        GetComponent<AICharacter>().isMoving = true;

    }

    private void Start()
    {
        ChooseDestination();

        character.agent.speed = character.walkSpeed;  
        character.agent.ResetPath();
    }

    public override void UpdateLogic()
    {
        character.agent.SetDestination(wanderDestination);

        // INFO: Given that the NPC is near to the destination location a timer is started
        if (Vector3.Distance(character.transform.position, wanderDestination) < distanceFromDestination)
        {
            currentIdleTime += Time.deltaTime;

            // INFO: After the NPC has waited at its destination location for a specified
            // time it will then choose a different location to move towards
            if (currentIdleTime > maxIdleTime)
            {
                currentIdleTime = 0.0f;
                ChooseDestination();
            }
        }
    }

    private void ChooseDestination()
    {
        // INFO: If there are no wandering destinations in the list then end
        if (NPCManager.Instance.GetWanderingDestinationsCount() == 0)
        {
            Debug.LogWarning("There are no wandering destinations setup in the wandering destinations list.");
            return;
        }

        // INFO: Sets the wandering destination that it has arrived at as free,
        // so that other NPCs can walk to it
        NPCManager.Instance.SetWanderingDestinationFree(wanderDestination);

        // INFO: Chooses a new destination to wander to
        wanderDestination = NPCManager.Instance.RandomWanderingDestination();
    }
}
