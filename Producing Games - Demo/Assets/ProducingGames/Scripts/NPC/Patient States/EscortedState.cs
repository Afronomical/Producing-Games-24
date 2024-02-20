using UnityEngine;

/// <summary>
/// <para>Written By: Matt Brake</para>
/// Moderated By: Matej Cincibus
/// 
///<para>Tracks the behaviour of AI when being escorted back to their room.</para>
///<para>Currently will just move towards player transform, until pathfinding is properly in place.</para>
/// </summary>

public class EscortedState : PatientStateBaseClass
{
    private Vector3 targetPos;
    private Vector3 lastPlayerPos;

    private bool shouldFollow = true;
    private bool hasPickedUpPatient = false;
    private readonly float stoppingDistance = 3.0f;

    private float timeAlone = 0;
    private readonly float maxTimeAlone = 5f;


    private void Start()
    {
        if (character.agent.hasPath)
            character.agent.ResetPath();

        character.agent.speed = character.runSpeed;
    }

    public override void UpdateLogic()
    {
        character.animator.SetBool("isMoving", true);

        targetPos = character.player.transform.position;
        
        if (character.raycastToPlayer.PlayerDetected()) //player is detected. following player function is called. 
        {
            if (timeAlone != 0)
                timeAlone = 0.0f;

            hasPickedUpPatient = true;

            lastPlayerPos = character.player.transform.position;

            MoveTowardsPlayer();
        }
        else if (hasPickedUpPatient && !character.raycastToPlayer.PlayerDetected()) //if patient previously picked up but currently not detecting player 
        {
            character.agent.SetDestination(lastPlayerPos); //Patient moves to player last known position to check whether they are still in range 

            // INFO: Given that the patient has arrived at their destination, start a countdown for the abandoned transition
            if (Vector3.Distance(transform.position, lastPlayerPos) < stoppingDistance)
            {
                timeAlone += Time.deltaTime;
                character.animator.SetBool("isAbandoned", true);

                if (timeAlone >= maxTimeAlone) //gives player 3 seconds to recollect patient before they enter wandering state again 
                {
                    timeAlone = 0.0f;
                    character.ChangePatientState(PatientCharacter.PatientStates.Abandoned); //changes state to abandoned
                }
            }
        }

        if (hasPickedUpPatient)
        {
            if (CheckBedInRange()) //checking whether bed is in range 
            {
                shouldFollow = false;
                character.ChangePatientState(PatientCharacter.PatientStates.Bed);
            }
        }
    }

    bool CheckBedInRange() /*only perform if player has picked NPC up */ //function to check whether bed in range 
    {
        Collider[] colliders = Physics.OverlapSphere(character.transform.position, character.detectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == character.bed)
                return true;
        }
        return false;
    }

    /// <summary>
    /// <para>Primary Function in escorting state.</para>
    /// <para>Checks if player is within range and follows them if true.</para>
    /// </summary>
    void MoveTowardsPlayer()
    {
        // INFO: Ensures the patient only rotates on the y-axis
        Vector3 playerPosition = new(character.player.transform.position.x, transform.position.y, character.player.transform.position.z);
        transform.LookAt(playerPosition);

        if (shouldFollow)
            character.agent.SetDestination(targetPos); // sets target position to player last pos 

        if (character.raycastToPlayer.playerDistance < stoppingDistance)  //stops patient moving any closer than 3 units 
        {
            shouldFollow = false;

            character.rb.velocity = Vector3.zero;
            character.agent.ResetPath();

            character.animator.SetBool("isMoving", false);
        }
        else
            shouldFollow = true;
    }
}
