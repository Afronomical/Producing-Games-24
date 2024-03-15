using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HRMToggle : InteractableTemplate
{
    public HRM heartMonitor;

    public override void Interact()
    {
        if(!heartMonitor.staticEffect.activeSelf) heartMonitor.TurnOff();
        else heartMonitor.TurnOn();

        //if(!HRM.instance.staticEffect.activeSelf) HRM.instance.TurnOff();
        //else HRM.instance.TurnOn();
    }
}
