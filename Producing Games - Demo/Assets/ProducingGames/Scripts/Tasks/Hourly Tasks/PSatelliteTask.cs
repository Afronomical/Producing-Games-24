using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PSatelliteTask : Task
{
    public override void TaskStart()
    {
        taskTarget.GetComponent<SatelliteBox>().enabled = true;
        taskTarget.GetComponent<SatelliteBox>().InitializeBox();
        base.TaskStart();
    }

    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget && taskTarget.GetComponent<SatelliteBox>().complete)
            CompleteTask();
    }


    public override void CheckDetectTask(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being looked at
        {
            targetInteraction.collectible = hTask.tooltipPrompt;
        }
    }


    public override void CompleteTask()
    {
        GameManager.Instance.AddSanity(2);
        base.CompleteTask();
    }


    public override void FailTask()
    {
        taskTarget.GetComponent<SatelliteBox>().enabled = false;
        taskTarget.GetComponent<BoxCollider>().enabled = false;
        PatientTaskManager.instance.satelliteHasBroken = true;
        GameManager.Instance.AddSanity(-4);
        base.FailTask();
    }
}
