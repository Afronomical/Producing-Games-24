using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matt Brake
/// <para>Moderated By: Matej Cincibus </para>
/// <para> The Demon state once a completed exorcism has taken place </para>
/// </summary>
public class ExorcisedState :DemonStateBaseClass
{

   
    private void Start()
    {
        GetComponent<AICharacter>().isMoving = false; 
        gameObject.SetActive(false);
        ///maybe scream sound effect of some kind 
    }
    public override void UpdateLogic()
    {
        
    }
}
