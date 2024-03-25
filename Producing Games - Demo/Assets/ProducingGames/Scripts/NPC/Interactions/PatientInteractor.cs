using UnityEngine;
using static PlayerVoiceController;

public class PatientInteractor : NPCInteractableTemplate
{
    public SoundEffect[] VoiceLines;

    public override void Escort()
    {
        int rand = Random.Range(0, VoiceLines.Length);
        if (VoiceLines[rand] != null)
        {
            AudioManager.instance.PlaySound(VoiceLines[rand], gameObject.transform);
        }

        character.ChangePatientState(PatientCharacter.PatientStates.Escorted);
    }
}
