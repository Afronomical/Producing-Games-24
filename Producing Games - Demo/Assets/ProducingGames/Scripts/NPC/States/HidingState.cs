using UnityEngine;

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
