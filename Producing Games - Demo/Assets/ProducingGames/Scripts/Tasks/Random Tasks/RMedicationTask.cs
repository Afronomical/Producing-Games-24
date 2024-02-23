using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RMedicationTask : Task
{
    public override void TaskStart()
    {
        detectingObjects = new List<GameObject>();
        detectingObjects.Add(taskTarget);  // Patient can be detected

        if (taskTarget && taskTarget.TryGetComponent(out PatientCharacter character))
            character.ChangePatientState(PatientCharacter.PatientStates.ReqMeds);

        base.TaskStart();
    }

    public override void CheckDetectTask(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being looked at
        {
            if (InventoryHotbar.instance.currentItem == rTask.itemToGive)  // Check for the correct item being held
                targetInteraction.collectible = rTask.tooltipPrompt;
            else if (targetInteraction.collectible == rTask.tooltipPrompt)
            {
                targetInteraction.collectible = PatientTaskManager.instance.noTaskPrompt;
                TooltipManager.Instance.HideTooltip();
            }
        }

        base.CheckDetectTask(interactedObject);
    }


    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being interacted with
        {
            if (InventoryHotbar.instance.currentItem == rTask.itemToGive)  // Check for the correct item being held
            {
                InventoryHotbar.instance.RemoveFromInventory(InventoryHotbar.instance.currentItem);
                CompleteTask();
            }
        }
    }

    public override void CompleteTask()
    {
        base.CompleteTask();
    }
}
