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

        // INFO: Given that the previous was panicked or scared we will have
        // the patient walk back to their hiding spot
        if (character.PreviousState == PatientCharacter.PatientStates.Bed)
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

                // STOP PLAYING WALKING ANIMATION

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
        // PLAY WALKING ANIMATION HERE

        isWalkingToReqMeds = true;

        character.agent.speed = character.walkSpeed;
        character.agent.SetDestination(character.BedDestination.position);
    }
}
