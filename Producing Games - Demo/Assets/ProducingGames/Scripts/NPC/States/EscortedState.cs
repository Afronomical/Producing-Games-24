using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Written By: Matt Brake</para>
/// Moderated By: Matej Cincibus
/// 
///<para>Tracks the behaviour of AI when being escorted back to their room.</para>
///<para>Currently will just move towards player transform, until pathfinding is properly in place.</para>
/// </summary>

public class EscortedState : StateBaseClass
{
    private Vector3 targetPos;
    private RaycastToPlayer raycastToPlayer;
    private bool shouldFollow = true;
    private bool hasPickedUpNPC = false;
    private bool bedInRange;
    private float timeAlone = 0;
    private readonly float maxTimeAlone = 5f;
    private readonly float stoppingDistance = 3.0f;

    private Vector3 lastPlayerPos;

    private void Awake()
    {
        GetComponent<AICharacter>().isMoving = true;

        if (raycastToPlayer == null)
            raycastToPlayer = GetComponent<RaycastToPlayer>();
    }

    private void Start()
    {
        character.agent.ResetPath();
        character.agent.speed = character.runSpeed;
    }

    public override void UpdateLogic()
    {
        targetPos = character.player.transform.position;

        if(character.deterred)
        {
            character.ChangeState(AICharacter.States.Abandoned);
            character.deterred = false;
        }

        if (raycastToPlayer.PlayerDetected()) //player is detected. following player function is called. 
        {
            if (timeAlone != 0)
                timeAlone = 0.0f;

            hasPickedUpNPC = true;

            lastPlayerPos = character.player.transform.position;

            MoveTowardsPlayer();
        }
        else if (hasPickedUpNPC && !raycastToPlayer.PlayerDetected()) //if NPC previously picked up but currently not detecting player 
        {
            character.agent.SetDestination(lastPlayerPos); //NPC moves to player last known position to check whether they are still in range 
            timeAlone += Time.deltaTime;

            if (timeAlone >= maxTimeAlone) //gives player 3 seconds to recollect NPC before they enter wandering state again 
            {
                character.ChangeState(AICharacter.States.Abandoned); //changes state to abandoned
                timeAlone = 0.0f;
            }
            return;
        }

        if (hasPickedUpNPC)
        {
            if (CheckBedInRange()) //checking whether bed is in range 
            {
                bedInRange = true;
                shouldFollow = false;
                character.agent.enabled = false;
                character.ChangeState(AICharacter.States.Bed);
                //move to bed instead of player, and then enter bed state 
            }
        }
    }

    bool CheckBedInRange() /*only perform if player has picked NPC up */ //function to check whether bed in range 
    {
        Collider[] colliders = Physics.OverlapSphere(character.transform.position, character.detectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.name == "Bed") ///just for testing 
            {
                Vector3 bedPos = new Vector3(collider.gameObject.transform.position.x, collider.gameObject.transform.position.y + 1.0f, collider.gameObject.transform.position.z);
                
                transform.position = bedPos;
                return true;

            }
            else
            {
                //Debug.Log("BED NOT FOUND"); 
            }
        }

        return false;
    }


    /// <summary>
    /// <para>Primary Function in escorting state.</para>
    /// <para>Checks if player is within range and follows them if true.</para>
    /// </summary>
    void MoveTowardsPlayer()
    {
        // INFO: Ensures the NPC only rotates on the y-axis
        Vector3 playerPosition = new(character.player.transform.position.x, transform.position.y, character.player.transform.position.z);
        transform.LookAt(playerPosition);

        if (character.rb != null && shouldFollow == true)
            character.agent.SetDestination(targetPos); // sets target position to player last pos 

        if (raycastToPlayer.playerDistance < stoppingDistance)  //stops NPC moving any closer than 3 units 
        {
            shouldFollow = false;

            character.rb.velocity = Vector3.zero;
            character.agent.isStopped = true;
            character.agent.ResetPath();
        }
        else
        {
            shouldFollow = true;

            character.agent.isStopped = false;
        }
    }
}
