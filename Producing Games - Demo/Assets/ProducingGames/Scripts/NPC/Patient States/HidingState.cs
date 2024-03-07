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

        character.agent.transform.position = hidingLocation;
        character.agent.Warp(hidingLocation);

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
    /// When the script is destroyed (changes state) it will free up the hiding
    /// spot location ready for when the next hiding task is set for a patient
    /// </summary>
    private void OnDestroy()
    {
        NPCManager.Instance.SetHidingLocationFree(hidingLocation);
    }
}
