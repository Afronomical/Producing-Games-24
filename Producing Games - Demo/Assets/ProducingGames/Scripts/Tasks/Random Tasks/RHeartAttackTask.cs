using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RHeartAttackTask : Task
{
    public float timeTillHeartAttack = 90;
    public float timeRemaining = 10;

    public override void TaskStart()
    {
        // Alert pager
        DetectTask();

        timeRemaining = timeTillHeartAttack;
        base.TaskStart();
        PagerMessages.instance.DisplayMessage(taskTarget.name + " is about to be killed by a heart attack", timeTillHeartAttack);
    }


    void Update()
    {
        if (!taskCompleted && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                FailTask();
            }
        }
    }


    public override void CheckDetectTask(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being looked at
        {
            targetInteraction.collectible = rTask.tooltipPrompt;
        }

        base.CheckDetectTask(interactedObject);
    }


    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being interacted with
        {
            CompleteTask();
        }
    }


    public override void CompleteTask()
    {
        if (taskTarget.TryGetComponent(out PatientCharacter character)) character.currentHealth += 1;
        Debug.Log("Completed heart attack task");
        base.CompleteTask();
    }


    public override void FailTask()
    {
        if (taskTarget.TryGetComponent(out PatientCharacter character))
        {
            character.currentHealth = 0;
            PagerMessages.instance.DisplayMessage("Patient was killed by a heart attack", 10);
        }

        base.FailTask();
    }
}
