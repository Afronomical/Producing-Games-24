using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matt Brake 
/// <para> Moderated By: Matej Cincibus</para>
/// <para> Manages the behaviour of AI when they die. </para>
/// </summary>

public class DeadState : StateBaseClass
{
    private void Awake()
    {
       // Debug.Log("NPC DEAD");
       
        character.agent.velocity = Vector3.zero;
        character.agent.ResetPath(); 
        transform.Rotate(0, 0, -90.0f); //placeholder function to display death
       
    }
    private void Start()
    {
        character.rb.velocity = Vector3.zero;
    }

    public override void UpdateLogic()
    {
    }
}
