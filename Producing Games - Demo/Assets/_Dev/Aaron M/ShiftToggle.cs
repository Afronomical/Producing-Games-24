using UnityEngine;
using UnityEngine.InputSystem;

public class ShiftToggle : InteractableTemplate
{
    public Transform startShiftPosition;
    public bool startShift;
   
    public override void Interact()
    {
        if (GameManager.Instance.player.GetComponent<PlayerInput>().enabled)
        {
            if (startShift && GameManager.Instance.inStudy)
            {
                StartCoroutine(GameManager.Instance.StartShift(startShiftPosition));
                // Open the door
                // No fade
                // No teleport
                // Remember to close it after shift
            }
            else if (!startShift)
            {
                StartCoroutine(GameManager.Instance.EndHour());
            }
        }
    }
}
