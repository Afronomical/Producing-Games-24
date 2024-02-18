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
        GetComponent<Animator>().SetBool("isMoving", true);
        GetComponent<Animator>().SetBool("isChasing", false);
    }

    public override void UpdateLogic()
    {
        if (character.agent.velocity.magnitude > 0)
        {
            GetComponent<Animator>().SetBool("isMoving", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("isMoving", false);
        }

        character.agent.SetDestination(patrolDestination);

        // INFO: Given that the NPC is near to the destination location a timer is started
        if (Vector3.Distance(character.transform.position, patrolDestination) < distanceFromDestination)
        {
            currentIdleTime += Time.deltaTime;

            character.isMoving = false;
            // INFO: After the NPC has waited at its destination location for a specified
            // time it will then choose a different location to move towards
            if (currentIdleTime > maxIdleTime)
            {
                currentIdleTime = 0.0f;
                ChooseDestination();
            }
        }
        else
        {
            character.isMoving = true;
        }

    }

    /// <summary>
    /// Chooses a destination from an available list of destination locations held in the NPC manager 
    /// </summary>
    private void ChooseDestination()
    {
        // INFO: If there are no wandering destinations in the list then end
        if (NPCManager.Instance.GetPatrolDestinationsCount() == 0)
        {
            Debug.LogWarning("There are no patrol destinations setup in the patrol destinations list.");
            return;
        }

        // INFO: Chooses a new destination to wander to
        patrolDestination = NPCManager.Instance.RandomPatrolDestination();
    }
}
