using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RHeartAttackTask : Task
{
    public float timeTillHeartAttack = 30;
    public float timeRemaining = 10;

    public override void TaskStart()
    {
        // Alert pager
        taskNoticed = true;
        PagerMessages.instance.DisplayMessage(taskTarget.name + " is about to have a heart attack!", 30);

        timeRemaining = timeTillHeartAttack;
        base.TaskStart();
    }


    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            FailTask();
        }
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
        if (taskTarget.TryGetComponent(out AICharacter character)) character.currentHealth += 1;
        Debug.Log("Completed heart attack task");
        base.CompleteTask();
    }


    public override void FailTask()
    {
        if (taskTarget.TryGetComponent(out AICharacter character))
        {
            character.currentHealth = 0;
            Debug.Log("Patient was killed by a heart attack");
        }

        base.FailTask();
    }
}
