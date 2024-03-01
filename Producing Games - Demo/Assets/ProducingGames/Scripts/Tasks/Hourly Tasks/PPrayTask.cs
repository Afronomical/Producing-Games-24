using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PPrayTask : Task
{
    private float prayTime = 5f;

    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct object being interacted with
        {
            StartCoroutine(Pray());
        }
    }


    public override void CheckDetectTask(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being looked at
        {
            targetInteraction.collectible = hTask.tooltipPrompt;
        }
    }


    private IEnumerator Pray()
    {
        taskTarget.GetComponent<InteractableTemplate>().collectible = PatientTaskManager.instance.noTaskPrompt;
        TooltipManager.Instance.HideTooltip();
        GameManager.Instance.player.GetComponent<PlayerInput>().enabled = false;
        yield return new WaitForSeconds(prayTime);
        GameManager.Instance.player.GetComponent<PlayerInput>().enabled = true;
        CompleteTask();
    }


    public override void CompleteTask()
    {
        GameManager.Instance.AddSanity(4);
        base.CompleteTask();
    }


    public override void FailTask()
    {
        if (taskTarget.TryGetComponent(out PatientCharacter character))
        {
            GameManager.Instance.AddSanity(-2);
        }

        base.FailTask();
    }
}
