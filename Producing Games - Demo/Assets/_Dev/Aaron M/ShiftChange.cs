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
            }
            else if (!startShift)
            {
                GameManager.Instance.CloseDoor();
            }
        }
    }

    public void ChangeShift()
    {
        if (startShift)
        {
            openDoor.SetActive(true);
            closedDoor.SetActive(false);
            endShift.SetActive(true);
        }
        else if (!startShift)
        {
            openDoor.SetActive(false);
            closedDoor.SetActive(true);
            endShift.SetActive(false);
        }
    }
}