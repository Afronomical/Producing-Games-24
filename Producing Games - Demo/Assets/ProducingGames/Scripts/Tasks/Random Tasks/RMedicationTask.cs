using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RMedicationTask : Task
{
    public override void TaskStart()
    {
        detectingObjects = new List<GameObject>();
        detectingObjects.Add(taskTarget);  // Patient can be detected
        detectingObjects.Add(taskTarget.GetComponent<PatientCharacter>().bed);  // Empty bed can be detected

        

        if (taskTarget && taskTarget.TryGetComponent(out PatientCharacter character))
            character.ChangePatientState(PatientCharacter.PatientStates.ReqMeds);

        base.TaskStart();
    }

    public override void CheckTaskConditions(GameObject interactedObject)
    {
        if (interactedObject == taskTarget)  // Check for the correct patient being interacted with
        {
            if (InventoryHotbar.instance.currentItem == rTask.itemToGive)  // Check for the correct item being held
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
        Debug.Log("Completed medication task");
        base.CompleteTask();
    }
}
