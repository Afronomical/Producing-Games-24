using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMedicineTask : Task
{
    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being interacted with
        {
            if (InventoryHotbar.instance.currentItem == hTask.itemToGive)  // Check for the correct item being held
            {
                InventoryHotbar.instance.RemoveFromInventory(InventoryHotbar.instance.currentItem);
                CompleteTask();

                //removing the request medicine animation
                if (interactedObject.TryGetComponent(out PatientCharacter character))
                {
                    character.animator.SetBool("reqMeds", false);
                }
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
