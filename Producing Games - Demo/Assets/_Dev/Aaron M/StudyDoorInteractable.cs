using UnityEngine;
using UnityEngine.InputSystem;

public class StudyDoorInteractable : InteractableTemplate
{
    public Transform startShiftPosition;
    public InteractiveObject startShiftSO, endHourSO;

    public void Awake()
    {
        collectible = startShiftSO;
    }

    public override void Interact()
    {
        if (GameManager.Instance.player.GetComponent<PlayerInput>().enabled)
        {
            if (GameManager.Instance.inStudy)
            {
                collectible = endHourSO;
                StartCoroutine(GameManager.Instance.StartShift(startShiftPosition));
            }
            else
            {
                collectible = startShiftSO;
                StartCoroutine(GameManager.Instance.EndHour());
            }
        }
    }
}
