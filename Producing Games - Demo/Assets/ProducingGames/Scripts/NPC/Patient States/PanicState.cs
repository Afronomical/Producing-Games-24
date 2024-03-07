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
    private bool isCowering = false;
    private Vector3 safetyLocation;
    private float calmingTime;

    private void Start()
    {
        character.agent.speed = character.runSpeed;

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
                // TEMP FOR NOW, WILL LIKELY BE CHANGED WITH THE INTRODUCTION OF NEW STATES
                character.ChangePatientState(PatientCharacter.PatientStates.Wandering);
            }
        }
    }
}
