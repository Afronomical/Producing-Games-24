using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RHungryTask : Task
{
    private bool initialized = false;

    public override void TaskStart()
    {
        detectingObjects = new List<GameObject>();
        detectingObjects.Add(taskTarget);  // Patient can be detected
        detectingObjects.Add(taskTarget.GetComponent<PatientCharacter>().bed);  // Empty bed can be detected

        taskTarget.transform.position = NPCManager.Instance.RandomKitchenPosition();

        if (taskTarget && taskTarget.TryGetComponent(out PatientCharacter character))
        {
            character.ChangePatientState(PatientCharacter.PatientStates.Hungry);
            character.hasBeenHungry = true;
        }

        initialized = true;
        base.TaskStart();
    }


    void Update()
    {
        // Check if the patient is in their bed
        if (taskNoticed && initialized && taskTarget && taskTarget.TryGetComponent(out PatientCharacter character))
        {
            if (character.currentState == PatientCharacter.PatientStates.Bed)
            {
                CompleteTask();

                //removing the hungry animation
                character.animator.SetBool("isHungry", false);

            }


        }
    }



    public override void CheckDetectTask(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being looked at
        {
            if (taskTarget.GetComponent<PatientCharacter>().currentState == PatientCharacter.PatientStates.Hungry || taskTarget.GetComponent<PatientCharacter>().currentState == PatientCharacter.PatientStates.Abandoned)
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
        Debug.Log("Completed hungry task");
        
        base.CompleteTask();
    }
}
