using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RWanderingTask : Task
{
    float distanceFromBed;

    public override void TaskStart()
    {
        GameObject[] groundToTPTo = GameObject.FindGameObjectsWithTag("Ground");

       int randomLocation = Random.Range(0, groundToTPTo.Length);
       taskTarget.transform.position = groundToTPTo[randomLocation].transform.position;

        // Teleport to a random location
        base.TaskStart();
    }


    void Update()
    {
        distanceFromBed = Vector3.Distance(taskTarget.transform.position, GameObject.FindWithTag("Bed").transform.position);

        // Check distance between patient and their bed
        if (taskTarget && taskTarget.TryGetComponent(out AICharacter character))
            if (distanceFromBed < 3)
            {
                CompleteTask();
            }
    }


    public override void CheckTaskConditions(GameObject interactedObject)
    {

    }


    public override void CompleteTask()
    {
        Debug.Log("Completed wandering task");
        base.CompleteTask();
    }
}
