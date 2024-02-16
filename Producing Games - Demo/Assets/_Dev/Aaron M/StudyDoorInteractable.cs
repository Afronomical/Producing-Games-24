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
            collectible = null;
            StartCoroutine(GameManager.Instance.StartShift(startShiftPosition));
        }
        else
        {
            collectible = null;
            StartCoroutine(GameManager.Instance.EndHour());
        }
    }
}
