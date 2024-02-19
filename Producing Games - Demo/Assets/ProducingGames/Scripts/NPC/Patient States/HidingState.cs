using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// Script in-charge of finding a hiding location and having the patient
/// walk and hide at that location
/// </summary>

public class HidingState : PatientStateBaseClass
{
    private Vector3 hidingLocation;

    private void Awake()
    {
        GetComponent<AICharacter>().isMoving = false;
    }

    private void Start()
    {
        ChooseLocation();

        character.agent.speed = 0.0f;
        character.agent.enabled = true;
        character.agent.ResetPath();

        character.agent.transform.position = hidingLocation;
        character.agent.Warp(hidingLocation);
    }

    public override void UpdateLogic()
    {

    }

    /// <summary>
    /// Chooses a hiding location from an available list of hiding locations held in the NPC manager
    /// </summary>
    private void ChooseLocation()
    {
        // INFO: If there are no hiding locations in the list then end
        if (NPCManager.Instance.GetHidingLocationsCount() == 0)
        {
            Debug.LogWarning("There are no hiding locations setup in the hiding location list.");
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
