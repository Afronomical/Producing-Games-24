using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComputerCursor : InspectableObject
{
    public SoundEffect clickSound;

    public override void Interact()
    {
        base.Interact();

        Cursor.lockState = CursorLockMode.None;
    }

    protected override void Update()
    {
        base.Update();
        if (stopLooking)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


    public void OnClickInput(InputAction.CallbackContext context)
    {
        if (context.performed && looking)
            AudioManager.instance.PlaySound(clickSound, null);
    }
}
