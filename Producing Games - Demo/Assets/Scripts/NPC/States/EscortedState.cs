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
    public override void UpdateLogic()
    {
        followPos = character.player.transform.position;
        GetComponent<AICharacter>().isMoving = true;
        if(RaycastToPlayer == null)
        {
            RaycastToPlayer = GetComponent<RaycastToPlayer>();
        }
        
       
        
       if(RaycastToPlayer != null && RaycastToPlayer.PlayerDetected())
        {
           // Debug.Log("calling move towards player");
            HasPickedUpNPC = true;
            MoveTowardsPlayer(); 
        }
        else
        {
            ///enter abandoned state 
            Debug.Log(" not detected"); 
        }


       if(HasPickedUpNPC && !RaycastToPlayer.PlayerDetected())
        {
            Debug.Log("Abandoned NPC!");
            //character.ChangeState(AICharacter.States.Abandoned); no state script for this at time of writing. 
            return; 
        }
        

    }
   
    //void CheckRange()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, character.DetectionRadius);
    //    bool PlayerInRange = false;
    //    bool BedInRange = false;

    //    foreach(Collider collider in colliders)
    //    {
    //        if(collider.CompareTag("Player"))
    //        {
    //            PlayerInRange = true;
    //            //FollowPlayer(); 
    //        }
    //    }
    //}


    /// <summary>
    /// <para>Primary Function in escorting state.</para>
    /// <para>Checks if player is within range and follows them if true.</para>
    /// </summary>
    void MoveTowardsPlayer()
    {
       
           
            
        

        if (character.rb != null && ShouldFollow)
        {
            character.step = character.walkSpeed * Time.deltaTime;
            currentPos = character.transform.position;
            targetPos = followPos;

            Vector3 newPos = Vector3.MoveTowards(currentPos, targetPos, character.EscortSpeed);
            character.transform.LookAt(character.player.transform);

            character.rb.MovePosition(newPos); 
           // Debug.Log("moving to: " + newPos); 
        }
        else
        {
            //Debug.Log("rb is null"); 
        }
       if(RaycastToPlayer.PlayerDistance < 3)
        {
            targetPos = currentPos;
            ShouldFollow = false;
            targetPos = Vector3.zero;
        }
       else
        {
            ShouldFollow = true;
        }
    }
    
    


}
