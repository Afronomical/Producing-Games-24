using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Written By: Matthew Brake 
/// Moderated By: .......
/// 
/// This Class is responsible entirely for spawning interactable objects in random pre-set locations across the level. Pass in the transform of the spawn location empty game object into the spawn location parameters
/// </summary>

public class InteractableSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ObjectSpawn
    {
        public GameObject ObjectToSpawn;
        public int amount;
    }

    public List<ObjectSpawn> ObjectsToBeSpawned = new List<ObjectSpawn>();
    //public List<Vector3> PossibleSpawnLocations = new List<Vector3>();
    public List<Transform> PossibleSpawn = new List<Transform> ();
    private List<Vector3> SpawnedLocations = new List<Vector3>();
    //public GameObject MedicineBottle;
    //public GameObject Rosary;
    //public int RosariesToSpawn;
    //public int MedicineToSpawn; 
   
     
    // Start is called before the first frame update
    void Start()
    {
        SpawnAllObjects();  
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Primary Backend function in this class. Entirely handles properly spawning the desired amount of your specified GameObject, at one of the locations.
    /// </summary>
    /// <param name="objectToSpawn"></param>
    /// <param name="amount"></param>
    private void SpawnObject(GameObject objectToSpawn, int amount)
    {
        if(objectToSpawn != null && amount > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                int Size = PossibleSpawn.Count;
                if (Size > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, Size);
                    Instantiate(objectToSpawn, PossibleSpawn[randomIndex].position, Quaternion.identity);
                    Debug.Log("Spawned: " + objectToSpawn.gameObject.name + " at: " + PossibleSpawn[randomIndex]);
                    //SpawnedLocations.Add(PossibleSpawnLocations[randomIndex]); 
                    PossibleSpawn.RemoveAt(randomIndex);
                }
                else
                {
                    Debug.Log("Not enough possible locations entered");
                }
            }
        }
    }


   public void SpawnAllObjects()
    {
        if(ObjectsToBeSpawned.Count > 0)
        {
            foreach (ObjectSpawn objectSpawn in ObjectsToBeSpawned)
            {
                SpawnObject(objectSpawn.ObjectToSpawn, objectSpawn.amount);
            }
        }
        else
        {
            Debug.Log("No Objects to be spawned"); 
        }
        
    }
}
