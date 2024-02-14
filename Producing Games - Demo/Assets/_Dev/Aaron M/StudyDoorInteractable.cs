using UnityEngine;

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
        if (GameManager.Instance.inStudy)
        {
            collectible = endHourSO;
            GameManager.Instance.player.transform.position = startShiftPosition.position;
            GameManager.Instance.player.transform.rotation = startShiftPosition.rotation;
            GameManager.Instance.StartShift();
        }
        else
        {
            collectible = startShiftSO;
            GameManager.Instance.EndHour();
        }
    }
}
