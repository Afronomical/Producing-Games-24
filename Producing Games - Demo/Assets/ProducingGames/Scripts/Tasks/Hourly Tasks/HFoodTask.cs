using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HFoodTask : Task
{
    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being interacted with
        {
            if (InventoryHotbar.instance.currentItem == hTask.itemToGive && taskTarget.GetComponent<InteractableTemplate>().collectible == hTask.tooltipPrompt)
            {
                InventoryHotbar.instance.RemoveFromInventory(InventoryHotbar.instance.currentItem);
                CompleteTask();

                //removing the hungry animation
                if(interactedObject.TryGetComponent(out PatientCharacter character))
                {
                    character.animator.SetBool("isHungry", false);
                }
            }
        }
    }


    public override void CheckDetectTask(GameObject interactedObject)
    {
        if (interactedObject == taskTarget && taskTarget.GetComponent<PatientCharacter>().currentState == PatientCharacter.PatientStates.Bed)  // Check for the correct patient being looked at
        {
            if (InventoryHotbar.instance.currentItem == hTask.itemToGive)  // Check for the correct item being held
                taskTarget.GetComponent<InteractableTemplate>().collectible = hTask.tooltipPrompt;
            else if (targetInteraction.collectible == hTask.tooltipPrompt)
            {
                taskTarget.GetComponent<InteractableTemplate>().collectible = PatientTaskManager.instance.noTaskPrompt;
                TooltipManager.Instance.HideTooltip();
            }
        }
    }


    public override void CompleteTask()
    {
        Debug.Log("Completed food task");
        
        base.CompleteTask();
    }


    public override void FailTask()
    {
        if (taskTarget.TryGetComponent(out PatientCharacter character))
        {
            character.currentHealth -= 1;
            Debug.Log("Patient lost 1 health due to failed food task");
        }

        base.FailTask();
    }
}
