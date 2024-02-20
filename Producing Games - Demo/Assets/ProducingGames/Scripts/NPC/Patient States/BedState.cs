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
        Transform pos = character.bed.transform.Find("PatientPosition");
        transform.position = pos.position;
        transform.rotation = pos.rotation;
        character.rb.velocity = Vector3.zero;
        character.rb.useGravity = false;
        GetComponent<Animator>().SetBool("inBed", true);
        //character.agent.isStopped = true;
        //character.agent.ResetPath();
    }

    private void OnDestroy()
    {
        character.rb.useGravity = true;
    }
}
