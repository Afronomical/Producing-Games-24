using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerCursor : InspectableObject
{
    public override void Interact()
    {
        base.Interact();

        Cursor.lockState = CursorLockMode.None;
    }

    protected override void Update()
    {
        base.Update();
        if(stopLooking) Cursor.lockState = CursorLockMode.Locked;
    }
}
