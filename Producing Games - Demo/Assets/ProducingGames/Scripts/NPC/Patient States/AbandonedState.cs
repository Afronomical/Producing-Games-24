using UnityEngine;

/// <summary>
/// Written By: Matt Brake
/// <para> Moderated By: Matej Cincibus</para>
/// <para> Clarifies the behaviour of the NPC when abandoned by player.</para>
/// </summary>

public class AbandonedState : PatientStateBaseClass
{
    private readonly float maxTimeAbandoned = 5f;
    private float timeAbandoned = 0;

    private RaycastToPlayer raycastToPlayer;


    //added boolean to check for abandoned state
    bool isAbandoned;

    private void Awake()
    {
        GetComponent<AICharacter>().isMoving = false; //flags character as not moving

        if (raycastToPlayer == null)
            raycastToPlayer = GetComponent<RaycastToPlayer>();

        
    }

    public override void UpdateLogic()
    {
        timeAbandoned += Time.deltaTime; //logs the time they NPC has been abandoned for.
        isAbandoned = true;

        if (timeAbandoned > maxTimeAbandoned)
        {
            character.ChangePatientState(PatientCharacter.PatientStates.Wandering); // once the max time for abandonment is reached, NPC goes wandering. 
            isAbandoned = false;
        }

        GetComponent<Animator>().SetBool("isAbandoned", isAbandoned);
    }
}
