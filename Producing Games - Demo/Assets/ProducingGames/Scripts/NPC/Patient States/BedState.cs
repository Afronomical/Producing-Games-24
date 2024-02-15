using UnityEngine;

/// <summary>
/// Written By: Matt Brake 
/// <para> Moderated By: Matej Cincibus</para>
/// <para> Manages the behaviour of AI when laying in bed. </para>
/// </summary>

public class BedState : PatientStateBaseClass
{
    private int wanderingChance;
    private int heartAttackChance;

    private void Awake()
    {
        GetComponent<AICharacter>().isMoving = false;
    }

    private void Start()
    {
        character.rb.velocity = Vector3.zero;
        //character.agent.isStopped = true;
        //character.agent.ResetPath();
    }

    public override void UpdateLogic()
    {
        //Debug.Log("Player in Bed State"); 
        ////fix character transform to bed pos. 
    }
}
