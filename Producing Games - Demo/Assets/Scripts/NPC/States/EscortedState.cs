using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Written By: Matt Brake
/// Moderated By: ........ 
/// 
/// Tracks the behaviour of AI when being escorted back to room 
/// currently will just move towards player transform, until pathfinding is properly in place. 
/// </summary>

public class EscortedState : StateBaseClass
{
    private Vector3 followPos;
    private RaycastToPlayer RaycastToPlayer; 
    public override void UpdateLogic()
    {
        GetComponent<AICharacter>().isMoving = true;
        //////I NEED RIGIDBODY  
        
       if(RaycastToPlayer.PlayerDetected())
        {
            ///move towards player 
        }
        else
        {
            ///enter abandoned state 
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

   
    void MoveTowardsPlayer()
    {
        if (followPos != Vector3.zero)
        {
            followPos = character.player.transform.position;
            //GetComponent<AICharacter>().rb
        }

        if (character.rb != null)
        {
            character.step = character.walkSpeed * Time.deltaTime;
            character.rb.MovePosition(Vector3.MoveTowards(character.transform.position, followPos, character.step));
        }
    }
    


}
