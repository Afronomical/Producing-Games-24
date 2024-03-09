using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCDoorTrigger : MonoBehaviour
{
    [SerializeField]
    private DoorNPC door;
    private int agentsInRange = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            agentsInRange++;
            if (!door.isOpen)
            {
                door.OpenDoor(other.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            agentsInRange--;
            if (door.isOpen && agentsInRange == 0)
            {
                door.CloseDoor();
            }
        }
    }
}
