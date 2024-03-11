using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RHidingTask : Task
{
    private bool initialized = false;

    public override void TaskStart()
    {
        detectingObjects = new List<GameObject>();
        detectingObjects.Add(taskTarget);  // Patient can be detected
        detectingObjects.Add(taskTarget.GetComponent<PatientCharacter>().bed);  // Empty bed can be detected

        taskTarget.transform.position = NPCManager.Instance.RandomHidingLocation();

        if (taskTarget && taskTarget.TryGetComponent(out PatientCharacter character))
        {
            character.ChangePatientState(PatientCharacter.PatientStates.Hiding);
            character.hasBeenHiding = true;
        }

        initialized = true;
        base.TaskStart();
    }


    void Update()
    {
        // Check if the patient is in their bed
        if (!taskCompleted && taskNoticed && initialized && taskTarget && taskTarget.TryGetComponent(out PatientCharacter character))
        {
            if (character.currentState == PatientCharacter.PatientStates.Bed)
                CompleteTask();
        }
    }


    public override void CheckDetectTask(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being looked at
        {
            if (taskTarget.GetComponent<PatientCharacter>().currentState == PatientCharacter.PatientStates.Hiding || taskTarget.GetComponent<PatientCharacter>().currentState == PatientCharacter.PatientStates.Abandoned)
                targetInteraction.collectible = rTask.tooltipPrompt;
            else
            {
                targetInteraction.collectible = PatientTaskManager.instance.noTaskPrompt;
                TooltipManager.Instance.HideTooltip();
            }
        }

        base.CheckDetectTask(interactedObject);
    }



    public override void CompleteTask()
    {
        Debug.Log("Completed Hiding task");
        base.CompleteTask();
    }
}
