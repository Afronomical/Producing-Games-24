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
        
       
        
       if(RaycastToPlayer.PlayerDetected()) //player is detected. following player function is called. 
        {
          
            HasPickedUpNPC = true;
            
            lastPlayerPos = character.player.transform.position;

            MoveTowardsPlayer(); 
        }
       else if(HasPickedUpNPC && !RaycastToPlayer.PlayerDetected()) ///if NPC previously picked up but currently not detecting player 
        {
            character.agent.SetDestination(lastPlayerPos); ///NPC moves to player last known position to check whether they are still in range 

            
            TimeAlone += Time.deltaTime;
            
            if(TimeAlone >= MaxTimeAlone) ///gives player 3 seconds to recollect NPC before they enter wandering state again 
            {
                character.ChangeState(AICharacter.States.Abandoned); //changes state to abandoned
                TimeAlone = 0;
            }

            return; 
        }

       if(HasPickedUpNPC)
        {
           if(CheckBedInRange()) ///checking whether bed is in range 
            {
               
                BedInRange = true;
                ShouldFollow = false;
                character.ChangeState(AICharacter.States.Bed);
                ///move to bed instead of player, and then enter bed state 
            }
        }
        

    }


    bool CheckBedInRange() /*only perform if player has picked NPC up */ //function to check whether bed in range 
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

            
            character.transform.LookAt(character.player.transform);

            
            character.agent.SetDestination(targetPos); /// sets target position to player last pos 
           
        }

        

       if(RaycastToPlayer.PlayerDistance < 3)  ////stops NPC moving any closer than 3 units 
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
