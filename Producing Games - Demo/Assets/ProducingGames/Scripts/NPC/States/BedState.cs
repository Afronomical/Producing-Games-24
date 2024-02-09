using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matt Brake 
/// <para> Moderated By: Matej Cincibus</para>
/// <para> Manages the behaviour of AI when laying in bed. </para>
/// </summary>

public class BedState : StateBaseClass
{
    private int wanderingChance;
    private int heartAttackChance;

    private void Awake()
    {
        GetComponent<AICharacter>().isMoving = false;
    }

    public override void UpdateLogic()
    {
        //Debug.Log("Player in Bed State"); 
        ////fix character transform to bed pos. 
    }
}
