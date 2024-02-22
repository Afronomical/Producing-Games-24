using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Task : MonoBehaviour
{
    public GameObject taskTarget;  // Patient that the task is for or object such as altar (The thing the player must interact with)
    protected InteractableTemplate targetInteraction;
    protected List<GameObject> detectingObjects;  // Used for detecting random tasks

    public bool isHourlyTask = true;
    public bool taskCompleted = false;
    [HideInInspector] public HourlyTask hTask;
    [HideInInspector] public RandomTask rTask;
    [HideInInspector] public bool taskNoticed = true;  // Used for telling the player about a random task
    [HideInInspector] public GameObject checkList;

    public virtual void TaskStart()
    {
        targetInteraction = taskTarget.GetComponent<InteractableTemplate>();

        if (!isHourlyTask)
        {
            taskNoticed = false;
        }
    }


    public virtual void CheckTaskConditions(GameObject interactedObject)
    {

    }


    public virtual void CheckDetectTask(GameObject interactedObject)
    {
        if (!isHourlyTask && !taskNoticed)
        {
            if (detectingObjects != null)
            {
                foreach (var obj in detectingObjects)
                {
                    if (interactedObject == taskTarget)  // Check for the correct object being looked at
                    {
                        DetectTask();
                    }
                }
            }
        }
    }


    public virtual void DetectTask()
    {
        if (!taskNoticed)
        {
            taskNoticed = true;
            CheckList.instance.AddTask(this);
        }
    }


    public virtual void CompleteTask()
    {
        taskCompleted = true;

        taskTarget.GetComponent<InteractableTemplate>().collectible = PatientTaskManager.instance.noTaskPrompt;
        TooltipManager.Instance.HideTooltip();


        //steam achievement for completing first task
        //if(SteamManager.Initialized)
        //{
        //    Steamworks.SteamUserStats.GetAchievement("CompleteTask", out bool completed);

        //    if(!completed)
        //    {
        //        SteamUserStats.SetAchievement("CompleteTask");
        //        SteamUserStats.StoreStats();
        //    }
        //}

        PatientTaskManager.instance.CompleteTask(this);
    }


    public virtual void FailTask()
    {

    }
}
