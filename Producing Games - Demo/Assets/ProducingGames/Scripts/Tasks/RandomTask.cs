using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Random Task", menuName = "Random Task")]
public class RandomTask : ScriptableObject
{
    public PatientTaskManager.RandomTasks taskType;

    [Range(0, 250)] public int chanceToHappen;

    public string taskName;

    public string taskDescription;

    public PatientTaskManager.TaskLocation location;

    public InteractiveObject itemToGive;

    public InteractiveObject tooltipPrompt;
}
