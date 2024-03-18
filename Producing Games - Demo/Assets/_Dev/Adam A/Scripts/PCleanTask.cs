using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PCleanTask : Task
{
    private float cleanTime = 5f;//Time it takes to clean
    private int gainedSanity = 6;//Gained Sanity if task is completed
    private int lossSanity = -4;//Loss Sanity if failed task

    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct object being interacted with
        {
            StartCoroutine(Clean());
        }
    }


    public override void CheckDetectTask(GameObject interactedObject)
    {
        //Debug.Log(interactedObject + " - " + taskTarget);
        if (interactedObject == taskTarget)  // Check for the correct table is being looked at
        {
            targetInteraction.collectible = hTask.tooltipPrompt;
        }
    }


    private IEnumerator Clean()
    {
        taskTarget.GetComponent<InteractableTemplate>().collectible = PatientTaskManager.instance.noTaskPrompt;
        TooltipManager.Instance.HideTooltip();
        GameManager.Instance.player.GetComponent<PlayerInput>().enabled = false;//Disable the controls for the player
        yield return new WaitForSeconds(cleanTime);
        GameManager.Instance.player.GetComponent<PlayerInput>().enabled = true;//Enables the controls when the specified time is finished
        CompleteTask();
    }

    //Adds specified Sanity to the player and completes the task
    public override void CompleteTask()
    {
        GameManager.Instance.AddSanity(gainedSanity);
        base.CompleteTask();
    }

    //Task Failed
    public override void FailTask()
    {
        if (taskTarget.TryGetComponent(out PatientCharacter character))
        {
            GameManager.Instance.AddSanity(lossSanity);
        }

        base.FailTask();
    }
}
