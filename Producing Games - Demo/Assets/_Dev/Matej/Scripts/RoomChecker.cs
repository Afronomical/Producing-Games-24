using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChecker : MonoBehaviour
{
    [Tooltip("If false then its the enter room checker")] public bool isExitRoomChecker = false;

    private DoorInteractable doorInteractable;

    private void Start()
    {
        doorInteractable = transform.parent.GetComponent<DoorInteractable>();
        Debug.Log(doorInteractable != null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isExitRoomChecker)
        {

        }
        else
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isExitRoomChecker)
        {

        }
        else
        {

        }
    }
}
