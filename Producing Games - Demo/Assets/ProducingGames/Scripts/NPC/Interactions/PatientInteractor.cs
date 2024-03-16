using UnityEngine;

public class PatientInteractor : NPCInteractableTemplate
{
    public SoundEffect VoiceLines;

    public override void Escort()
    {
        AudioManager.instance.PlaySound(VoiceLines, gameObject.transform);

        character.ChangePatientState(PatientCharacter.PatientStates.Escorted);
    }
}
