/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// Handles the logic of the possessed patient
/// </summary>

public class PossessedState : PatientStateBaseClass
{
    private readonly float possessedDuration = 5.0f;

    private void Start()
    {
        DemonCharacter demonCharacter = GameManager.Instance.demon.GetComponent<DemonCharacter>();

        // INFO: Given that the patient isn't the possessed patient or
        // that the demon is already active, we can return
        if (!character.isPossessed || demonCharacter.currentState != DemonCharacter.DemonStates.Inactive
                                   || demonCharacter.currentState != DemonCharacter.DemonStates.Exorcised)
            return;


        // INFO: Makes the demon start patrolling and spawns it at the possessed patients bed
        demonCharacter.ChangeDemonState(DemonCharacter.DemonStates.Patrol);
        demonCharacter.agent.Warp(character.bed.transform.position);

        // PLAY POSSESSED ANIMATION HERE

        Invoke(nameof(LeavePossessedState), possessedDuration);
    }

    private void LeavePossessedState()
    {
        // TEMPORARY MAY BE CHANGED BASED ON SPECIFICATION
        character.ChangePatientState(PatientCharacter.PatientStates.Wandering);
    }
}
