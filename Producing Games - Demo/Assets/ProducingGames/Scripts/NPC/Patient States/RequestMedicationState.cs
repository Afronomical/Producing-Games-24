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
        if (character.agent.hasPath)
            character.agent.ResetPath();

        character.agent.enabled = false;

        Transform pos = character.bed.transform.Find("PatientPosition");
        transform.position = pos.position;

        Debug.Log(gameObject.name + ": requests medication.");

        character.animator.SetBool("reqMeds", true);
    }

    /*public override void UpdateLogic()
    {
    }*/
}
