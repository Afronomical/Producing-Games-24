using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortedState : StateBaseClass
{
    private Vector3 followPos;
    
    public override void UpdateLogic()
    {
        GetComponent<AICharacter>().isMoving = true;
        //////I NEED RIGIDBODY  
        
        if (followPos != Vector3.zero)
        {
            followPos = character.player.transform.position; 
            
        }
    }
}
