using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RPrayingTask : Task
{
    public override void TaskStart()
    {
        detectingObjects = new List<GameObject>();
        detectingObjects.Add(taskTarget);  // Patient can be detected
        detectingObjects.Add(taskTarget.GetComponent<PatientCharacter>().bed);  // Empty bed can be detected

        taskTarget.transform.position = NPCManager.Instance.RandomPrayingDestination();

        if (taskTarget && taskTarget.TryGetComponent(out PatientCharacter character))
            character.ChangePatientState(PatientCharacter.PatientStates.Prayer);

        base.TaskStart();
    }


    public override void CheckDetectTask(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being looked at
        {
            if (taskTarget.GetComponent<PatientCharacter>().currentState == PatientCharacter.PatientStates.Prayer || taskTarget.GetComponent<PatientCharacter>().currentState == PatientCharacter.PatientStates.Abandoned)
                targetInteraction.collectible = rTask.tooltipPrompt;
            else
            {
                targetInteraction.collectible = PatientTaskManager.instance.noTaskPrompt;
                TooltipManager.Instance.HideTooltip();
            }
        }

        base.CheckDetectTask(interactedObject);
    }


    void Update()
    {
        // Check if the patient is in their bed
        if (!taskCompleted && taskNoticed && taskTarget && taskTarget.TryGetComponent(out PatientCharacter character))
        {
            if (character.currentState == PatientCharacter.PatientStates.Bed)
            {
                CompleteTask();
                character.animator.SetBool("isPraying", false);
            }

            
        }
    }



    public override void CompleteTask()
    {
        Debug.Log("Completed praying task");
        
        base.CompleteTask();
    }
}
