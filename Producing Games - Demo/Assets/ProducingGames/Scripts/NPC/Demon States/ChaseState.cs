using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// The demon will chase the player so long as the player remains in its
/// radius, if it leaves its radius, the demon will go to the players last
/// known position and after some time go back to patrolling. If it gets
/// close enough to the player it will enter the attack state
/// </summary>

public class ChaseState : DemonStateBaseClass
{
    private Vector3 targetPos;
    private Vector3 lastTargetPos;
    private bool isChasing;

    private float timeAlone = 0;
    private readonly float maxTimeAlone = 20f;

    private void Start()
    {
        if (character.agent.hasPath)
            character.agent.ResetPath();

        character.agent.speed = character.runSpeed;

        isChasing = false;
    }

    public override void UpdateLogic()
    {
        targetPos = character.player.transform.position;

        /*
         * if (character.player.GetComponent<PlayerController>().GetIsHiding())
         * {
         *      character.ChangeDemonState(DemonCharacter.DemonStates.Patrol);
         * }
         */

        if (character.raycastToPlayer.PlayerDetected()) //player is detected. following player function is called. 
        {
            isChasing = true;

            if (timeAlone != 0)
                timeAlone = 0.0f;

            lastTargetPos = character.player.transform.position;

            MoveTowardsPlayer();

            Collider[] colliders = Physics.OverlapSphere(character.transform.position, character.attackRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject == character.player)
                    character.ChangeDemonState(DemonCharacter.DemonStates.Attack);
            }
        }     
        else
        {
            character.agent.SetDestination(lastTargetPos);
            timeAlone += Time.deltaTime;

            isChasing = false;

            if (timeAlone >= maxTimeAlone)
                character.ChangeDemonState(DemonCharacter.DemonStates.Patrol); //changes state to patrol
        }

        character.animator.SetBool("isChasing", isChasing);
    }

    /// <summary>
    /// <para>Primary Function in escorting state.</para>
    /// <para>Checks if player is within range and follows them if true.</para>
    /// </summary>
    void MoveTowardsPlayer()
    {
        // INFO: Ensures the NPC only rotates on the y-axis
        Vector3 playerPosition = new(targetPos.x, transform.position.y, targetPos.z);
        transform.LookAt(playerPosition);

        character.agent.SetDestination(targetPos); // sets target position to player last pos 
    }
}
