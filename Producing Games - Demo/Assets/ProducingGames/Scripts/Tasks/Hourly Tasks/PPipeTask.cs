using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPipeTask : Task
{
    public override void TaskStart()
    {
        taskTarget.GetComponent<PipeMinigame>().enabled = true;
        taskTarget.GetComponent<PipeMinigame>().InitializePuzzle();
        base.TaskStart();
    }

    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget.GetComponent<PipeMinigame>().valve.gameObject && taskTarget.GetComponent<PipeMinigame>().complete)
            CompleteTask();
    }


    public override void CompleteTask()
    {
        GameManager.Instance.AddSanity(2);
        base.CompleteTask();
    }


    public override void FailTask()
    {
        taskTarget.GetComponent<PipeMinigame>().valve.GetComponent<BoxCollider>().enabled = false;
        taskTarget.GetComponent<PipeMinigame>().enabled = false;
        taskTarget.GetComponent<BoxCollider>().enabled = false;
        PatientTaskManager.instance.waterHasBroken = true;
        GameManager.Instance.AddSanity(-4);
        base.FailTask();
    }
}
