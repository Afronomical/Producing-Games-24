using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HFoodTask : Task
{
    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being interacted with
        {
            if (InventoryHotbar.instance.currentItem == hTask.itemToGive)  // Check for the correct item being held
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
