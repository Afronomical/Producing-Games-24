using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PCleanTask : Task
{
    private float cleanTime = 5f;

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
        if (interactedObject == taskTarget)  // Check for the correct patient being looked at
        {
            targetInteraction.collectible = hTask.tooltipPrompt;
        }
    }


    private IEnumerator Clean()
    {
        taskTarget.GetComponent<InteractableTemplate>().collectible = PatientTaskManager.instance.noTaskPrompt;
        TooltipManager.Instance.HideTooltip();
        GameManager.Instance.player.GetComponent<PlayerInput>().enabled = false;
        yield return new WaitForSeconds(cleanTime);
        GameManager.Instance.player.GetComponent<PlayerInput>().enabled = true;
        CompleteTask();
    }


    public override void CompleteTask()
    {
        GameManager.Instance.AddSanity(6);
        base.CompleteTask();
    }


    public override void FailTask()
    {
        if (taskTarget.TryGetComponent(out PatientCharacter character))
        {
            GameManager.Instance.AddSanity(-4);
        }

        base.FailTask();
    }
}
