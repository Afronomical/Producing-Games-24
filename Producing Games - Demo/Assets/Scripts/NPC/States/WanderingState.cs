using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// </summary>
public class WanderingState : StateBaseClass
{
    private float distanceFromDestination = 1.1f;
    private Vector3 wanderDestination;
    private bool hasReachedDestination = false;

    //private float idleTime = 3.0f;

    private float currentIdleTime = 0.0f;
    private float maxIdleTime = 3.0f;

    private float wanderingSpeed = 1.0f;

    private RaycastToPlayer raycastToPlayer;

    private void Start()
    {
        wanderDestination = NPCWandererManager.Instance.RandomDestination();
        raycastToPlayer = GetComponent<RaycastToPlayer>();
        character.agent.speed = wanderingSpeed;
    }

    public override void UpdateLogic()
    {
        GetComponent<AICharacter>().isMoving = true;

        character.agent.SetDestination(wanderDestination);

        Debug.Log(Vector3.Distance(character.transform.position, wanderDestination));

        if (Vector3.Distance(character.transform.position, wanderDestination) < distanceFromDestination)
        {
            currentIdleTime += Time.deltaTime;

            if (currentIdleTime > maxIdleTime)
            {
                currentIdleTime = 0.0f;
                ChooseNewDestination();
            }

            //Invoke(nameof(ChooseNewDestination), idleTime);
        }

        if (raycastToPlayer.PlayerDetected())
        {
            character.agent.isStopped = true;
            //character.ChangeState(AICharacter.States.Escorted);
        }
    }

    private void ChooseNewDestination()
    {
        Vector3 newWanderDestination = wanderDestination;

        while (newWanderDestination == wanderDestination)
        {
            // INFO: Prevents infinite while loop if list contains 1 or less locations
            if (NPCWandererManager.Instance.GetDestinationLocationsCount() <= 1)
            {
                break;
            }

            newWanderDestination = NPCWandererManager.Instance.RandomDestination();
        }

        wanderDestination = newWanderDestination;
    }
}
