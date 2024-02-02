using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// <para>Written By: Matt Brake</para>
/// Moderated By: ........ 
/// 
///<para>Tracks the behaviour of AI when being escorted back to their room.</para>
///<para>Currently will just move towards player transform, until pathfinding is properly in place.</para>
/// </summary>

public class EscortedState : StateBaseClass
{
    private Vector3 followPos;
    private Vector3 currentPos;
    private Vector3 targetPos;
    private RaycastToPlayer RaycastToPlayer;
    private bool ShouldFollow = true;
    private bool HasPickedUpNPC = false;
    private bool BedInRange; 
    private float TimeAlone = 0;
    private float MaxTimeAlone = 5f;

    private Vector3 lastPlayerPos;

    private void Start()
    {
        character.agent.ResetPath();
    }

    public override void UpdateLogic()
    {

        followPos = character.player.transform.position;
        GetComponent<AICharacter>().isMoving = true;
        if(RaycastToPlayer == null)
        {
            RaycastToPlayer = GetComponent<RaycastToPlayer>();
        }
        
       
        
       if(RaycastToPlayer.PlayerDetected())
        {
           // Debug.Log("calling move towards player");
            HasPickedUpNPC = true;
            
            lastPlayerPos = character.player.transform.position;

            MoveTowardsPlayer(); 
        }
       else if(HasPickedUpNPC && !RaycastToPlayer.PlayerDetected())
        {
            character.agent.SetDestination(lastPlayerPos);

            //Debug.Log("Abandoned NPC!");
            TimeAlone += Time.deltaTime;
            //Debug.Log(TimeInAbandonded);
            if(TimeAlone >= MaxTimeAlone) ///gives player 3 seconds to recollect NPC before they enter wandering state again 
            {
                character.ChangeState(AICharacter.States.Abandoned); //no state script for this at time of writing. 
                TimeAlone = 0;
            }

            return; 
        }

       if(HasPickedUpNPC)
        {
           if(CheckBedInRange())
            {
                //Debug.Log("BED LOCATED");
                BedInRange = true;
                ShouldFollow = false;
                character.ChangeState(AICharacter.States.Bed);
                ///move to bed instead of player, and then enter bed state 
            }
        }
        

    }


    bool CheckBedInRange() /*only perform if player has picked NPC up */
    {
        
        
        Collider[] colliders = Physics.OverlapSphere(character.transform.position, character.DetectionRadius);
        foreach(Collider collider in colliders)
        {
            if(collider.gameObject.name == "Bed") ///just for testing 
            {
                //Debug.Log("found bed"); 
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


        TimeAlone = 0;
        

        if (character.rb != null && ShouldFollow == true)
        {
            character.step = character.walkSpeed * Time.deltaTime;
            currentPos = character.transform.position;
            targetPos = followPos;

            //Vector3 newPos = Vector3.MoveTowards(currentPos, targetPos, character.EscortSpeed);

            //if (RaycastToPlayer.PlayerDistance > 3)
            //{
            //    newPos = Vector3.MoveTowards(currentPos, targetPos, character.EscortSpeed);
            //}
            character.transform.LookAt(character.player.transform);

            //character.rb.MovePosition(newPos);
            character.agent.SetDestination(targetPos);
            //if (RaycastToPlayer.PlayerDistance > 3)
                //character.agent.SetDestination(newPos);
            //else
                //character.agent.isStopped = true;
           // Debug.Log("moving to: " + newPos); 
        }

        //Debug.Log(character.rb.velocity);

       if(RaycastToPlayer.PlayerDistance < 3)
        {
            targetPos = currentPos;
            ShouldFollow = false;
            targetPos = Vector3.zero;
            character.agent.ResetPath();
            character.agent.isStopped = true;
            character.rb.velocity = Vector3.zero;
        }
       else
        {
            ShouldFollow = true;
            character.agent.isStopped = false;
        }
    }
    
    


}
