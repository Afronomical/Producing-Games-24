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
        character.agent.enabled = false;
        Transform pos = character.bed.transform.Find("PatientPosition");
        transform.position = pos.position;
        Debug.Log(gameObject.name + ": requests medication.");
    }

    public override void UpdateLogic()
    {

    }
}
