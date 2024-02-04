using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : StateBaseClass
{
    public override void UpdateLogic()
    {
        Debug.Log("NPC DEAD");
        character.rb.velocity = Vector3.zero;
        character.agent.velocity = Vector3.zero;
        character.agent.ResetPath(); 
        transform.Rotate(0, 0 ,- 90);
    }
}
