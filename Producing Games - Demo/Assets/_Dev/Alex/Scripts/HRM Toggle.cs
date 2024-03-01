using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HRMToggle : InteractableTemplate
{
    public override void Interact()
    {
        if(HRM.instance.gameObject.activeSelf) HRM.instance.TurnOff();
        else HRM.instance.TurnOn();
    }
}
