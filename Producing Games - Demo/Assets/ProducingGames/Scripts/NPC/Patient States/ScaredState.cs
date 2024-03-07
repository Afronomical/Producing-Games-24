using UnityEngine;

/// <summary>
/// Written By: Matthew Brake
/// <para> Moderated By: Matej Cincibus</para>
/// Manages the behaviour of the patients before hour 7, when they encounter the demon or any paranormal activity. 
/// <para> The main difference between this and panic state is the lack of the cowering behaviour, and the consideration of paranormal events</para>
/// 
/// </summary>
public class ScaredState : PatientStateBaseClass
{
    // INFO: Need reference to current horror events happening 
    private bool detectedBed = false;
    private Vector3 safetyLocation;
    private float calmingTime;

    private void Start()
    {
        // INFO: If the previous state was the bed state, we firstly need to
        // teleport the agent to the closest point on the navmesh before assigning
        // their destination location
        character.NearestNavMeshPoint();

        character.agent.speed = character.runSpeed;

        // INFO: Plays the scared SFX
        if (character.scaredNPC != null)
            AudioManager.instance.PlaySound(character.scaredNPC, character.transform);

        // INFO: Set the animation
        character.animator.SetBool("isScared", true);

        // INFO: If the previous patient state was the bed state, we don't
        // want the safety choice to be their bed, as they wouldn't go anywhere
        // so we need to choose the other (hiding location)
        if (character.PreviousState == PatientCharacter.PatientStates.Bed)
            character.safetyChoice = SafetyChoices.HidingSpot;
        else
            character.safetyChoice = character.GetRandomEnum<SafetyChoices>();

        // INFO: Randomly chooses a safety location based on the chosen
        // enum member
        safetyLocation = character.SafetyChooser();

        character.agent.SetDestination(safetyLocation);
    }

    public override void UpdateLogic()
    {
        // INFO: Given that the patient reaches their hiding location they will then
        // wait for a while before going into another state as they are no longer scared
        if (character.agent.remainingDistance < 0.1f)
        {
            calmingTime += Time.deltaTime;

            if (calmingTime > character.calmingDuration)
            {
                // INFO: Returns to the previous state before the patient became scared, to
                // ensure the task system does not break
                character.ChangePatientState(character.PreviousState);
            }
        }
    }

    /*
    public override void UpdateLogic()
    {
        if (character.CheckBedInRange())
        {
            // INFO: Prevents unnecessary set destination calls
            if (!detectedBed)
            {
                detectedBed = true;
                character.agent.SetDestination(character.BedDestination.position);
            }

            transform.LookAt(new Vector3(character.BedDestination.position.x,
                transform.position.y, character.BedDestination.position.z));

            // INFO: Switches to bed state once patient gets close enough to the bed
            if (character.agent.remainingDistance < 0.1f)
                character.ChangePatientState(PatientCharacter.PatientStates.Bed);
        }
    }
    */
}
