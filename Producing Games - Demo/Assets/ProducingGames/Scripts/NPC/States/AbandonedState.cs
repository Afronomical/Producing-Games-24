using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matt Brake
/// <para> Moderated By: </para>
/// <para> Clarifies the behaviour of the NPC when abandoned by player.</para>
/// </summary>

public class AbandonedState : StateBaseClass
{
    private RaycastToPlayer raycastToPlayer;
    private float timeAbandoned = 0;
    private readonly float maxTimeAbandoned = 5f; 

    public override void UpdateLogic()
    {
        GetComponent<AICharacter>().isMoving = false; //flags character as not moving

        timeAbandoned += Time.deltaTime; //logs the time they NPC has been abandoned for.

        if (raycastToPlayer == null)
            raycastToPlayer = GetComponent<RaycastToPlayer>();

        
        if (timeAbandoned > maxTimeAbandoned)
            character.ChangeState(AICharacter.States.Wandering); // once the max time for abandonment is reached, NPC goes wandering. 
    }
}
