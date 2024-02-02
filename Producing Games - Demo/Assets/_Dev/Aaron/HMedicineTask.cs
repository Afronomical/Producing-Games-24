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
        /*if (interactedObject == patient)
        {

        }

        if (InventoryHotbar.instance.heldObject)
        base.CheckTaskConditions();*/
    }


    public override void CompleteTask()
    {

        base.CompleteTask();
    }


    public override void FailTask()
    {

        base.FailTask();
    }
}
