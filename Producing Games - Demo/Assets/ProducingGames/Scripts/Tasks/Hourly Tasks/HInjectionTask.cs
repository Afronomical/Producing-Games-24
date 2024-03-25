using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HInjectionTask : Task
{
    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being interacted with
        {
            if (InventoryHotbar.instance.currentItem == hTask.itemToGive && targetInteraction.collectible == hTask.tooltipPrompt)
            {
                InventoryHotbar.instance.RemoveFromInventory(InventoryHotbar.instance.currentItem);
                GameManager.Instance.player.GetComponent<PickUpItem>().arms.GiveInjection();
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
            else if (targetInteraction.collectible == hTask.tooltipPrompt)
            {
                targetInteraction.collectible = PatientTaskManager.instance.noTaskPrompt;
                TooltipManager.Instance.HideTooltip();
            }
        }
    }



    public override void CompleteTask()
    {
        Debug.Log("Completed injection task");
        base.CompleteTask();
    }


    public override void FailTask()
    {
        
   
            GameManager.Instance.AddSanity(-4);

           

            Debug.Log("Patient lost 4 sanity due to failed injection task");
        

        base.FailTask();
    }
}
