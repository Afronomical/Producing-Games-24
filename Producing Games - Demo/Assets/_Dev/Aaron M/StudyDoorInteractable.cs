using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
                StartCoroutine(GameManager.Instance.StartShift(startShiftPosition));
            }
            else
            {
                StartCoroutine(GameManager.Instance.EndHour());
            }
        }
    }
}