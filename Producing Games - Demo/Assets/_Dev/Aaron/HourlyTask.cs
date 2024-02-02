using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hourly Task", menuName = "Hourly Task")]
public class HourlyTask : ScriptableObject
{
    public PatientTaskManager.HourlyTasks taskType;

    [Range(0, 100)] public int chanceToHappen;

    public HourlyTask[] BlockingTasks;

    public string taskName;

    public string taskDescription;

    public PatientTaskManager.TaskLocation location;

    public InteractiveObject itemToGive;

    public bool genericTask;
}
