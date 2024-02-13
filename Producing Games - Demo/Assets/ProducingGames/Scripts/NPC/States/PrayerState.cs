using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matt Brake
/// <para> Moderated By: .....</para>
/// <para> This State carries out behaviour of the NPCs when they are praying. They will run to a prayer point 
/// and carry out a prayer until the player interacts with them.</para>
/// </summary>
public class PrayerState : StateBaseClass
{
    private Vector3 prayingDestination;
    private float distanceFromPrayerPoint = 3.0f; 
    private bool atLocation; 

    private void Awake()
    {
        character.isMoving = true;
    }

    private void Start()
    {
        prayingDestination = NPCManager.Instance.RandomPrayingDestination();
        character.agent.ResetPath();
        character.agent.speed = character.runSpeed;
        //maybe some sound effects to signify the praying? panic sounds or latin speech 
    }


    public override void UpdateLogic()
    {
        character.agent.SetDestination(prayingDestination);
        if(CheckDistanceToLocation())
        {
            atLocation = true;
        }
        if(atLocation)
        {
            character.agent.ResetPath(); 
            ExecutePrayer();
        }
    }

    /// <summary>
    /// The main function which carries out the praying logic. 
    /// </summary>
    public void ExecutePrayer()
    {

    }


    /// <summary>
    /// checks the distance between NPC and Prayer Point to determine whether can enter praying logic. 
    /// </summary>
    /// <returns></returns>
    public bool CheckDistanceToLocation()
    {
        if (Vector3.Distance(character.transform.position, prayingDestination) < distanceFromPrayerPoint)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
