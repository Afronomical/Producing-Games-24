using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestMedicationState : StateBaseClass
{
    private void Start()
    {
        Debug.Log(gameObject.name + ": requests medication.");
    }

    public override void UpdateLogic()
    {

    }
}
