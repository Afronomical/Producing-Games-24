using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShiftChange : InteractableTemplate
{
    public Transform startShiftPosition;
    public bool startShift;
    public GameObject openDoor, closedDoor, endShift;
    private Animation doorAnim;
    public AnimationClip openAnim, closeAnim;

    private void Awake()
    {
        doorAnim = openDoor.GetComponent<Animation>();
    }

    public override void Interact()
    {
        if (GameManager.Instance.player.GetComponent<PlayerInput>().enabled)
        {
            if (startShift && GameManager.Instance.inStudy)
            {
                GameManager.Instance.OpenDoor(transform);
            }
            else if (!startShift)
            {
                GameManager.Instance.CloseDoor();
            }
        }
    }

    public void ChangeShift()
    {
        StartCoroutine(AnimateDoor(startShift));
    }

    private IEnumerator AnimateDoor(bool open)
    {
        if (open)
        {
            openDoor.SetActive(true);
            closedDoor.SetActive(false);
            endShift.SetActive(true);
            doorAnim.clip = openAnim;
            doorAnim.Play();
            yield return new WaitForSeconds(0);
        }
        else if (!open)
        {
            doorAnim.clip = closeAnim;
            doorAnim.Play();
            yield return new WaitForSeconds(1);
            openDoor.SetActive(false);
            closedDoor.SetActive(true);
            endShift.SetActive(false);
        }
    }
}