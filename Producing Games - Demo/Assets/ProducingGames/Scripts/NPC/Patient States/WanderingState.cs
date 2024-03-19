using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: Matt Brake 
/// 
/// The wandering state allows the NPC to wander around the map by choosing a destination
/// location from a list of potential locations that the NPC can travel to
/// </summary>

public class WanderingState : PatientStateBaseClass
{
    private Vector3 wanderDestination;
    private float wanderingIdleTime = 0.0f;

    private void Start()
    {
        character.agent.speed = character.walkSpeed;

        // INFO: Plays the desired male voice line if the patient is a male
        // otherwise plays the desirted female voice line
        if (character.isMale)
        {
           // NPCManager.Instance.PlayMaleVoiceLine(NPCManager.MaleVoiceLines.ManOneWanderOne, transform);
        }
        else
        {
           // NPCManager.Instance.PlayFemaleVoiceLine(NPCManager.FemaleVoiceLines.FemOneWanderOne, transform);
        }

        // INFO: Places the patient on the navmesh if they aren't already on it
        if (!character.agent.isOnNavMesh)
            character.NearestNavMeshPoint();

        ChooseWanderingDestination();
    }

    public override void UpdateLogic()
    {
        character.animator.SetFloat("movement", character.agent.velocity.magnitude);

        // INFO: Given that the patient is near to the destination location a timer is started
        if ((character.transform.position - wanderDestination).sqrMagnitude < character.distanceFromDestination)
        {
            wanderingIdleTime += Time.deltaTime;

            // INFO: After the patient has waited at its destination location for a specified
            // time it will then choose a different location to move towards
            if (wanderingIdleTime > character.wanderingIdleDuration)
            {
                wanderingIdleTime = 0.0f;
                ChooseWanderingDestination();
            }
        }
    }

    /// <summary>
    /// Chooses a destination from an available list of destination locations held in the NPC manager 
    /// </summary>
    private void ChooseWanderingDestination()
    {
        // INFO: If there are no wandering destinations in the list then end
        if (NPCManager.Instance.GetWanderingDestinationsCount() == 0)
        {
            Debug.LogWarning("There are no wandering destinations setup in the wandering destinations list.");
            return;
        }

        // INFO: Sets the wandering destination that it has arrived at as free,
        // so that other NPCs can walk to it
        NPCManager.Instance.SetWanderingDestinationFree(wanderDestination);

        // INFO: Chooses a new destination to wander to
        wanderDestination = NPCManager.Instance.RandomWanderingDestination();

        character.agent.SetDestination(wanderDestination);
    }

    /// <summary>
    /// When the script is destroyed (changes state) it will free up the wandering
    /// spot location ready for when the next wandering task is set for a patient
    /// </summary>
    private void OnDestroy()
    {
        NPCManager.Instance.SetWanderingDestinationFree(wanderDestination);
    }
}
