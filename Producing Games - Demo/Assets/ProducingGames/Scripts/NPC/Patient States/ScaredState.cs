using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matthew Brake
/// <para> Moderated By: .....</para>
/// Manages the behaviour of the patients before hour 7, when they encounter the demon or any paranormal activity. 
/// <para> The main difference between this and panic state is the lack of the cowering behaviour, and the consideration of paranormal events</para>
/// 
/// </summary>
public class ScaredState : PatientStateBaseClass
{
    //need reference to current horror events happening 
    private float currentEventDistance;
    private Vector3 safetyLocation;
    private bool detectedBed = false;


    public enum Choices
    {
        HidingSpot,
        Bedroom
    }

    private Choices currentChoice;

    private void Start()
    {
        if(character.scaredNPC != null)
        {
            AudioManager.instance.PlaySound(character.scaredNPC,character.transform);
        }

        //set the animation
        character.animator.SetBool("isScared", true);


        if (character.agent.hasPath)
            character.agent.ResetPath();
        character.agent.speed = character.runSpeed;
        currentChoice = GetRandomEnum<Choices>();

        switch (currentChoice)
        {
            case Choices.HidingSpot:
                safetyLocation = NPCManager.Instance.RandomHidingLocation();
                break;
            case Choices.Bedroom:
                safetyLocation = character.bed.transform.position;
                break;
            default:
                break;
        }
    }

    public override void UpdateLogic()
    {

        character.agent.SetDestination(safetyLocation);
        if (CheckBedInRange())
        {
            // INFO: Prevents unnecessary set destination calls
            if (!detectedBed)
            {
                detectedBed = true;
                character.agent.SetDestination(character.BedDestination.position);
            }

           
            transform.LookAt(new Vector3(character.BedDestination.position.x, transform.position.y, character.BedDestination.position.z));

            // INFO: Switches to bed state once patient gets close enough to the bed
            if (character.agent.remainingDistance < 0.1f)
                character.ChangePatientState(PatientCharacter.PatientStates.Bed);
        }

    }
    /// <summary>
    /// Function that handles the detection of the bed belonging to a specific patient
    /// </summary>
    /// <returns></returns>
    bool CheckBedInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(character.transform.position, character.detectionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == character.bed)
                return true;
        }
        return false;
    }


    private T GetRandomEnum<T>()
    {
        System.Array enumArray = System.Enum.GetValues(typeof(T));
        T randomEnumMember = (T)enumArray.GetValue(Random.Range(0, enumArray.Length));
        return randomEnumMember;
    }

   
}
