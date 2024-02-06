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
    public float radius = 2f;
    private int playerItemAmount = 0;
    
    [ShowOnly]public List<GameObject> RequiredObjects = new();
    public List<GameObject> PlayerObjects = new();

    private void Update()
    {
       if(playerItemAmount < 3)
        {
            CheckForPlayerItems(); 
        }
    }

    /// <summary>
    /// Enables the exorcism object to know which objects are required for the exorcism at run time, depending on the demon.
    /// Only takes a list of gameobjects.
    /// </summary>
    /// <param name="newObjects"></param>
    public void SetRequiredObjects(List<GameObject> newObjects)
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
    /// <param name="list1"></param>
    /// <param name="list2"></param>
    /// <returns></returns>
    public bool DoListsMatch(List<GameObject> list1, List<GameObject> list2)    
    {
        bool areListsEqual = true;
        if (list1.Count != list2.Count)
            return false;

        list1.Sort();
        list2.Sort();
        for (int i = 0; i < list1.Count; i++)
        {
            if (list2[i] != list1[i])
            {
                areListsEqual = false;
            }
        }
        return areListsEqual;
       
    }

    public void FailExorcism()
    {
        Debug.Log("Failed Exorcism"); 
        ///enable rage mode 
    }
    public void CompleteExorcism()
    {
        Debug.Log("Exorcism Completed!"); 
    }

    /// <summary>
    /// Performs a constant check for whether more items have been dropped on the table for the exorcism
    /// </summary>
    void CheckForPlayerItems()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider collider in colliders)
        {
            ///check they are interactable objects e.g water, cross, before adding to the count 
            ++playerItemAmount;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}
