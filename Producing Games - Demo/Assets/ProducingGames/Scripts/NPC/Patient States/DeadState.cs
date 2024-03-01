using UnityEngine;

/// <summary>
/// Written By: Matt Brake 
/// <para> Moderated By: Matej Cincibus</para>
/// <para> Manages the behaviour of AI when they die. </para>
/// </summary>

public class DeadState : PatientStateBaseClass
{
    private void Start()
    {
        if (character.agent.hasPath)
            character.agent.ResetPath();

        character.animator.SetBool("isDead", true);

        GameManager.Instance.DecrementRemainingPatients();
    }

    /*public override void UpdateLogic()
    {
    }*/
}
