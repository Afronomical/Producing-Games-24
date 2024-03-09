/// <summary>
/// Written By: Matt Brake
/// <para> Moderated By: Matej Cincibus</para>
/// <para> Clarifies the behaviour of the Patient when abandoned by player.</para>
/// </summary>

public class AbandonedState : PatientStateBaseClass
{
    private void Start()
    {
        // PLAY ABANDONED ANIMATION HERE

        Invoke(nameof(Abandoned), character.abandonedDuration);
    }

    private void Abandoned()
    {
        character.animator.SetBool("isAbandoned", false);

        // INFO: Patient goes back to wandering once completely abandoned
        character.ChangePatientState(PatientCharacter.PatientStates.Wandering); 
    }
}
