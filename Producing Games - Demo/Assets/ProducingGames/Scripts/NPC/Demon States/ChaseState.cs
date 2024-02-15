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

        /*
         * if (character.player.GetComponent<PlayerController>().GetIsHiding())
         * {
         *      character.ChangeDemonState(DemonCharacter.DemonStates.Patrol);
         * }
         */

        if (character.raycastToPlayer.PlayerDetected()) //player is detected. following player function is called. 
        {
            if (timeAlone != 0)
                timeAlone = 0.0f;

            lastTargetPos = character.player.transform.position;

            MoveTowardsPlayer();

            Collider[] colliders = Physics.OverlapSphere(character.transform.position, character.attackRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject == character.player)
                {
                    Debug.Log("Changing To attack state");
                    character.ChangeDemonState(DemonCharacter.DemonStates.Attack);
                }
            }
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
