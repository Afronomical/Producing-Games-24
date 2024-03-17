using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCardiogramTask : Task
{
    GameObject patient;
    HRM hrm;

    public override void TaskStart()
    {
        patient = taskTarget;
        taskTarget = taskTarget.GetComponent<PatientCharacter>().bed.transform.Find("HR Monitor").gameObject;
        hrm = taskTarget.transform.GetChild(0).GetChild(1).GetComponent<HRM>();
        if (hrm == null) Debug.LogError("Cardiogram Random Task failed to find cardiogram");
        hrm.TurnOff();
        DiegeticUIManager.Instance.pagerBroken = true;
        base.TaskStart();
    }

    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget && targetInteraction.collectible == hTask.tooltipPrompt)
        {
            hrm.TurnOn();
            DiegeticUIManager.Instance.pagerBroken = false;
            CompleteTask();
        }
    }


    public override void CheckDetectTask(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being looked at
        {
            targetInteraction.collectible = hTask.tooltipPrompt;
        }
    }


    public override void CompleteTask()
    {
        if (patient.TryGetComponent(out PatientCharacter character))
        {
            character.cardiogramNotChecked = false;
        }

        base.CompleteTask();
    }


    public override void FailTask()
    {
        base.FailTask();
    }
}
