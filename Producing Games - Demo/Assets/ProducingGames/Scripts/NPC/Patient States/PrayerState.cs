using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matt Brake
/// <para> Moderated By: .....</para>
/// <para> This State carries out behaviour of the NPCs when they are praying. They will run to a prayer point 
/// and carry out a prayer until the player interacts with them.</para>
/// </summary>
public class PrayerState : PatientStateBaseClass
{
    private Vector3 prayingDestination;
    private float distanceFromPrayerPoint = 3.0f; 
    private bool atLocation; 
    //need references to character individual beds 

    private void Awake()
    {
        //character.isMoving = true;
    }

    private void Start()
    {
        character.agent.ResetPath();
        prayingDestination = NPCManager.Instance.RandomPrayingDestination();
        character.agent.speed = character.runSpeed;
        //maybe some sound effects to signify the praying? panic sounds or speech 
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
        ///singing/gospel, increasing sanity? 
        //Debug.Log(character.name + "is Praying"); 
       
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

    /// <summary>
    /// checks whether the patient is praying in their own room, which determines how to end the prayer state 
    /// </summary>
    /// <returns></returns>
    public bool IsLocationPatientRoom()
    {
        if(Vector3.Distance(prayingDestination,character.bed.transform.position)<distanceFromPrayerPoint)
        {
            return true;
        }
        else
        return false;
        
    }
}
