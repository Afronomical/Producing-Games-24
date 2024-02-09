using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RWanderingTask : Task
{
    public override void TaskStart()
    {
        // Teleport to a random location

        base.TaskStart();
    }


    void Update()
    {
        // Check distance between patient and their bed
        if (taskTarget && taskTarget.TryGetComponent(out AICharacter character))
        {
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
