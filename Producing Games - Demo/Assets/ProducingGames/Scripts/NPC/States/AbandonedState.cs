using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Written By: Matt Brake
/// <para> Moderated By: ..... </para>
/// <para> Clarifies the behaviour of the NPC when abandoned by player.</para>
/// </summary>

public class AbandonedState : StateBaseClass
{
    private RaycastToPlayer RaycastToPlayer;
    private float TimeAbandoned = 0;
    private float MaxTimeAbandoned = 5f; 

    public override void UpdateLogic()
    {
        TimeAbandoned += Time.deltaTime; //logs the time they NPC has been abandoned for.

        GetComponent<AICharacter>().isMoving = false;  ///flags character as not moving 
        if (RaycastToPlayer == null)
        {
            RaycastToPlayer = GetComponent<RaycastToPlayer>();
        }

        
        if (TimeAbandoned > MaxTimeAbandoned)
        {
            character.ChangeState(AICharacter.States.Wandering); /// once the max time for abandonment is reached, NPC goes wandering. 
        }
    }
}
