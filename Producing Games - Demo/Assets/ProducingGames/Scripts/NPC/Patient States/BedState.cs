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
        character.agent.enabled = false;
        character.rb.velocity = Vector3.zero;
        character.rb.useGravity = false;

        Transform pos = character.bed.transform.Find("PatientPosition");
        transform.SetPositionAndRotation(pos.position, pos.rotation);

        character.animator.SetBool("inBed", true);
    }
}
