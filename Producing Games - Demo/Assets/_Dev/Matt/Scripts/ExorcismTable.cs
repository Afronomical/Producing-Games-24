using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Written By: Matt Brake
/// <para> Moderated By: ...... </para>
/// <para> Holds the required items for a ritual/exorcism, compares the objects put down by the player
///  and decides the end result. </para>
/// </summary>
public class ExorcismTable : MonoBehaviour
{
    public static ExorcismTable instance;

    private int playerItemAmount;

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
        playerItemAmount = 0;
        Debug.Log(playerItemAmount);
     }


    public float radius = 2f;
    
    
    [ShowOnly]public List<GameObject> RequiredObjects = new();
    public List<GameObject> PlayerObjects = new();

    

    private void Update()
    {
       if(playerItemAmount < 3)
        {
            CheckForPlayerItems(); 
        }
       else if(playerItemAmount == 3)
        {
            if(DoListsMatch(PlayerObjects, RequiredObjects)) 
            {
                CompleteExorcism();
            }
            else if(!DoListsMatch(PlayerObjects, RequiredObjects))
            {
                FailExorcism();
            }
        }
    }

    /// <summary>
    /// Enables the exorcism object to know which objects are required for the exorcism at run time, depending on the demon.
    /// Only takes a list of gameobjects.
    /// </summary>
    /// <param name="newObjects"></param>
    public void SetRequiredObjects(List<GameObject> newObjects) ////called from when the demon is spawned to pass in the specific items for demon 
    {
        for (int i = 0; i < newObjects.Count; i++)
        {
            RequiredObjects.Add(newObjects[i]);
            Debug.Log(newObjects[i].name + "added to list");
        }
        Debug.Log("All items added to Required Objects list");  
    }


    /// <summary>
    /// Checks whether the required objects match the ones the player has laid down. If one is a mismatch, immediate fail.
    /// </summary>
    /// <param name="RequiredObjects"></param>
    /// <param name="PlayerObjects"></param>
    /// <returns></returns>
    public bool DoListsMatch(List<GameObject> RequiredObjects, List<GameObject> PlayerObjects)    
    {
        
        bool areListsEqual = true;
        if (RequiredObjects.Count != PlayerObjects.Count)
            return false;

    
        for (int i = 0; i < RequiredObjects.Count; i++)
        {
            if (!PlayerObjects.Contains(RequiredObjects[i]))
            {
                areListsEqual = false;
            }
        }
        areListsEqual = true;
        return areListsEqual;
       
    }

    public void FailExorcism()
    {
        Debug.Log("Failed Exorcism"); 
        ///enable rage mode here 
    }
    public void CompleteExorcism()
    {
        Debug.Log("Exorcism Completed!"); 
        //set task as complete here
        //
    }

    /// <summary>
    /// Performs a constant check for whether more items have been dropped on the table for the exorcism
    /// </summary>
    void CheckForPlayerItems()
    {
        //Debug.Log("Checking for player items");
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider collider in colliders)
        {
            ///check they are interactable objects e.g water, cross, before adding to the count 
            if(collider.gameObject.TryGetComponent(out IInteractable interactable))
            {
                PlayerObjects.Add(collider.gameObject);
                collider.gameObject.GetComponent<Collider>().enabled = false;   
                ++playerItemAmount;
                Debug.Log("Adding:" + collider.gameObject);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}
