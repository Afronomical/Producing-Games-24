using UnityEngine;

/// <summary>
/// Written By: Matt Brake 
/// <para> Moderated By: Matej Cincibus</para>
/// <para> Manages the behaviour of AI when laying in bed. </para>
/// </summary>

public class BedState : PatientStateBaseClass
{
    private void Start()
    {
        // INFO: Prevents the patient from moving
        character.agent.enabled = false;
        character.rb.velocity = Vector3.zero;
        character.rb.useGravity = false;

        transform.SetPositionAndRotation(character.BedDestination.position, character.BedDestination.rotation);

        character.animator.SetBool("inBed", true);
    }
}
