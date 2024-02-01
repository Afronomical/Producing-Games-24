using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public GameObject patient;

    public bool isHourlyTask = true;
    [HideInInspector] HourlyTask hTask;
    [HideInInspector] HourlyTask rTask;
    [HideInInspector] public bool taskNoticed = true;  // Used for telling the player about a random task

    void Start()
    {
        
    }


    void Update()
    {
        
    }


    public virtual void CheckTaskConditions()
    {

    }


    public virtual void CompleteTask()
    {

    }


    public virtual void FailTask()
    {

    }
}
