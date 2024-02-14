using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : DemonStateBaseClass
{
    private Vector3 targetPos;
    private Vector3 lastTargetPos;

    private float timeAlone = 0;
    private readonly float maxTimeAlone = 20f;

    private void Awake()
    {
        GetComponent<AICharacter>().isMoving = true;
    }

    private void Start()
    {
        character.agent.ResetPath();
        character.agent.speed = character.runSpeed;
    }

    public override void UpdateLogic()
    {
        targetPos = character.player.transform.position;

        if (character.raycastToPlayer.PlayerDetected()) //player is detected. following player function is called. 
        {
            if (timeAlone != 0)
                timeAlone = 0.0f;

            lastTargetPos = character.player.transform.position;

            MoveTowardsPlayer();
        }
        else
        {
            character.agent.SetDestination(lastTargetPos);
            timeAlone += Time.deltaTime;

            if (timeAlone >= maxTimeAlone)
            {
                timeAlone = 0.0f;
                character.ChangeDemonState(DemonCharacter.DemonStates.Patrol); //changes state to patrol
            }
        }
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

        if (character.rb != null)
            character.agent.SetDestination(targetPos); // sets target position to player last pos 
    }
}
