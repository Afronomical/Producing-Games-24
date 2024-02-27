using UnityEngine;

public class PatientInteractor : NPCInteractableTemplate
{
    public override void Escort()
    {
        character.ChangePatientState(PatientCharacter.PatientStates.Escorted);
    }
}
