using UnityEngine;

/// <summary>
/// Written By: Matt Brake
/// <para> Moderated By: Matej Cincibus</para>
/// <para> This State carries out behaviour of the NPCs when they are praying. They will run to a prayer point 
/// and carry out a prayer until the player interacts with them.</para>
/// </summary>

public class PrayerState : PatientStateBaseClass
{
    private Vector3 prayingDestination;
    private bool isWalkingToPrayingDest = false;

    private void Start()
    {
        ChoosePrayingDestination();

        // INFO: Given that the previous was none we will have the patient
        // teleport, otherwise we have them walk to their destination
        if (character.PreviousState == PatientCharacter.PatientStates.None)
            TeleportToPrayingSpot();
        else
            WalkToPrayingSpot();
    }

    public override void UpdateLogic()
    {
        character.animator.SetFloat("movement", character.agent.velocity.magnitude);

        if (isWalkingToPrayingDest)
        {
            // INFO: Stops the walking animation and transitions to the praying animation
            if (character.agent.remainingDistance < 0.1f)
            {
                isWalkingToPrayingDest = false;

                character.animator.SetBool("isPraying", true);
            }
        }
    }

    /// <summary>
    /// Chooses a praying location from an available list of praying locations held in the NPC manager
    /// </summary>
    public void ChoosePrayingDestination()
    {
        // INFO: If there are no praying locations in the list then end
        if (NPCManager.Instance.GetPrayerLocationsCount() == 0)
        {
            Debug.LogError("There are no praying locations setup in the praying location list.");
            return;
        }

        // INFO: Chooses a location to be praying at
        prayingDestination = NPCManager.Instance.RandomPrayingDestination();
    }

    /// <summary>
    /// Rather than having the patient walk to their hiding spot, we
    /// will have them teleport to it
    /// </summary>
    private void TeleportToPrayingSpot()
    {
        character.agent.Warp(prayingDestination);

        character.animator.SetBool("isPraying", true);
    }

    /// <summary>
    /// Rather than teleporting the patient, this will have the patient
    /// walk back to their hiding spot
    /// </summary>
    private void WalkToPrayingSpot()
    {
        character.ResetAnimation();
        
        character.agent.speed = character.walkSpeed;
        character.agent.SetDestination(prayingDestination);

        isWalkingToPrayingDest = true;
    }

    /// <summary>
    /// When the script is destroyed (changes state) it will free up the praying
    /// spot location ready for when the next pray task is set for a patient
    /// </summary>
    private void OnDestroy()
    {
        NPCManager.Instance.SetHidingLocationFree(prayingDestination);
    }

    /*
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
    */
}
