using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matthew Brake
/// <para> Moderated By: .....</para>
/// Manages the behaviour of the patients before hour 7 when they encounter the demon or any paranormal activity. 
/// </summary>
public class ScaredState : PatientStateBaseClass
{
    //need reference to current horror events happening 
    public SoundEffect NPCScared;
    private float currentEventDistance;


   

    private void Start()
    {
        if (character.agent.hasPath)
            character.agent.ResetPath();
        character.agent.speed = character.runSpeed; 
    }
}
