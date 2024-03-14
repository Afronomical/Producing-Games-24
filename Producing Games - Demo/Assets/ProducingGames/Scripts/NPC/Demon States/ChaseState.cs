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
    private Vector3 playerPos;
    private Vector3 lastPlayerPos;

    private bool isChasing = false;

    private float chaseAloneTime = 0;

    private void Start()
    {
        character.agent.speed = character.runSpeed;
    }

    public override void UpdateLogic()
    {
        character.animator.SetFloat("movement", character.agent.velocity.magnitude);
        character.animator.SetBool("isChasing", isChasing);

        playerPos = character.player.transform.position;

        
          if (character.player.GetComponent<PlayerMovement>().isHiding)
          {
               character.ChangeDemonState(DemonCharacter.DemonStates.Patrol);
          }
         

        // INFO: If the player is detected, the following player function is called
        if (character.raycastToPlayer.PlayerDetected())
        {
            if (chaseAloneTime != 0)
                chaseAloneTime = 0.0f;

            isChasing = true;
            lastPlayerPos = character.player.transform.position;

            MoveTowardsPlayer();
            Attack();
        }
        else
        {
            // INFO: Demon goes to the last known player location
            // Prevents running set destination call multiple times
            if (isChasing)
            { 
                isChasing = false;
                character.agent.SetDestination(lastPlayerPos);
            }

            chaseAloneTime += Time.deltaTime;

            if (chaseAloneTime > character.chaseAloneDuration)
                character.ChangeDemonState(DemonCharacter.DemonStates.Patrol);
        }
    }

    private void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(character.transform.position, character.attackRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == character.player)
                character.ChangeDemonState(DemonCharacter.DemonStates.Attack);
        }
    }

    /// <summary>
    /// <para>Primary Function in escorting state.</para>
    /// <para>Checks if player is within range and follows them if true.</para>
    /// </summary>
    void MoveTowardsPlayer()
    {
        // INFO: Ensures the demon only rotates on the y-axis
        Vector3 playerPosition = new(playerPos.x, transform.position.y, playerPos.z);

        transform.LookAt(playerPosition);

        character.agent.SetDestination(playerPos);
    }
}
