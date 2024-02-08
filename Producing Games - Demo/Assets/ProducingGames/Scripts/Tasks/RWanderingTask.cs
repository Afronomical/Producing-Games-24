using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RWanderingTask : Task
{
    void Start()
    {
        
    }


    void Update()
    {
        
    }


    public override void CheckTaskConditions(GameObject interactedObject)
    {

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
            Debug.Log("Patient lost 2 health due to failed medicine task");
        }

        base.FailTask();
    }
}
