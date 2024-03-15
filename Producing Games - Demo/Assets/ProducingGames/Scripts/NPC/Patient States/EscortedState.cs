using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

/// <summary>
/// <para>Written By: Matt Brake</para>
/// Moderated By: Matej Cincibus
/// 
///<para>Tracks the behaviour of AI when being escorted back to their room.</para>
///<para>Currently will just move towards player transform, until pathfinding is properly in place.</para>
/// </summary>

public class EscortedState : PatientStateBaseClass
{
    private Vector3 playerPos;
    private Vector3 lastPlayerPos;

    private bool shouldFollow = true;
    private bool beenPickedUp = false;

    private bool detectedBed = false;
    private bool lostPlayer = true;

    private float escortedAloneTime = 0;

    private void Start()
    {
        character.agent.speed = character.runSpeed;

        // INFO: Find player position as soon as the state is entered
        // so that patients don't get stuck in the escorted state
        playerPos = character.player.transform.position;
        character.agent.SetDestination(playerPos);

        // INFO: Plays the desired male voice line if the patient is a male
        // otherwise plays the desirted female voice line
        if (character.isMale)
        {
            NPCManager.Instance.PlayMaleVoiceLine(NPCManager.MaleVoiceLines.Scream, transform);
        }
        else
        {
            NPCManager.Instance.PlayFemaleVoiceLine(NPCManager.FemaleVoiceLines.Scream, transform);
        }
    }

    public override void UpdateLogic()
    {
        character.animator.SetFloat("movement", character.agent.velocity.magnitude);

        playerPos = character.player.transform.position;

        // INFO: Player is detected and following player function is called
        if (character.raycastToPlayer.PlayerDetected())
        {
            if (escortedAloneTime != 0)
                escortedAloneTime = 0.0f;

            beenPickedUp = true;
            lostPlayer = false;
            lastPlayerPos = character.player.transform.position;

            MoveTowardsPlayer();
        }
        // INFO: If patient previously picked up but currently not detecting player 
        else if (beenPickedUp && !character.raycastToPlayer.PlayerDetected())
        {
            // INFO: Patient goes to the last known player location
            // Prevents running set destination call multiple times
            if (!lostPlayer)
            {
                lostPlayer = true;
                character.agent.SetDestination(lastPlayerPos);
            }

            // INFO: Given that the patient has arrived at their destination, start a countdown for the abandoned transition
            if ((transform.position - lastPlayerPos).sqrMagnitude < character.distanceFromPlayer)
            {
                escortedAloneTime += Time.deltaTime;
                character.animator.SetBool("isAbandoned", true);

                // INFO: Gives player n seconds to re-collect patient before they enter abandoned state
                if (escortedAloneTime > character.aloneEscortedDuration)
                {
                    escortedAloneTime = 0.0f;
                    character.ChangePatientState(PatientCharacter.PatientStates.Abandoned);
                }
            }
        }

        if (beenPickedUp)
        {
            // INFO: Checks whether bed is in range and goes to it if it is
            if (character.CheckBedInRange())
            {
                // INFO: Prevents unnecessary set destination calls
                if (!detectedBed)
                {
                    detectedBed = true;
                    character.agent.SetDestination(character.BedDestination.position);
                }

                shouldFollow = false;

                // INFO: Switches to bed state once patient gets close enough to the bed
                if (character.agent.remainingDistance < 0.1f)
                    character.ChangePatientState(PatientCharacter.PatientStates.Bed);
            }
        }
    }

    /// <summary>
    /// <para>Primary Function in escorting state.</para>
    /// <para>Checks if player is within range and follows them if true.</para>
    /// </summary>
    private void MoveTowardsPlayer()
    {
        // INFO: Ensures the patient only rotates on the y-axis
        Vector3 playerPosition = new(playerPos.x, transform.position.y, playerPos.z);

        transform.LookAt(playerPosition);

        if (shouldFollow)
            character.agent.SetDestination(playerPos);

        // INFO: Stops patient from moving closer to the player
        if (character.raycastToPlayer.playerDistance < character.distanceFromPlayer)
        {

            shouldFollow = false;
            character.rb.velocity = Vector3.zero;
            character.agent.ResetPath();
            if(character.raycastToPlayer.playerDistance < character.distanceFromPlayer /2)
            {
                float moveDistance = character.distanceFromPlayer - character.raycastToPlayer.playerDistance;
                BackAwayFromPlayer(moveDistance);
            }
            

        }
        
        else
            shouldFollow = true;
    }

    private void BackAwayFromPlayer(float moveDistance)
    {
        Vector3 moveDirection = transform.position - playerPos;

        moveDirection.Normalize();
        moveDirection *= moveDistance;

        character.agent.SetDestination(transform.position + moveDirection);
    }
}
