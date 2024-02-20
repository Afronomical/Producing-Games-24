/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// </summary>

public class PossessedState : PatientStateBaseClass
{
    private void Start()
    {
        // INFO: Extra security check
        if (character.isPossessed)
            GameManager.Instance.demon.GetComponent<DemonCharacter>().ChangeDemonState(DemonCharacter.DemonStates.Patrol);
    }

    /*public override void UpdateLogic()
    {
    }*/
}
