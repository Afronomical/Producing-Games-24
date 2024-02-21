using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RHungryTask : Task
{
    
    public override void TaskStart()
    {
        detectingObjects = new List<GameObject>();
        detectingObjects.Add(taskTarget);  // Patient can be detected
        detectingObjects.Add(taskTarget.GetComponent<PatientCharacter>().bed);  // Empty bed can be detected

        taskTarget.transform.position = NPCManager.Instance.RandomKitchenPosition();

        if (taskTarget && taskTarget.TryGetComponent(out PatientCharacter character))
            character.ChangePatientState(PatientCharacter.PatientStates.Hungry);

        base.TaskStart();
    }


    void Update()
    {
        // Check if the patient is in their bed
        if (taskTarget && taskTarget.TryGetComponent(out PatientCharacter character))
        {
            if (character.currentState == PatientCharacter.PatientStates.Bed)
            {
                CompleteTask();

                //removing the hungry animation
                character.animator.SetBool("isHungry", false);

            }


        }
    }



    public override void CompleteTask()
    {
        Debug.Log("Completed hungry task");
        
        base.CompleteTask();
    }
}
