using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// Script in-charge of the request medication state of patients
/// </summary>

public class RequestMedicationState : PatientStateBaseClass
{
    private bool isWalkingToReqMeds = false;

    private void Start()
    {
        Debug.Log(gameObject.name + ": requests medication.");

        // INFO: Given that the previous was none we will have the patient
        // teleport, otherwise we have them walk to their destination
        if (character.PreviousState == PatientCharacter.PatientStates.None)
            TeleportToReqMedSpot();
        else
            WalkToReqMedSpot();
    }

    public override void UpdateLogic()
    {
        if (isWalkingToReqMeds)
        {
            // INFO: Stops the walking animation and transitions to the req meds animation
            if (character.agent.remainingDistance < 0.1f)
            {
                isWalkingToReqMeds = false;

                character.animator.SetBool("reqMeds", true);
            }
        }
    }

    /// <summary>
    /// Rather than having the patient walk to their hiding spot, we
    /// will have them teleport to it
    /// </summary>
    private void TeleportToReqMedSpot()
    {
        character.agent.Warp(character.BedDestination.position);

        character.animator.SetBool("reqMeds", true);
    }

    /// <summary>
    /// Rather than teleporting the patient, this will have the patient
    /// walk back to their hiding spot
    /// </summary>
    private void WalkToReqMedSpot()
    {
        character.ResetAnimation();

        character.agent.speed = character.walkSpeed;
        character.agent.SetDestination(character.BedDestination.position);

        isWalkingToReqMeds = true;
    }
}
