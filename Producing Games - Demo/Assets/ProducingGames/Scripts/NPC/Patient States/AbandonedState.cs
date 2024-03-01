using UnityEngine;

/// <summary>
/// Written By: Matt Brake
/// <para> Moderated By: Matej Cincibus</para>
/// <para> Clarifies the behaviour of the Patient when abandoned by player.</para>
/// </summary>

public class AbandonedState : PatientStateBaseClass
{
    private readonly float maxTimeAbandoned = 5f;
    private float timeAbandoned = 0;

    public override void UpdateLogic()
    {
        timeAbandoned += Time.deltaTime; //logs the time the patient has been abandoned for.

        if (timeAbandoned > maxTimeAbandoned)
        {
            character.animator.SetBool("isAbandoned", false);
            character.ChangePatientState(PatientCharacter.PatientStates.Wandering); // once the max time for abandonment is reached, patient goes wandering. 
        }
    }
}
