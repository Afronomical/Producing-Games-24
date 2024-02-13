using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// <para>Moderated By: Matt Brake</para>
/// 
/// Manages and keeps track of the NPCs in the scene. Provides the NPCs with set
/// locations to walk towards. Holds containers for all positions needed, taken or free. 
/// 
/// <para> **Revised Version by MB. Added functionality of Possessive Demons.** </para>
/// </summary>

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;

    [SerializeField] private List<Transform> wanderingDestinations = new();
    [SerializeField] private List<Transform> hidingLocations = new();
    [SerializeField] private List<GameObject> npcList = new();
    [SerializeField] private List<DemonItemsSO> demonTypes = new();
    [SerializeField] private List<Transform> prayingLocations = new();
    [SerializeField] private List<GameObject> npcBeds = new();

    private Dictionary<Vector3, bool> wanderingLib = new();
    private Dictionary<Vector3, bool> hidingLib = new();

    public DemonItemsSO ChosenDemon { get; private set; }
    public int GetWanderingDestinationsCount() => wanderingDestinations.Count;
    public int GetHidingLocationsCount() => hidingLocations.Count;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;


        AICharacter[] aICharacters = FindObjectsByType<AICharacter>(FindObjectsSortMode.None);
        foreach (AICharacter character in aICharacters)
        {
            npcList.Add(character.gameObject);
        }

        AssignRandomDemonType();

        // INFO: Add all wandering vector3 positions to the dictionary and initialise
        // their value as false (which states that the location hasn't been taken yet)
        foreach (Transform transform in wanderingDestinations)
        {
            wanderingLib.Add(transform.position, false);
        }

        // INFO: Do the same for hiding locations
        foreach (Transform transform in hidingLocations)
        {
            hidingLib.Add(transform.position, false);
        }
    }

    /// <summary>
    /// Randomises a demon for the game instance and a selected NPC to possess. 
    /// </summary>
    public void AssignRandomDemonType()
    {
        int demonChoice = Random.Range(0, demonTypes.Count);
        int npcChoice = Random.Range(0, npcList.Count);

        GameObject chosenNPC = npcList[npcChoice];
        ChosenDemon = demonTypes[demonChoice];

        chosenNPC.GetComponent<AICharacter>().isPossessed = true;

        Debug.Log(chosenNPC.name + "Has been possessed by: " + ChosenDemon.DemonName);
    }

    /// <summary>
    /// Chooses a random location from the destinations list for the NPC to wander towards
    /// </summary>
    /// <returns></returns>
    public Vector3 RandomWanderingDestination()
    {
        Vector3 wanderingDestination = FindSuitableLocation(wanderingDestinations, AvailableLocations(wanderingLib), wanderingLib);

        return wanderingDestination;
    }

    /// <summary>
    /// Chooses a random location from the destinations list for the NPC to wander towards
    /// </summary>
    /// <returns></returns>
    public Vector3 RandomHidingLocation()
    {
        Vector3 hidingLocation = FindSuitableLocation(hidingLocations, AvailableLocations(hidingLib), hidingLib);

        return hidingLocation;
    }

    public void SetWanderingDestinationFree(Vector3 wanderingDestination)
    {
        if (wanderingLib.ContainsKey(wanderingDestination))
            wanderingLib[wanderingDestination] = false;
    }

    public void SetHidingLocationFree(Vector3 hidingLocation)
    {
        if (hidingLib.ContainsKey(hidingLocation))
            hidingLib[hidingLocation] = false;
    }

    private int AvailableLocations(Dictionary<Vector3, bool> locationLib)
    {
        int availableLocations = 0;

        foreach (bool key in locationLib.Values)
        {
            if (!key)
                availableLocations++;
        }

        return availableLocations;
    }

    private Vector3 FindSuitableLocation(List<Transform> locationsList, int availableLocations, Dictionary<Vector3, bool> locationLib)
    {
        Vector3 chosenLocation;

        // INFO: Continues to find a different hiding location that hasn't been taken yet
        do
        {
            chosenLocation = locationsList[Random.Range(0, locationsList.Count)].position;

            if (availableLocations == 0)
            {
                Debug.LogWarning("There are " + availableLocations + " free locations for patients to go to.\nChoosing an already taken location.");
                break;
            }

        } while (locationLib[chosenLocation]);

        // INFO: Changes the state of the location as taken
        locationLib[chosenLocation] = true;

        return chosenLocation;
    }

    public Vector3 RandomPrayingDestination()
    {
        return prayingLocations[Random.Range(0, prayingLocations.Count)].position;  
    }
}
