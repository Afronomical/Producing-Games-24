using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Written By: Matt Brake 
/// <para> Moderated By: ...... </para>
/// <para> Manages the behaviour of AI when laying in bed. </para>
/// </summary>
public class BedState : StateBaseClass
{
    public int WanderingChance;
    public int HeartAttackChance;  
    
    
    public override void UpdateLogic()
    {
        GetComponent<AICharacter>().isMoving = false;
        Debug.Log("Player in Bed State"); 
        ////fix character transform to bed pos. 
    }
}
