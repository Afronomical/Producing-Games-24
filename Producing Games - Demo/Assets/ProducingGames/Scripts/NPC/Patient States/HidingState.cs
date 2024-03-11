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

    private void Start()
    {
        ChooseHidingLocation();

        // INFO: Given that the previous was panicked or scared we will have
        // the patient walk back to their hiding spot
        if (character.PreviousState == PatientCharacter.PatientStates.Panic ||
            character.PreviousState == PatientCharacter.PatientStates.Scared)
            WalkToHidingSpot();
        else
            TeleportToHidingSpot();

        // PLAY HIDING ANIMATION HERE
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
        hidingLocation = NPCManager.Instance.RandomHidingLocation();
    }

    /// <summary>
    /// Rather than having the patient walk to their hiding spot, we
    /// will have them teleport to it
    /// </summary>
    private void TeleportToHidingSpot()
    {
        character.agent.Warp(hidingLocation);
    }

    /// <summary>
    /// Rather than teleporting the patient, this will have the patient
    /// walk back to their hiding spot
    /// </summary>
    private void WalkToHidingSpot()
    {
        character.agent.SetDestination(hidingLocation);
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
