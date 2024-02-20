using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: Matt Brake 
/// 
/// The wandering state allows the NPC to wander around the map by choosing a destination
/// location from a list of potential locations that the NPC can travel to
/// </summary>

public class WanderingState : PatientStateBaseClass
{
    private Vector3 wanderDestination;
    private readonly float distanceFromDestination = 1.1f;

    // INFO: Timer variables used to define the duration that an NPC waits at a location
    private float currentIdleTime = 0.0f;
    private readonly float maxIdleTime = 3.0f;

    private void Start()
    {
        if (character.agent.hasPath)
            character.agent.ResetPath();

        character.agent.speed = character.walkSpeed;  

        ChooseDestination();

        character.animator.SetBool("inBed", false);
        character.animator.SetBool("isPraying", false);
    }

    public override void UpdateLogic()
    {
        character.agent.SetDestination(wanderDestination);

        // INFO: Given that the patient is near to the destination location a timer is started
        if (Vector3.Distance(character.transform.position, wanderDestination) < distanceFromDestination)
        {
            currentIdleTime += Time.deltaTime;

            // INFO: After the patient has waited at its destination location for a specified
            // time it will then choose a different location to move towards
            if (currentIdleTime > maxIdleTime)
            {
                currentIdleTime = 0.0f;
                ChooseDestination();
            }
        }

        if (character.agent.velocity.magnitude > 0)
            character.animator.SetBool("isMoving", true);
        else
            character.animator.SetBool("isMoving", false);
    }

    /// <summary>
    /// Chooses a destination from an available list of destination locations held in the NPC manager 
    /// </summary>
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
