using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// Script in-charge of the request medication state of patients
/// </summary>

public class RequestMedicationState : PatientStateBaseClass
{
    private void Start()
    {
        Debug.Log(gameObject.name + ": requests medication.");

        // INFO: Given that the previous was panicked or scared we will have
        // the patient walk back to their hiding spot
        if (character.PreviousState == PatientCharacter.PatientStates.Panic ||
            character.PreviousState == PatientCharacter.PatientStates.Scared)
            WalkToReqMedSpot();
        else
            TeleportToReqMedSpot();

        character.animator.SetBool("reqMeds", true);
    }

    /// <summary>
    /// Rather than having the patient walk to their hiding spot, we
    /// will have them teleport to it
    /// </summary>
    private void TeleportToReqMedSpot()
    {
        character.agent.Warp(character.BedDestination.position);
    }

    /// <summary>
    /// Rather than teleporting the patient, this will have the patient
    /// walk back to their hiding spot
    /// </summary>
    private void WalkToReqMedSpot()
    {
        character.agent.SetDestination(character.BedDestination.position);
    }
}
