using UnityEngine;

/// <summary>
/// Written by: Matej Cincibus
/// Moderated by: ...
/// 
/// This class handles the dice interactable object
/// </summary>

public class DiceInteractable : InteractableTemplate, IConsumable
{
    [Header("Dice Settings")]
    [Tooltip("Any remaining amount correlates to an unsuccessful outcome")]
    [Range(0, 100)] [SerializeField] private float successfulOutcomeChance;
    [Min(1)] [SerializeField] private int numOfDice = 6;

    [Header("Negative Effect Settings")]
    [Tooltip("How much the player is slowed down when they get the bad outcome")]
    [Range(0, 1)] [SerializeField] private float slowedDownPercentage = 0.5f;

    private PlayerMovement playerMovement;
    private DemonCharacter demonCharacter;

    public void Consume()
    {
        // INFO: Prevents the use of the dice if the demon hasn't spawned in yet or is inactive
        if (GameManager.Instance.demon == null)
        {
            Debug.LogWarning("Cannot use dice as demon has not spawned in yet!");
            return;
        }
        else
        {
            // INFO: Grabs a reference to it only once
            if (demonCharacter == null)
                demonCharacter = GameManager.Instance.demon.GetComponent<DemonCharacter>();

            if (demonCharacter.currentState == DemonCharacter.DemonStates.Inactive)
            {
                Debug.LogWarning("Cannot use dice as demon is inactive!");
                return;
            }
        }

        // PLAY DICE ANIMATION/SOUND (?)

        InventoryHotbar.instance.RemoveFromInventory(collectible);

        // INFO: Roll the dice and get a number back
        int diceRoll = Random.Range(1, numOfDice + 1);
        float dicePercentage = (float)diceRoll / (float)numOfDice * 100.0f;

        // AFTER ANIMATION PLAYS DO WHATS BELOW

        Debug.Log("DP: " + dicePercentage);
        Debug.Log("SOC: " + successfulOutcomeChance);

        if (dicePercentage < successfulOutcomeChance)
        {
            Debug.Log("Rolled a: " + diceRoll + ". Successful outcome!");
            // STOP HUNTING MUSIC (?)

            // INFO: Teleports demon to a random location on the map and sets them to patrolling
            demonCharacter.agent.Warp(NPCManager.Instance.RandomPatrolDestination());
            demonCharacter.ChangeDemonState(DemonCharacter.DemonStates.Patrol);
        }
        else
        {
            playerMovement = GameManager.Instance.player.GetComponent<PlayerMovement>();

            Debug.Log("Rolled a: " + diceRoll + ". Unsuccessful outcome!");

            // PLAY HEART BEAT SOUND

            playerMovement.slowedEffect = true;
            playerMovement.walkSpeed *= slowedDownPercentage;
            playerMovement.sprintSpeed *= slowedDownPercentage;
        }

        Destroy(gameObject);
    }

    public override void Interact()
    {
        Debug.Log("***Player has picked up dice***");
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
