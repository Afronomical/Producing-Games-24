using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HolyWaterFountain : InteractableTemplate
{
    public bool playerHasJug; ///need this to pull in a player bool to whether its in the inventory or not 
    public GameObject jug;
    public int timesUsedThisHour = 0;
    public int maxUsesPerHour = 4;
    public float fillTime;
    public Vector3 fillPoint; 
    
    public override void Interact()
    {
       //collect water from the fountain if it has any 
       if(playerHasJug && timesUsedThisHour < maxUsesPerHour)
        {
            //collect water here 
            fillJug();
            ++timesUsedThisHour;

        }
        else
        {
            ///tell player they cannot collect water 
        }
    }


    private void fillJug()
    {
        jug.transform.position = fillPoint; ///for the set amount of time 
        StartCoroutine(startFill());
        return; 
    }


    private IEnumerator startFill()
    {
        yield return new WaitForSeconds(fillTime);    
        
    }
    
}
