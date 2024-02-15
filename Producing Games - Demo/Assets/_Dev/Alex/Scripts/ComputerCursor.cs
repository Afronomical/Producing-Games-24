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
}
