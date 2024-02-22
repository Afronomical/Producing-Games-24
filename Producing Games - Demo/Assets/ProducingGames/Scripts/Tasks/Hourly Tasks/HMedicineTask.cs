using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HMedicineTask : Task
{
    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being interacted with
        {
            if (InventoryHotbar.instance.currentItem == hTask.itemToGive && targetInteraction.collectible == hTask.tooltipPrompt)
            {
                InventoryHotbar.instance.RemoveFromInventory(InventoryHotbar.instance.currentItem);
                CompleteTask();
            }
        }
    }


    public override void CheckDetectTask(GameObject interactedObject)
    {
        if (interactedObject == taskTarget && taskTarget.GetComponent<PatientCharacter>().currentState == PatientCharacter.PatientStates.Bed)  // Check for the correct patient being looked at
        {
            if (InventoryHotbar.instance.currentItem == hTask.itemToGive)  // Check for the correct item being held
                targetInteraction.collectible = hTask.tooltipPrompt;
            else
            {
                targetInteraction.collectible = PatientTaskManager.instance.noTaskPrompt;
                TooltipManager.Instance.HideTooltip();
            }
        }
    }


    public override void CompleteTask()
    {
        Debug.Log("Completed medicine task");
        base.CompleteTask();
    }


    public override void FailTask()
    {
        if (taskTarget.TryGetComponent(out PatientCharacter character))
        {
            character.currentHealth -= 2;
            Debug.Log("Patient lost 2 health due to failed medicine task");
        }

        base.FailTask();
    }
}
