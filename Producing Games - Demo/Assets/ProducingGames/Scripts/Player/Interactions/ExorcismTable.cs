using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By: Matt Brake
/// <para> Moderated By: Matej Cincibus </para>
/// <para> Holds the required items for a ritual/exorcism, compares the objects put down by the player
///  and decides the end result. </para>
/// </summary>

public class ExorcismTable : MonoBehaviour
{
    [SerializeField] private float radius = 2.0f;
    [SerializeField] private List<GameObject> playerObjects = new();
    [ShowOnly] public List<GameObject> requiredObjects = new();

    private int playerItemAmount = 0;

    private void Start()
    {
        AcquireDemonObjects();
    }

    private void Update()
    {
        if (playerItemAmount < 3)
            CheckForPlayerItems();
        else if (playerItemAmount == 3)
        {
            if (DoListsMatch(playerObjects, requiredObjects))
                CompleteExorcism();
            else if (!DoListsMatch(playerObjects, requiredObjects))
                FailExorcism();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    ///// <summary>
    ///// Enables the exorcism object to know which objects are required for the exorcism at run time, depending on the demon.
    ///// Only takes a list of gameobjects.
    ///// </summary>
    ///// <param name="newObjects"></param>
    //public void SetRequiredObjects(List<GameObject> newObjects) //called from when the demon is spawned to pass in the specific items for demon 
    //{
    //    for (int i = 0; i < newObjects.Count; i++)
    //    {
    //        requiredObjects.Add(newObjects[i]);
    //        Debug.Log(newObjects[i].name + "added to list");
    //    }
    //    Debug.Log("All items added to Required Objects list");
    //}

    /// <summary>
    /// Checks whether the required objects match the ones the player has laid down. If one is a mismatch, immediate fail.
    /// </summary>
    /// <param name="RequiredObjects"></param>
    /// <param name="PlayerObjects"></param>
    /// <returns></returns>
    public bool DoListsMatch(List<GameObject> RequiredObjects, List<GameObject> PlayerObjects)
    {
        if (RequiredObjects.Count != PlayerObjects.Count)
            return false;

        for (int i = 0; i < RequiredObjects.Count; i++)
        {
          if(playerObjects[i].GetComponent<InteractableTemplate>().collectible.objectName != requiredObjects[i].GetComponent<InteractableTemplate>().collectible.objectName)
           
                return false;
        }

        return true;
    }

    public void FailExorcism()
    {
        Debug.Log("Failed Exorcism");
        GameManager.Instance.exorcismFailed = true;
        GameManager.Instance.EndGame(false);
        //enable rage mode here. or game end 

    }

    public void CompleteExorcism()
    {

        //steam achievement for completing exorcism
        //if(SteamManager.Initialized)
        //{
        //    Steamworks.SteamUserStats.GetAchievement("CompleteExorcism", out bool completed);

        //    if(!completed)
        //    {
        //        SteamUserStats.SetAchievement("CompleteExorcism");
        //        SteamUserStats.StoreStats();
        //    }
        //}

        Debug.Log("Exorcism Completed!");
        //set task as complete here. win screen or demon scream etc 
        GameManager.Instance.demon.GetComponent<DemonCharacter>().ChangeDemonState(DemonCharacter.DemonStates.Exorcised);
        GameManager.Instance.EndGame(true);
    }

    /// <summary>
    /// Performs a constant check for whether more items have been dropped on the table for the exorcism
    /// </summary>
    void CheckForPlayerItems()
    {
        //Debug.Log("Checking for player items");
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            //check they are interactable objects e.g water, cross, before adding to the count 
            if (collider.gameObject.TryGetComponent(out IInteractable interactable))
            {
                playerObjects.Add(collider.gameObject);
                collider.gameObject.GetComponent<Collider>().enabled = false;
                ++playerItemAmount;
                Debug.Log("Adding:" + collider.gameObject);
            }
        }
    }

    void AcquireDemonObjects()
    {
        foreach(var item in NPCManager.Instance.ChosenDemon.itemsForExorcism)
        {
            requiredObjects.Add(item.gameObject);
            Debug.Log(item.gameObject + "has been added to required list ");
        }
        
    }

}
