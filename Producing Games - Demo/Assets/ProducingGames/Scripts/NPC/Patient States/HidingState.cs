using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// Script in-charge of finding a hiding location and having the patient
/// hide at that location by teleporting them at the start of the hour
/// </summary>

public class HidingState : PatientStateBaseClass
{
    private Vector3 hidingLocation;
    private bool isWalkingToHidingDest = false;

    private void Start()
    {
        ChooseHidingLocation();

        // INFO: Given that the previous was none we will have the patient
        // teleport, otherwise we have them walk to their destination
        if (character.PreviousState == PatientCharacter.PatientStates.None ||
            character.PreviousState == PatientCharacter.PatientStates.Bed)
            TeleportToHidingSpot();
        else
            WalkToHidingSpot();
    }

    public override void UpdateLogic()
    {
        character.animator.SetFloat("movement", character.agent.velocity.magnitude);

        if (isWalkingToHidingDest)
        {
            // INFO: Stops the walking animation and transitions to the praying animation
            if (character.agent.remainingDistance < 0.1f)
            {
                // INFO: Check whether selected hiding spot is being occupied by player
                NPCManager.Instance.HidingCutsceneLib.TryGetValue(hidingLocation, out HidingCutScene value);

                // INFO: If it isn't being occupied by the player we can break, otherwise
                // we find another hiding location
                if (value.playerIsOccupying)
                {
                    ChooseHidingLocation();
                    return;
                }

                // INFO: Occupy the hiding spot with the patient
                value.patient = character;

                isWalkingToHidingDest = false;
                TeleportToHidingSpot();
            }
        }
    }

    /// <summary>
    /// Chooses a hiding location from an available list of hiding locations held in the NPC manager
    /// </summary>
    private void ChooseHidingLocation()
    {
        // INFO: If there are no hiding locations in the list then end
        if (NPCManager.Instance.GetHidingLocationsCount() == 0)
        {
            Debug.LogError("There are no hiding locations setup in the hiding location list.");
            return;
        }

        // INFO: Chooses a location to hide at
        for (int i = 0; i < 100; i++)
        {
            hidingLocation = NPCManager.Instance.RandomHidingLocation();

            // INFO: Check whether selected hiding spot is being occupied by player
            NPCManager.Instance.HidingCutsceneLib.TryGetValue(hidingLocation, out HidingCutScene value);

            // INFO: If it isn't being occupied by the player we can break, otherwise
            // we find another hiding location
            if (!value.playerIsOccupying)
                break;
        }
        //hidingLocation = NPCManager.Instance.RandomHidingLocation();
    }

    /// <summary>
    /// Rather than having the patient walk to their hiding spot, we
    /// will have them teleport to it
    /// </summary>
    private void TeleportToHidingSpot()
    {
        //character.agent.Warp(hidingLocation);
        //character.animator.SetBool("isTerrified", true);

        // INFO: Check whether selected hiding spot is being occupied by player
        NPCManager.Instance.HidingCutsceneLib.TryGetValue(hidingLocation, out HidingCutScene value);

        if (value.patient == null)
            value.patient = character;

        character.agent.enabled = false;
        //character.rb.velocity = Vector3.zero;

        //transform.position = new(hidingLocation.x, transform.position.y, hidingLocation.z);
        transform.SetPositionAndRotation(new(hidingLocation.x, transform.position.y, hidingLocation.z), character.BedDestination.rotation);
    }

    /// <summary>
    /// Rather than teleporting the patient, this will have the patient
    /// walk back to their hiding spot
    /// </summary>
    private void WalkToHidingSpot()
    {
        character.ResetAnimation();
        
        character.agent.speed = character.walkSpeed;
        character.agent.SetDestination(hidingLocation);

        isWalkingToHidingDest = true;
    }

    /// <summary>
    /// When the script is destroyed (changes state) it will free up the hiding
    /// spot location ready for when the next hiding task is set for a patient
    /// </summary>
    private void OnDestroy()
    {
        NPCManager.Instance.SetHidingLocationFree(hidingLocation);
    }
}
