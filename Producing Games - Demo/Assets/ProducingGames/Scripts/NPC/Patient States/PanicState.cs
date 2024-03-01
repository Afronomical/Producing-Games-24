using UnityEngine;

/// <summary>
/// Written by: Matej Cincibus
/// Moderated by: ...
/// 
/// Controls the panic state of the patient, this state occurs when the patient
/// is near the demon
/// 
/// References: https://discussions.unity.com/t/randomize-an-enum-list/89491/3
/// </summary>

public class PanicState : PatientStateBaseClass
{
    public enum Choices
    {
        HidingSpot,
        Bedroom
    }

    private Choices currentChoice;
    private bool isCowering = false;
    private Vector3 safetyLocation;
    private float calmingTime;

    private void Start()
    {
        if (character.agent.hasPath)
            character.agent.ResetPath();

        character.agent.speed = character.runSpeed;

        currentChoice = GetRandomEnum<Choices>();

        // INFO: Randomly chooses between going to a random hiding spot or going
        // to their bedroom to hide
        switch (currentChoice)
        {
            case Choices.HidingSpot:
                safetyLocation = NPCManager.Instance.RandomHidingLocation();
                break;
            case Choices.Bedroom:
                safetyLocation = character.bed.transform.position;
                break;
            default:
                Debug.LogWarning("Unable to choose a suitable safety location to go to!");
                break;
        }
    }

    public override void UpdateLogic()
    {
        character.agent.SetDestination(safetyLocation);

        // INFO: Goes into cower state which is where the patient stops
        // moving and goes into a fetal position
        if (character.DistanceFromDemon < character.cowerRadius && !isCowering)
        {
            isCowering = true;
            character.agent.isStopped = true;

            // PLAY COWERING ANIMATION HERE + MAYBE CRYING SOUNDS
        }
        else if (character.DistanceFromDemon > character.cowerRadius && isCowering)
        {
            isCowering = false;
            character.agent.isStopped = false;

            // STOP COWERING ANIMATION + STOP CRYING SOUNDS -> PLAY WALKING ANIMATION
        }

        // INFO: Given that the patient reaches their hiding location and the demon is no longer near them
        // they will then wait for a while before going into another state as they are no longer panicked
        if (Vector3.Distance(character.transform.position, safetyLocation) < character.distanceFromDestination && !isCowering)
        {
            calmingTime += Time.deltaTime;

            if (calmingTime > character.calmingDuration)
            {
                // TEMP FOR NOW, WILL LIKELY BE CHANGED WITH THE INTRODUCTION OF NEW STATES
                character.ChangePatientState(PatientCharacter.PatientStates.Wandering);
            }
        }
    }

    /// <summary>
    /// Returns a random enum member
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private T GetRandomEnum<T>()
    {
        System.Array enumArray = System.Enum.GetValues(typeof(T));
        T randomEnumMember = (T)enumArray.GetValue(Random.Range(0, enumArray.Length));
        return randomEnumMember;
    }
}
