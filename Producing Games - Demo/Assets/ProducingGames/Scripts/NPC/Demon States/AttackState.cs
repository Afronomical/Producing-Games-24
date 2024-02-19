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
        GetComponent<Animator>().SetBool("isAttacking", true);
        if(!hasMoved) //to prevent continuous snapping 
        MoveToStartPos();
    }


    private void MoveToStartPos() //temporary function to snap player back to start position 
    {
        //this is where animation will go before moving player back to start pos
        //GetComponent<Animator>().SetBool("isAttacking", false);
        //GetComponent<Animator>().SetBool("isChasing", false);

        //character.player.transform.position = GameManager.Instance.playerStartPosition.position;
        //GameManager.Instance.currentTime = 60;  // End the hour
        StartCoroutine(GameManager.Instance.EndHour());
        hasMoved = true;
        //character.ChangeDemonState(DemonCharacter.DemonStates.Inactive);
    }
}   
