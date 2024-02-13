using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// Script in-charge of finding a hiding location and having the patient
/// walk and hide at that location
/// </summary>

public class HidingState : StateBaseClass
{
    private Vector3 hidingLocation;

    private void Awake()
    {
        GetComponent<AICharacter>().isMoving = true;
    }

    private void Start()
    {
        ChooseLocation();

        character.agent.speed = character.walkSpeed;
        character.agent.ResetPath();
    }

    public override void UpdateLogic()
    {
        character.agent.SetDestination(hidingLocation);
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
}
