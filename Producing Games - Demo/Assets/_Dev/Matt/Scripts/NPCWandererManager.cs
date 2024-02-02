using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// Holds a container of positions that each NPC will choose from to move towards
/// during the wandering state.
/// </summary>
public class NPCWandererManager : MonoBehaviour
{
    public static NPCWandererManager Instance;

    [SerializeField] private List<Transform> destinationLocations = new();

    public int GetDestinationLocationsCount() => destinationLocations.Count;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public Vector3 RandomDestination()
    {
        return destinationLocations[Random.Range(0, destinationLocations.Count)].position;
    }
}
