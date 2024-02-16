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
            StartCoroutine(GameManager.Instance.StartShift(startShiftPosition));
        }
        else
        {
            collectible = startShiftSO;
            StartCoroutine(GameManager.Instance.EndHour());
        }
    }
}
