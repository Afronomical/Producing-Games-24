/// <summary>
/// Written By: Matt Brake 
/// <para> Moderated By: Matej Cincibus</para>
/// <para> Manages the behaviour of AI when they die. </para>
/// </summary>

public class DeadState : PatientStateBaseClass
{
    private void Start()
    {
        character.animator.SetBool("isDead", true);
        GameManager.Instance.DecrementRemainingPatients();
    }
}
