using UnityEngine;

/// <summary>
/// Written By: Matt Brake 
/// <para> Moderated By: Matej Cincibus</para>
/// <para> Manages the behaviour of AI when laying in bed. </para>
/// </summary>

public class BedState : PatientStateBaseClass
{
    private bool isWalkingToBed = false;

    private void Start()
    {
        // INFO: Given that the previous was panicked or scared we will have
        // the patient walk back to their bed  
        if(character.PreviousState == PatientCharacter.PatientStates.Bed)
            PutInBed();
        else
            WalkToBed();
    }

    public override void UpdateLogic()
    {
        if (isWalkingToBed)
        {
            // INFO: Puts the patient into bed once they get close enough to it
            if (character.agent.remainingDistance < 0.1f)
            {
                isWalkingToBed = false;
                PutInBed();
            }
        }
    }

    /// <summary>
    /// Function that handles putting the patient into the actual bed
    /// </summary>
    private void PutInBed()
    {
        // STOP PLAYING WALKING ANIMATION HERE

        // INFO: Prevents the patient from moving
        character.agent.enabled = false;
        character.rb.velocity = Vector3.zero;
        character.rb.useGravity = false;

        transform.SetPositionAndRotation(character.BedDestination.position, character.BedDestination.rotation);

        character.animator.SetBool("inBed", true);
    }

    /// <summary>
    /// Function used when the patients previous state was either panicked/scared
    /// This is done so that the patient doesn't just teleport to their bed once
    /// they leave the panicked/scared state
    /// </summary>
    private void WalkToBed()
    {
        // PLAY WALKING ANIMATION HERE

        isWalkingToBed = true;

        character.agent.speed = character.walkSpeed;
        character.agent.SetDestination(character.BedDestination.position);
    }
}
