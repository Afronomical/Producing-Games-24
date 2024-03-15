using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComputerCursor : InspectableObject
{
    public SoundEffect clickSound;
    public TMP_Text clock;

    public override void Interact()
    {
        base.Interact();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        actionTooltip.enabled = true;
        actionTooltip.text = "Press C to leave your PC!";
    }

    protected override void Update()
    {
        base.Update();
        if (stopLooking)
        {
            actionTooltip.enabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        DisplayTime();
    }


    public void OnClickInput(InputAction.CallbackContext context)
    {
        if (context.performed && looking)
            AudioManager.instance.PlaySound(clickSound, null);
    }

    private void DisplayTime()
    {
        string time = "0";
        time += GameManager.Instance.currentHour.ToString();

        time += ":";
        if (GameManager.Instance.currentTime < 10)
        {
            time += "0";
        }
        time += (int)GameManager.Instance.currentTime;
        time += " AM";

        clock.text = time.ToUpper();
    }

    private void OnMouseDown()
    {
        
    }
}
