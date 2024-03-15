using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// The demon walks to a specific location it chooses from a list of available locations
/// </summary>

public class PatrolState : DemonStateBaseClass
{
    private Vector3 patrolDestination;
    private float patrolIdleTime = 0.0f;

    private void Start()
    {
        character.agent.speed = character.walkSpeed;

        ChoosePatrolDestination();
    }

    public override void UpdateLogic()
    {
        character.animator.SetFloat("movement", character.agent.velocity.magnitude);

        // INFO: Given that the demon is near to the destination location a timer is started
        if ((character.transform.position - patrolDestination).sqrMagnitude < character.distanceFromDestination)
        {
            patrolIdleTime += Time.deltaTime;

            // INFO: After the demon has waited at its destination location for a specified
            // time it will then choose a different location to move towards
            if (patrolIdleTime > character.patrolIdleDuration)
            {
                patrolIdleTime = 0.0f;
                ChoosePatrolDestination();
            }
        }
    }

    /// <summary>
    /// Chooses a destination from an available list of destination locations held in the NPC manager 
    /// </summary>
    private void ChoosePatrolDestination()
    {
        // INFO: If there are no patrol destinations in the list then end
        if (NPCManager.Instance.GetPatrolDestinationsCount() == 0)
        {
            Debug.LogWarning("There are no patrol destinations setup in the patrol destinations list.");
            return;
        }

        // INFO: Chooses a new destination to patrol to
        patrolDestination = NPCManager.Instance.RandomPatrolDestination();

        character.agent.SetDestination(patrolDestination);
    }
}
