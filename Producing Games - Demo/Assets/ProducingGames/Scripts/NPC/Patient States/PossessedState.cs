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
        {
            GameManager.Instance.demon.GetComponent<DemonCharacter>().ChangeDemonState(DemonCharacter.DemonStates.Patrol);
            GameManager.Instance.demon.transform.position = character.bed.transform.position;
        }
    }

    /*public override void UpdateLogic()
    {
    }*/
}
