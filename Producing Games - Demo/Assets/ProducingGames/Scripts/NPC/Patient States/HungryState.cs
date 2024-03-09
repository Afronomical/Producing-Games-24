using UnityEngine;

/// <summary>
/// Written By: Matt Brake
/// <para> Moderated By: Matej Cincibus </para>
/// <para> Determines functionality for Patients when they are hungry</para>
/// </summary>

public class HungryState : PatientStateBaseClass
{
    private Vector3 hungryLocation;
    private bool isWalkingToHungryDest = false;

    private void Start()
    {
        ChooseKitchenDestination();

        // INFO: Given that the previous was panicked or scared we will have
        // the patient walk back to their hungry spot
        if (character.PreviousState == PatientCharacter.PatientStates.Panic ||
            character.PreviousState == PatientCharacter.PatientStates.Scared)
            WalkToHungrySpot();
        else
            TeleportToHungrySpot();
    }

    public override void UpdateLogic()
    {
        if (isWalkingToHungryDest)
        {
            // INFO: Stops the walking animation and transitions to the praying animation
            if (character.agent.remainingDistance < 0.1f)
            {
                isWalkingToHungryDest = false;

                // STOP PLAYING WALKING ANIMATION

                character.animator.SetBool("isHungry", true);
            }
        }
    }

    /// <summary>
    /// Chooses a kitchen location from an available list of kitchen locations held in the NPC manager
    /// </summary>
    public void ChooseKitchenDestination()
    {
        // INFO: If there are no kitchen locations in the list then end
        if (NPCManager.Instance.GetKitchenLocationsCount() == 0)
        {
            Debug.LogError("There are no kitchen locations setup in the kitchen location list.");
            return;
        }

        // INFO: Chooses a location to be hungry at
        hungryLocation = NPCManager.Instance.RandomKitchenPosition();
    }

    /// <summary>
    /// Rather than having the patient walk to their hiding spot, we
    /// will have them teleport to it
    /// </summary>
    private void TeleportToHungrySpot()
    {
        character.agent.Warp(hungryLocation);

        character.animator.SetBool("isHungry", true);
    }

    /// <summary>
    /// Rather than teleporting the patient, this will have the patient
    /// walk back to their hiding spot
    /// </summary>
    private void WalkToHungrySpot()
    {
        // PLAY WALKING ANIMATION HERE

        isWalkingToHungryDest = true;

        character.agent.speed = character.walkSpeed;
        character.agent.SetDestination(hungryLocation);
    }

    /// <summary>
    /// When the script is destroyed (changes state) it will free up the kitchen
    /// spot location ready for when the next hungry task is set for a patient
    /// </summary>
    private void OnDestroy()
    {
        NPCManager.Instance.SetHidingLocationFree(hungryLocation);
    }

    /*
    private Vector3 destinationPos;
    private readonly float distanceFromFood;

    private float currentIdleTime = 0f;
    private readonly float maxIdleTime = 3f; 

    private void Start()
    {
        //character.animator.SetBool("isHungry", true);

        if (character.agent.hasPath)
            character.agent.ResetPath();

        ChooseKitchenDestination();
    }

    public override void UpdateLogic()
    {
        character.animator.SetBool("isHungry", true);

        character.agent.SetDestination(destinationPos);

        if(CheckDistanceToLocation())
        {
             currentIdleTime += Time.deltaTime;

            if(currentIdleTime >= maxIdleTime)
            {
                ChooseKitchenDestination(); 
            }
        }

    }

    /// <summary>
    /// Checks the distance from character to the destination and stops if within range
    /// </summary>
    /// <returns></returns>
    public bool CheckDistanceToLocation()
    {
        if (Vector3.Distance(character.transform.position, destinationPos) < distanceFromFood)
            return true;
        return false;
    }

    public void Eat()
    {
        // whatever specs we get next for this state 
    }

    public void ChooseKitchenDestination()
    {
        // INFO: If there are no kitchen locations in the list then end
        if (NPCManager.Instance.GetKitchenLocationsCount() == 0)
        {
            Debug.LogWarning("There are no kitchen locations setup in the hiding location list.");
            return;
        }

        destinationPos = NPCManager.Instance.RandomKitchenPosition();
    }
    */
}
