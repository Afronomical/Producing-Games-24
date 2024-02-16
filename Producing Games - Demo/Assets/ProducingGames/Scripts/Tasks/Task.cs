using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Task : MonoBehaviour
{
    public GameObject taskTarget;  // Patient that the task is for or object such as altar (The thing the player must interact with)
    protected List<GameObject> detectingObjects;  // Used for detecting random tasks

    public bool isHourlyTask = true;
    public bool taskCompleted = false;
    [HideInInspector] public HourlyTask hTask;
    [HideInInspector] public RandomTask rTask;
    [HideInInspector] public bool taskNoticed = true;  // Used for telling the player about a random task
    [HideInInspector] public GameObject checkList;

    public virtual void TaskStart()
    {
        if (!isHourlyTask)
        {
            taskNoticed = false;
        }
    }


    public virtual void CheckTaskConditions(GameObject interactedObject)
    {

    }


    public void CheckDetectTask(GameObject interactedObject)
    {
        foreach (GameObject obj in detectingObjects)
        {
            if (interactedObject == taskTarget)  // Check for the correct object being looked at
            {
                DetectTask();
            }
        }
    }


    public virtual void DetectTask()
    {
        taskNoticed = true;
        CheckList.instance.AddTask(this);
    }


    public virtual void CompleteTask()
    {
        taskCompleted = true;

        if (isHourlyTask)
        {
            taskTarget.transform.Find("Eye 1").GetComponent<MeshRenderer>().material = hTask.basicEyes;
            taskTarget.transform.Find("Eye 2").GetComponent<MeshRenderer>().material = hTask.basicEyes;
        }

        PatientTaskManager.instance.CompleteTask(this);
    }


    public virtual void FailTask()
    {
        if (isHourlyTask)
        {
            taskTarget.transform.Find("Eye 1").GetComponent<MeshRenderer>().material = hTask.basicEyes;
            taskTarget.transform.Find("Eye 2").GetComponent<MeshRenderer>().material = hTask.basicEyes;
        }
    }
}
