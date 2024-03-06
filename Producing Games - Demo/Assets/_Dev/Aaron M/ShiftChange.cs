using UnityEngine;
using UnityEngine.InputSystem;

public class ShiftChange : InteractableTemplate
{
    public Transform startShiftPosition;
    public bool startShift;
    public GameObject openDoor, closedDoor, endShift;
   
    public override void Interact()
    {
        if (GameManager.Instance.player.GetComponent<PlayerInput>().enabled)
        {
            if (startShift && GameManager.Instance.inStudy)
            {
                GameManager.Instance.OpenDoor(startShiftPosition);
                openDoor.SetActive(true);
                closedDoor.SetActive(false);
                endShift.SetActive(true);
            }
            else if (!startShift)
            {
                GameManager.Instance.CloseDoor();
                openDoor.SetActive(false);
                closedDoor.SetActive(true);
                endShift.SetActive(false);
            }
        }
    }
}