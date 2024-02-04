using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Task : MonoBehaviour
{
    public GameObject taskTarget;  // Patient that the task is for or object such as altar (The thing the player must interact with)

    public bool isHourlyTask = true;
    [HideInInspector] public HourlyTask hTask;
    [HideInInspector] public HourlyTask rTask;
    [HideInInspector] public bool taskNoticed = true;  // Used for telling the player about a random task

    void Start()
    {

    }


    void Update()
    {
        
    }


    public virtual void CheckTaskConditions(GameObject interactedObject)
    {

    }


    public virtual void CompleteTask()
    {
        taskTarget.transform.Find("Eye 1").GetComponent<MeshRenderer>().material = hTask.basicEyes;
        taskTarget.transform.Find("Eye 2").GetComponent<MeshRenderer>().material = hTask.basicEyes;
        PatientTaskManager.instance.CompleteTask(this);
    }


    public virtual void FailTask()
    {
        taskTarget.transform.Find("Eye 1").GetComponent<MeshRenderer>().material = hTask.basicEyes;
        taskTarget.transform.Find("Eye 2").GetComponent<MeshRenderer>().material = hTask.basicEyes;
    }
}
