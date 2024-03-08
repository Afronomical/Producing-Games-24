using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para> Written By: Matthew Brake  </para>
/// Moderated By: Matej Cincibus
/// <para>This Class is responsible entirely for spawning interactable objects in random pre-set locations across the level.</para> 
/// Pass in the transform of the spawn location empty game object into the spawn location parameters.
/// </summary>

public class InteractableSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct ObjectSpawn
    {
        public GameObject objectToSpawn;
        public int amount;
        public List<Transform> Locations;
    }

    public List<ObjectSpawn> objectsToBeSpawned = new();
    public List<Transform> possibleSpawns = new();
    private List<Vector3> spawnedLocations = new();
    public List<Transform> shopSpawns = new();

    public static InteractableSpawner instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    private void Start()
    {
        SpawnAllObjects();
    }

    private void Update()
    {
        // Temp Code:
        /*if (Input.GetKeyDown(KeyCode.G))
        {
            SpawnAllObjects();
        }*/
    }

    /// <summary>
    /// Primary Backend function in this class. Entirely handles properly spawning the desired amount of your specified GameObject, at one of the locations.
    /// </summary>
    /// <param name="objectToSpawn"></param>
    /// <param name="amount"></param>
    public void SpawnObject(GameObject objectToSpawn, int amount, List<Transform> locations)
    {
        if (objectToSpawn != null && amount > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                if(locations.Count > 0)
                {
                    int size = locations.Count;
                    int randomIndex = Random.Range(0, size);
                    Instantiate(objectToSpawn, locations[randomIndex].position, Quaternion.identity);
                    locations.RemoveAt(randomIndex);
                }
                else
                {
                    int Size = possibleSpawns.Count;
                    if (Size > 0)
                    {
                        int randomIndex = Random.Range(0, Size);
                        Instantiate(objectToSpawn, possibleSpawns[randomIndex].position, Quaternion.identity);
                        Debug.Log("Spawned: " + objectToSpawn.name + " at: " + possibleSpawns[randomIndex]);
                        //SpawnedLocations.Add(PossibleSpawnLocations[randomIndex]); 
                        possibleSpawns.RemoveAt(randomIndex);
                    }
                    else
                    {
                        Debug.Log("Not enough possible locations entered");
                       
                    }
                }



            }
        }
    }

    /// <summary>
    /// Spawns all pre set objects at random pre set locations. 
    /// </summary>
    private void SpawnAllObjects()
    {
        if (objectsToBeSpawned.Count > 0)
        {
            foreach (ObjectSpawn objectSpawn in objectsToBeSpawned)
            {
                SpawnObject(objectSpawn.objectToSpawn, objectSpawn.amount, objectSpawn.Locations);
            }
        }
        else
        {
            Debug.Log("No Objects to be spawned");
        }

    }

    /// <summary>
    /// Spawns a specific item bought from the computer shop.
    /// </summary>
    /// <param name="objectToSpawn"></param>
    /// <param name="transform"></param>
    public void SpawnSpecificObject(GameObject objectToSpawn, Transform transform)
    {
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// spawns all purchased items from computer shop. 
    /// </summary>
    /// <param name="Purchaseditems"></param>
    /// <param name="transforms"></param>
    public void SpawnAllPurchasedItems(List<GameObject> Purchaseditems, List<Transform> transforms)
    {
        //temporary until char sim have "purchasing objects" functionality 
        foreach (GameObject item in Purchaseditems)
        {
            int transformChoice = Random.Range(0, transforms.Count);
            Instantiate(item, transforms[transformChoice].position, Quaternion.identity);
            transforms.RemoveAt(transformChoice);
        }
    }
}
