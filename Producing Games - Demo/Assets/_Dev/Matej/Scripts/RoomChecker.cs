using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChecker : MonoBehaviour
{
    [Tooltip("If false then its the enter room checker")] 
    public bool isExitRoomChecker = false;

    public DoorInteractable doorInteractable;

    private void OnTriggerEnter(Collider other)
    {
        if (IsCharacter(other.gameObject))
        {
            if (isExitRoomChecker)
            {
                if (doorInteractable.entitiesInsideRoom.Contains(other.gameObject))
                    doorInteractable.entitiesInsideRoom.Remove(other.gameObject);
            }
            else
            {
                if (!doorInteractable.entitiesInsideRoom.Contains(other.gameObject))
                    doorInteractable.entitiesInsideRoom.Add(other.gameObject);
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (IsCharacter(other.gameObject))
    //    {
    //        if (isExitRoomChecker)
    //        {
    //            if (doorInteractable.entitiesInsideRoom.Contains(other.gameObject))
    //                doorInteractable.entitiesInsideRoom.Remove(other.gameObject);
    //        }
    //        else
    //        {
    //            if (!doorInteractable.entitiesInsideRoom.Contains(other.gameObject))
    //                doorInteractable.entitiesInsideRoom.Add(other.gameObject);
    //        }
    //    }
    //}

    private bool IsCharacter(GameObject gameObject)
    {
        if (gameObject == GameManager.Instance.player || gameObject == GameManager.Instance.demon)
            return true;
        return false;
    }
}
