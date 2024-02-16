using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RWanderingTask : Task
{
    float distanceFromBed;

    public override void TaskStart()
    {
        detectingObjects = new List<GameObject>();
        detectingObjects.Add(taskTarget);  // Patient can be detected
        detectingObjects.Add(taskTarget.GetComponent<PatientCharacter>().bed);  // Empty bed can be detected

        GameObject[] groundToTPTo = GameObject.FindGameObjectsWithTag("Ground");

        int randomLocation = Random.Range(0, groundToTPTo.Length);
        taskTarget.transform.position = groundToTPTo[randomLocation].transform.position;

        if (taskTarget && taskTarget.TryGetComponent(out PatientCharacter character))
            character.ChangePatientState(PatientCharacter.PatientStates.Wandering);

        base.TaskStart();
    }


    void Update()
    {
        // Check if the patient is in their bed
        if (taskTarget && taskTarget.TryGetComponent(out PatientCharacter character))
        {
            if (character.currentState == PatientCharacter.PatientStates.Bed)
                CompleteTask();
        }
    }



    public override void CompleteTask()
    {
        Debug.Log("Completed wandering task");
        base.CompleteTask();
    }
}
