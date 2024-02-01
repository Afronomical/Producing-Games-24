using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachineController : MonoBehaviour
{
    // Handles the switching of the states depending on if certain conditions are met
    private AICharacter character;
    public float detectionRange = 4f;
    public float attackRange = 1f;
    public LayerMask unwalkableLayer;
    private float distance;
    private Vector3 lastPosition;
    private int stuckCheckFrames;
    private float changeStateTimer;
    private float changeStateTime = 0.25f;


    private void Start()
    {
        character = GetComponent<AICharacter>();
        changeStateTimer = UnityEngine.Random.Range(-2.5f, changeStateTime);
    }


    private void Update()
    {
        changeStateTimer += Time.deltaTime;
        if (changeStateTimer > changeStateTime)  // Don't try to change the state every frame
        {
            changeStateTimer = 0f;
            CheckState();
        }
    }


    private void CheckState()
    {
        distance = Vector3.Distance(character.player.transform.position, character.transform.position);


        
        


        //if (character.isMoving && character.currentState != AICharacter.States.Run && character.currentState != AICharacter.States.Downed)  // Check to see if the character is stuck on an object
        //{
        //    if (StuckCheck())
        //    {
        //        character.isMoving = false;
        //        character.ChangeState(AICharacter.States.None);
        //    }
        //}
        //else
        //    stuckCheckFrames = 0;

        lastPosition = transform.position;  // Update the last position of this character
    }




    private bool RaycastToPlayer(float range)
    {
        if (Physics2D.Raycast(transform.position, (character.player.transform.position - transform.position), range, unwalkableLayer))
            return false;  // The raycast hit a wall
        else return true;  // The enemy can see the player
    }


    private bool StuckCheck()
    {
        if (Vector3.Distance(lastPosition, transform.position) > 0.2f)  // They are moving
        {
            stuckCheckFrames = 0;
            return false;
        }
        else  // They haven't moved enough
        {
            stuckCheckFrames++;
            if (stuckCheckFrames >= 4)
            {
                stuckCheckFrames = 0;
                return true;  // They are stuck
            }
            else
                return false;  // They haven't been stuck long enough
        }
    }
}
