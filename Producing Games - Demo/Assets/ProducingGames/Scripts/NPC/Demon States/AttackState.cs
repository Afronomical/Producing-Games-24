using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matt Brake
/// Moderated By: ...
/// <para> The Behaviour of the demon once caught the player  </para>
/// </summary>

public class AttackState : DemonStateBaseClass
{
  
    private bool hasMoved;

    private void Awake()
    {
        //startPos = character.startPosition;
    }
    private void Start()
    {
        hasMoved = false;
        Debug.Log("attacking"); 
    }

    public override void UpdateLogic()
    {
        if(!hasMoved)
        MoveToStartPos();
    }


    private void MoveToStartPos()
    {
        //this is where animation will go before moving player back to start pos 
        character.player.transform.position = GameManager.Instance.playerStartPosition.position;
        hasMoved = true;

    }
}   
