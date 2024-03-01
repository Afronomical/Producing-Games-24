using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HComfortTask : Task
{
    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget && targetInteraction.collectible == hTask.tooltipPrompt) 
        {
            CompleteTask();
        }
    }


    public override void CheckDetectTask(GameObject interactedObject)
    {
        if (interactedObject == taskTarget && taskTarget.GetComponent<PatientCharacter>().currentState == PatientCharacter.PatientStates.Bed)  // Check for the correct patient being looked at
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
        if (taskTarget.TryGetComponent(out PatientCharacter character))
        {
            character.currentHealth -= 1;
            GameManager.Instance.AddSanity(-1);
        }

        base.FailTask();
    }
}
