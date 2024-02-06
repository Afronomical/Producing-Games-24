using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// The wandering state allows the NPC to wander around the map by choosing a destination
/// location from a list of potential locations that the NPC can travel to
/// </summary>
public class WanderingState : StateBaseClass
{
    private Vector3 wanderDestination;
    private float distanceFromDestination = 1.1f;

    // INFO: Timer variables used to define the duration that an NPC waits at a location
    private float currentIdleTime = 0.0f;
    private float maxIdleTime = 3.0f;

    private float wanderingSpeed = 1.0f;

    private void Start()
    {
        wanderDestination = NPCWandererManager.Instance.RandomDestination();
        character.agent.speed = wanderingSpeed;
    }

    public override void UpdateLogic()
    {
        // INFO: Enables the NPCs movement capabilities
        GetComponent<AICharacter>().isMoving = true;

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
                ChooseNewDestination();
            }
        }
    }

    private void ChooseNewDestination()
    {
        // INFO: Sets the new destination to the current destination
        Vector3 newWanderDestination = wanderDestination;

        // INFO: Whilst the new chosen destination is equal to the current destination
        // the new destination will continue to be randomized to find a different
        // destination
        while (newWanderDestination == wanderDestination)
        {
            // INFO: Prevents infinite while loop if list contains 1 or less locations
            if (NPCWandererManager.Instance.GetDestinationLocationsCount() <= 1) break;

            newWanderDestination = NPCWandererManager.Instance.RandomDestination();
        }

        wanderDestination = newWanderDestination;
    }
}
