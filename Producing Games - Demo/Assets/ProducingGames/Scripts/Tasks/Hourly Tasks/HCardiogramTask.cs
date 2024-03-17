using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HCardiogramTask : Task
{
    GameObject patient;

    public override void TaskStart()
    {
        patient = taskTarget;
        taskTarget = taskTarget.GetComponent<PatientCharacter>().bed.transform.Find("HR Monitor").gameObject;
        base.TaskStart();
    }

    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget && targetInteraction.collectible == hTask.tooltipPrompt) 
        {
            CompleteTask();
        }
    }


    public override void CheckDetectTask(GameObject interactedObject)
    {
        if (interactedObject == taskTarget && patient.GetComponent<PatientCharacter>().currentState == PatientCharacter.PatientStates.Bed)  // Check for the correct patient being looked at
        {
            targetInteraction.collectible = hTask.tooltipPrompt;
        }
    }


    public override void CompleteTask()
    {
        base.CompleteTask();
    }


    public override void FailTask()
    {
        if (patient.TryGetComponent(out PatientCharacter character))
        {
            character.cardiogramNotChecked = true;
        }

        base.FailTask();
    }
}
