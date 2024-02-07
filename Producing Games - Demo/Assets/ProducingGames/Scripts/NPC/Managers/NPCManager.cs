using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// <para>Moderated By: Matt Brake</para>
/// 
/// Holds a container of positions that each NPC will choose from to move towards
/// during the wandering state, it's a singleton design as we only want one instance
/// of the NPC Wandering Manager.
/// 
/// <para> **Revised Version by MB. Added functionality of Possessive Demons.** </para>
/// </summary>

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;

    [SerializeField] private List<Transform> destinationLocations = new();

    [SerializeField] private List<GameObject> NPCS = new();

    [SerializeField] private List<DemonItemsSO> DemonTypes = new();

    public DemonItemsSO ChosenDemon { get; private set; }

    private void Start()
    {
        AICharacter[] aICharacters = FindObjectsByType<AICharacter>(FindObjectsSortMode.None);
       foreach(AICharacter character in aICharacters)
        {
            NPCS.Add(character.gameObject);
        }

        AssignRandomDemonType();
         
        
    }

    /// <summary>
    /// Randomises a demon for the game instance and a selected NPC to possess. 
    /// </summary>
    public void AssignRandomDemonType()
    {
        int DemonChoice = UnityEngine.Random.Range(0, DemonTypes.Count);
        int NPCChoice = UnityEngine.Random.Range(0, NPCS.Count);

        GameObject ChosenNPC = NPCS[NPCChoice];
        ChosenDemon = DemonTypes[DemonChoice];

        ChosenNPC.GetComponent<AICharacter>().isPossessed = true;
        
        

        Debug.Log(ChosenNPC.name + "Has been possessed by: " + ChosenDemon.DemonName);

    }





    public int GetDestinationLocationsCount() => destinationLocations.Count;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    /// <summary>
    /// Chooses a random location from the destinations list for the NPC to wander towards
    /// </summary>
    /// <returns></returns>
    public Vector3 RandomDestination()
    {
        return destinationLocations[Random.Range(0, destinationLocations.Count)].position;
    }
}
