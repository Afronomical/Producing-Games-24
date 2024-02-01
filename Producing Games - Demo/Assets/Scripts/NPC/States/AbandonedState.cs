using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbandonedState : StateBaseClass
{
    private RaycastToPlayer RaycastToPlayer;
    private float TimeAbandoned = 0;
    private float MaxTimeAbandoned = 5f; 

    public override void UpdateLogic()
    {
        TimeAbandoned += Time.deltaTime;

        GetComponent<AICharacter>().isMoving = false;
        Debug.Log("NPC in Abandoned State");
        if (RaycastToPlayer == null)
        {
            RaycastToPlayer = GetComponent<RaycastToPlayer>();
        }

        if(RaycastToPlayer.PlayerDetected())
        {
            character.ChangeState(AICharacter.States.Escorted);
        }
        
        if (TimeAbandoned > MaxTimeAbandoned)
        {
            character.ChangeState(AICharacter.States.Wandering);
        }
    }
}
