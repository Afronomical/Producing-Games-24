using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matt Brake
/// <para> Moderated By: Matej Cincibus</para>
/// <para> Clarifies the behaviour of the NPC when abandoned by player.</para>
/// </summary>

public class AbandonedState : StateBaseClass
{
    private readonly float maxTimeAbandoned = 5f;
    private float timeAbandoned = 0;

    private RaycastToPlayer raycastToPlayer;

    private void Awake()
    {
        GetComponent<AICharacter>().isMoving = false; //flags character as not moving

        if (raycastToPlayer == null)
            raycastToPlayer = GetComponent<RaycastToPlayer>();
    }

    public override void UpdateLogic()
    {
        timeAbandoned += Time.deltaTime; //logs the time they NPC has been abandoned for.
        
        if (timeAbandoned > maxTimeAbandoned)
            character.ChangeState(AICharacter.States.Wandering); // once the max time for abandonment is reached, NPC goes wandering. 
    }
}
