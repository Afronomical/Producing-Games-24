using UnityEngine;
using UnityEngine.AI;

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
    private bool isCowering = false;
    private Vector3 safetyLocation;
    private float calmingTime;

    private void Start()
    {
        character.agent.speed = character.runSpeed;

        // INFO: Plays the desired male voice line if the patient is a male
        // otherwise plays the desirted female voice line
        switch(character.patientName)
        {
            case PatientNames.MartinaBlanco:
                NPCManager.Instance.PlayMartinaVoiceLine(NPCManager.MartinaVoiceLines.FemOneScreamOne, transform);
                break;
            case PatientNames.CarmenIglesias:
                NPCManager.Instance.PlayCarmenVoiceLine(NPCManager.CarmenVoiceLines.FemTwoScream, transform);
                break;
            case PatientNames.DiegoSanz:
                NPCManager.Instance.PlayDiegoVoiceLine(NPCManager.DiegoVoiceLines.ManOneScream, transform);
                break;
            case PatientNames.LucianoGarcia:
                NPCManager.Instance.PlayLucianoVoiceLine(NPCManager.LucianoVoiceLines.ManTwoScreamOne, transform);
                break;

        }

        if (character.isMale)
        {
           // NPCManager.Instance.PlayMaleVoiceLine(NPCManager.MaleVoiceLines.ManOneScream, transform);
        }
        else
        {
           // NPCManager.Instance.PlayFemaleVoiceLine(NPCManager.FemaleVoiceLines.FemOneScreamOne, transform);
        }

        // INFO: If the previous patient state was the bed state, we don't
        // want the safety choice to be their bed, as they wouldn't go anywhere
        // so we need to choose the other (hiding location)
        if (character.PreviousState == PatientCharacter.PatientStates.Bed)
        {
            // INFO: Teleports to closest point of navmesh as beds aren't on the navmesh
            character.NearestNavMeshPoint();
            character.safetyChoice = SafetyChoices.HidingSpot;
        }
        else
            character.safetyChoice = character.GetRandomEnum<SafetyChoices>();

        // INFO: Randomly chooses a safety location based on the chosen
        // enum member
        safetyLocation = character.SafetyChooser();

        character.agent.SetDestination(safetyLocation);
    }

    public override void UpdateLogic()
    {
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
            character.animator.SetBool("isRunning", true);
        }

        character.animator.SetBool("isTerrified", isCowering);

        // INFO: Given that the patient reaches their hiding location and the demon is no longer near them
        // they will then wait for a while before going into another state as they are no longer panicked
        if (character.agent.remainingDistance < 0.1f && !isCowering)
        {
            calmingTime += Time.deltaTime;

            if (calmingTime > character.calmingDuration)
            {
                // INFO: Returns to the previous state before the patient became scared, to
                // ensure the task system does not break
                character.animator.SetBool("isRunning", false);
                character.animator.SetBool("isTerrified", true);
                character.ChangePatientState(character.PreviousState);
            }
        }
    }
}
