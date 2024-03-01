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

    [Header("Patient Locations:")]
    [SerializeField] private List<Transform> wanderingDestinations = new();
    [SerializeField] private List<Transform> hidingLocations = new();
    [SerializeField] private List<Transform> prayingLocations = new();
    [SerializeField] private List<Transform> kitchenLocations = new();

    [Header("Demon Locations:")]
    [SerializeField] private Transform demonInstantiationLocation;
    [SerializeField] private List<Transform> patrolDestinations = new();

    [Header("Miscellaneous:")]
    [SerializeField] private List<DemonItemsSO> demonTypes = new();
    [HideInInspector] public readonly List<GameObject> patientList = new();
    [HideInInspector] public readonly List<GameObject> patientBeds = new();


    // INFO: The key represents the location that the NPC should move to
    // the value represents whether the location has been taken by an NPC
    private readonly Dictionary<Vector3, bool> wanderingLib = new();
    private readonly Dictionary<Vector3, bool> hidingLib = new();
    private readonly Dictionary<Vector3, bool> prayingLib = new();
    private readonly Dictionary<Vector3, bool> hungryLib = new();

    public DemonItemsSO ChosenDemon { get; private set; }
    public int GetWanderingDestinationsCount() => wanderingDestinations.Count;
    public int GetHidingLocationsCount() => hidingLocations.Count;
    public int GetPatrolDestinationsCount() => patrolDestinations.Count;
    public int GetKitchenLocationsCount() => kitchenLocations.Count;
    public int GetPrayerLocationsCount() => prayingLocations.Count;
    public Transform GetDemonInstantionLocation() => demonInstantiationLocation;


    public SoundEffect heartAttackSound;



    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        // INFO: Get all patients in the scene and store them in the patients list
        PatientCharacter[] aICharacters = FindObjectsByType<PatientCharacter>(FindObjectsSortMode.None);
        foreach (PatientCharacter character in aICharacters)
            patientList.Add(character.gameObject);

        // INFO: Get all beds in the scene and store them in the beds list
        GameObject[] beds = GameObject.FindGameObjectsWithTag("Bed");
        foreach (var item in beds)
            patientBeds.Add(item);

        // INFO: Add all vector3 positions to the dictionary and initialise
        // their value as false (which states that the location hasn't been taken yet)
        foreach (Transform transform in wanderingDestinations)
            wanderingLib.Add(transform.position, false);

        foreach (Transform transform in hidingLocations)
            hidingLib.Add(transform.position, false);

        foreach (Transform transform in prayingLocations)
            prayingLib.Add(transform.position, false);

        foreach (Transform transform in kitchenLocations)
            hungryLib.Add(transform.position, false);

        AssignBeds();
        AssignRandomDemonType();
    }

    /// <summary>
    /// Assigns the available beds randomly to each patient
    /// </summary>
    private void AssignBeds()
    {
        foreach (GameObject Patient in patientList)
        {
            int bedChoice = Random.Range(0, patientBeds.Count);
            GameObject chosenBed = patientBeds[bedChoice];
            Patient.GetComponent<PatientCharacter>().bed = chosenBed;
            Debug.Log("Set " + Patient.name + " to bed number: " + chosenBed);
            patientBeds.Remove(chosenBed);
        }
    }

    /// <summary>
    /// Randomises a demon for the game instance and a selected NPC to possess
    /// </summary>
    public void AssignRandomDemonType()
    {
        int demonChoice = Random.Range(0, demonTypes.Count);
        int npcChoice = Random.Range(0, patientList.Count);

        GameObject chosenNPC = patientList[npcChoice];
        ChosenDemon = demonTypes[demonChoice];

        chosenNPC.GetComponent<PatientCharacter>().isPossessed = true;

        Debug.Log(chosenNPC.name + " has been possessed by: " + ChosenDemon.demonName);
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

    /// <summary>
    /// Returns a random position for the patient to go to pray at. 
    /// </summary>
    /// <returns></returns>
    public Vector3 RandomPrayingDestination()
    {
        Vector3 prayerDestination = FindSuitableLocation(prayingLocations, AvailableLocations(prayingLib), prayingLib);
        return prayerDestination;
    }

    /// <summary>
    /// Returns a random position for the patient to go to eat at. 
    /// </summary>
    /// <returns></returns>
    public Vector3 RandomKitchenPosition()
    {
        Vector3 kitchenPos = FindSuitableLocation(kitchenLocations, AvailableLocations(hungryLib), hungryLib);
        return kitchenPos;
    }

    /// <summary>
    /// Returns a random position for the demon to go to
    /// </summary>
    /// <returns></returns>
    public Vector3 RandomPatrolDestination()
    {
        return patrolDestinations[Random.Range(0, patrolDestinations.Count)].position;
    }

    /// <summary>
    /// Sets the specified value (bool) of the passed in key (Vector3) to false, signifying
    /// that it can be accessed again by other NPCs
    /// </summary>
    /// <param name="wanderingDestination"></param>
    public void SetWanderingDestinationFree(Vector3 wanderingDestination)
    {
        if (wanderingLib.ContainsKey(wanderingDestination))
            wanderingLib[wanderingDestination] = false;
    }

    /// <summary>
    /// Sets the specified value (bool) of the passed in key (Vector3) to false, signifying
    /// that it can be accessed again by other NPCs
    /// </summary>
    /// <param name="kitchenDestination"></param>
    public void SetKitchenDestinationFree(Vector3 kitchenDestination)
    {
        if (hungryLib.ContainsKey(kitchenDestination))
            hungryLib[kitchenDestination] = false;
    }

    /// <summary>
    /// Sets the specified value (bool) of the passed in key (Vector3) to false, signifying
    /// that it can be accessed again by other NPCs
    /// </summary>
    /// <param name="prayingDestination"></param>
    public void SetPrayingDestinationFree(Vector3 prayingDestination)
    {
        if (prayingLib.ContainsKey(prayingDestination))
            prayingLib[prayingDestination] = false;
    }

    /// <summary>
    /// Sets the specified value (bool) of the passed in key (Vector3) to false, signifying
    /// that it can be accessed again by other NPCs
    /// </summary>
    /// <param name="hidingLocation"></param>
    public void SetHidingLocationFree(Vector3 hidingLocation)
    {
        if (hidingLib.ContainsKey(hidingLocation))
            hidingLib[hidingLocation] = false;
    }

    /// <summary>
    /// Checks the number of available locations that have not yet been taken by an NPC
    /// </summary>
    /// <param name="locationLib"></param>
    /// <returns></returns>
    private int AvailableLocations(Dictionary<Vector3, bool> locationLib)
    {
        int availableLocations = 0;

        // INFO: Given that the value (bool) is false (not taken) we increment 
        // the number of available locations
        foreach (bool key in locationLib.Values)
        {
            if (!key)
                availableLocations++;
        }
        return availableLocations;
    }

    /// <summary>
    /// Finds a suitable location that has not been claimed by an NPC yet. If it's unable to find
    /// a suitable location it will choose an already taken location.
    /// </summary>
    /// <param name="locationsList"></param>
    /// <param name="availableLocations">The number of locations that are still available</param>
    /// <param name="locationLib"></param>
    /// <returns></returns>
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

}
