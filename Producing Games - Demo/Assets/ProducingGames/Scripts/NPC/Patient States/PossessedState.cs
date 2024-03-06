/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// Handles the logic of the possessed patient
/// </summary>

public class PossessedState : PatientStateBaseClass
{
    private void Start()
    {
        // INFO: Extra security check
        if (character.isPossessed)
        {
            DemonCharacter demonCharacter = GameManager.Instance.demon.GetComponent<DemonCharacter>();
            
            // INFO: Makes the demon start patrolling and spawns it at the possessed patients bed
            demonCharacter.ChangeDemonState(DemonCharacter.DemonStates.Patrol);
            demonCharacter.agent.Warp(character.bed.transform.position);
        }
    }
}
