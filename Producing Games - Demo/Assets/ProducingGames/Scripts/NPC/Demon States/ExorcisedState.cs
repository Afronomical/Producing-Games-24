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
        ///maybe scream sound effect of some kind

        //bool to change animation to death
        GetComponent<Animator>().SetBool("isExorcised", true);
        GetComponent<Animator>().SetBool("isChasing", false);

        //invoke the destruction of the character
    }
    public override void UpdateLogic()
    {
        Invoke("Exorcise", 3);
        
    }

    void Exorcise()
    {
        GetComponent<AICharacter>().isMoving = false; 
        gameObject.SetActive(false); //de activates the Demon 

    }
}
