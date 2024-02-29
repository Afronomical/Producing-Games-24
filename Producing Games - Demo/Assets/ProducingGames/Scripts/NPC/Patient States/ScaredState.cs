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
    public SoundEffect NPCScared;
    private float currentEventDistance;
    private Vector3 safetyLocation;
    private Transform closestEvent;

    public enum Choices
    {
        HidingSpot,
        Bedroom
    }

    private Choices currentChoice;

    private void Start()
    {
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
        currentEventDistance = Vector3.Distance(character.transform.position, closestEvent.position);

        if(currentEventDistance < character.detectionRadius)
        {
            if(NPCScared)
            {
                AudioManager.instance.PlaySound(NPCScared, character.transform);
            }
             
        }
        else if(character.DistanceFromDemon < character.detectionRadius)
        {

        }
    }

    private T GetRandomEnum<T>()
    {
        System.Array enumArray = System.Enum.GetValues(typeof(T));
        T randomEnumMember = (T)enumArray.GetValue(Random.Range(0, enumArray.Length));
        return randomEnumMember;
    }
}
