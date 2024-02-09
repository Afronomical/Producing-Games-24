using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HInjectionTask : Task
{
    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being interacted with
        {
            if (InventoryHotbar.instance.currentItem == hTask.itemToGive)  // Check for the correct item being held
            {
                InventoryHotbar.instance.RemoveFromInventory(InventoryHotbar.instance.currentItem);
                CompleteTask();
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
        
   
            GameManager.Instance.currentSanity -= 4;

           

            Debug.Log("Patient lost 4 sanity due to failed injection task");
        

        base.FailTask();
    }
}
