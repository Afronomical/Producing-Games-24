using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PFuseBoxTask : Task
{
    private int gainedSanity = 8;//Gained Sanity if task is completed

    //Sets up the fusebox to be "broken"
    public override void TaskStart()
    {
        taskTarget.GetComponent<FuseBox>().enabled = true;
        taskTarget.GetComponent<FuseBox>().InitializeBox();
        base.TaskStart();
    }

    //If all 4 fuses are correctly placed, the task is complete
    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget && taskTarget.GetComponent<FuseBox>().complete)
            CompleteTask();
    }


    public override void CheckDetectTask(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check if the fusebox is being looked at
        {
            targetInteraction.collectible = hTask.tooltipPrompt;
        }
    }

    //Adds specified Sanity to the player and completes the task
    public override void CompleteTask()
    {
        GameManager.Instance.AddSanity(8);
        base.CompleteTask();
    }

    //Task has failed
    public override void FailTask()
    {
        taskTarget.GetComponent<FuseBox>().enabled = false;
        taskTarget.GetComponent<BoxCollider>().enabled = false;
        LightManager.Instance.AllLightToggle(true);
        base.FailTask();
    }
}
