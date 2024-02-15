using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matt Brake 
/// <para> Moderated By: Matej Cincibus</para>
/// <para> Manages the behaviour of AI when they die. </para>
/// </summary>

public class DeadState : PatientStateBaseClass
{
    private void Awake()
    {
       // Debug.Log("NPC DEAD");
       
        transform.Rotate(0, 0, -90.0f); //placeholder function to display death
       
    }
    private void Start()
    {
        character.agent.velocity = Vector3.zero;
        character.agent.ResetPath(); 
        character.rb.velocity = Vector3.zero;
        GameManager.Instance.DecrementRemainingPatients();
    }

    public override void UpdateLogic()
    {
    }
}
