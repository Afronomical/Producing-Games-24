using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMedicineTask : Task
{
    void Start()
    {
        
    }


    void Update()
    {
        
    }


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
        Debug.Log("Completed medicine task");
        base.CompleteTask();
    }


    public override void FailTask()
    {
        if (taskTarget.TryGetComponent(out AICharacter character))
        {
            character.health -= 2;
            Debug.Log("Patient lost 2 health due to failed medicine task");
        }

        base.FailTask();
    }
}
