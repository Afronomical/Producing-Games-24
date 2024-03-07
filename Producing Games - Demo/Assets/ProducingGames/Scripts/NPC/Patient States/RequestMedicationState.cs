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

        transform.SetPositionAndRotation(character.BedDestination.position, character.BedDestination.rotation);

        character.animator.SetBool("reqMeds", true);
    }
}
