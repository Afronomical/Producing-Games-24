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
    private readonly float distanceFromPrayerPoint = 3.0f; 

    private void Start()
    {
        if (character.agent.hasPath)
            character.agent.ResetPath();

        character.agent.enabled = false;

        prayingDestination = NPCManager.Instance.RandomPrayingDestination();

        //maybe some sound effects to signify the praying? panic sounds or speech 
        character.agent.transform.position = prayingDestination;
        character.agent.Warp(prayingDestination);

        character.animator.SetBool("isPraying", true);
    }

    /*public override void UpdateLogic()
    {
    }*/

    /// <summary>
    /// The main function which carries out the praying logic. 
    /// </summary>
    public void ExecutePrayer()
    {
        // singing/gospel, increasing sanity? 
        //Debug.Log(character.name + "is Praying");   
    }

    /// <summary>
    /// checks whether the patient is praying in their own room, which determines how to end the prayer state 
    /// </summary>
    /// <returns></returns>
    public bool IsLocationPatientRoom()
    {
        if(Vector3.Distance(prayingDestination,character.bed.transform.position)<distanceFromPrayerPoint)
            return true;
        return false;     
    }
}
