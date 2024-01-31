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
    private Vector3 currentPos;
    private Vector3 targetPos;
    private RaycastToPlayer RaycastToPlayer;
    private bool ShouldFollow = true;
    public override void UpdateLogic()
    {
        followPos = character.player.transform.position;
        GetComponent<AICharacter>().isMoving = true;
        if(RaycastToPlayer == null)
        {
            RaycastToPlayer = GetComponent<RaycastToPlayer>();
        }
        
        //////I NEED RIGIDBODY  
        
       if(RaycastToPlayer != null && RaycastToPlayer.PlayerDetected())
        {
            Debug.Log("calling move towards player");
            
            MoveTowardsPlayer(); 
        }
        else
        {
            ///enter abandoned state 
            Debug.Log(" not detected"); 
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
       
           
            
        

        if (character.rb != null && ShouldFollow)
        {
            character.step = character.walkSpeed * Time.deltaTime;
            currentPos = character.transform.position;
            targetPos = followPos;

            Vector3 newPos = Vector3.MoveTowards(currentPos, targetPos, character.EscortSpeed);
            character.transform.LookAt(character.player.transform);

            character.rb.MovePosition(newPos); 
            Debug.Log("moving to: " + newPos); 
        }
        else
        {
            Debug.Log("rb is null"); 
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
