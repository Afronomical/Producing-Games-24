using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Written By: Matt Brake
/// <para> Moderated By: ...... </para>
/// <para> Determines functionality for Patients when they are hungry</para>
/// </summary>
public class HungryState : StateBaseClass
{
    private Vector3 destinationPos;
    private float distanceFromFood;
    private bool inKitchen;
    private float currentIdleTime = 0f;
    private float maxIdleTime = 3f; 

    private void Awake()
    {
        character.isMoving = true; 
    }

    private void Start()
    {
        character.agent.ResetPath();
        ChooseKitchenDestination();
    }

    public override void UpdateLogic()
    {
        character.agent.SetDestination(destinationPos);
        if(CheckDistanceToLocation())
        {
             currentIdleTime += Time.deltaTime;

            if(currentIdleTime >= maxIdleTime)
            {
                ChooseKitchenDestination(); 
            }
        }
    }


    /// <summary>
    /// Checks the distance from character to the destination and stops if within range
    /// </summary>
    /// <returns></returns>
    public bool CheckDistanceToLocation()
    {
        if (Vector3.Distance(character.transform.position, destinationPos) < distanceFromFood)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Eat()
    {
        /// whatever specs we get next for this state 
    }

    public void ChooseKitchenDestination()
    {
       destinationPos = NPCManager.Instance.RandomKitchenPosition();
    }
}
